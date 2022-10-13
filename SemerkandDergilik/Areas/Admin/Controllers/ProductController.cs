using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Models;
using Semerkand_Dergilik.Enums;
using Semerkand_Dergilik.ViewModels;
using Semerkand_Dergilik.VMClasses;
using System.Data;

namespace Semerkand_Dergilik.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Authorize(Roles = "Admin")] // case sensitive  
    public class ProductController : Controller
    {
        readonly IProductManager _ipm;

        public ProductController(IProductManager ipm) // services.AddRepManServices(); 
        {
            _ipm = ipm;
        }

        [Route("ProductIndex")]
        public IActionResult Index()
        {
            return View();
        }


        [Route("ProductList")]
        public async Task<IActionResult> ProductList(int id)
        {
            int category_id = id;

            IEnumerable<Product> productList = await _ipm.GetActivesProductsByCategoryIDAsync(category_id);

            ProductVM pvm = new ProductVM
            {
                Products = productList.Adapt<IEnumerable<ProductDTO>>().ToList(),

            };

            TempData["category_id"] = category_id;

            return View(pvm);
        }


        [Route("DeleteProductAjax")]
        public async Task<PartialViewResult> DeleteProductAjax(int id)
        {
            Product product_item = await _ipm.GetByIdAsync(id);
            ProductDTO pDTO = product_item.Adapt<ProductDTO>();


            ViewBag.CRUD = "delete_operation";

            return PartialView("_CrudProductPartial", pDTO);
        }


        [Route("CRUDProduct")]
        [HttpPost]
        public async Task<IActionResult> CRUDProduct(ProductDTO pdto, IFormFile productPicture)
        {
            if (TempData["Deleted"] == null)
            {
                ModelState.Remove("ProductPicture");

                if (ModelState.IsValid)
                {
                    Product prd = pdto.Adapt<Product>();

                    prd.Status = (int)pdto.Status; // casting bu olmadan dene



                    //////
                    ///
                    if (productPicture != null && productPicture.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productPicture.FileName); // path oluşturma

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductPicture", fileName); // server'a kayıt edilecek path => wwwroot/UserPicture/fileName

                        // kayıt işlemi
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await productPicture.CopyToAsync(stream); // userPicture'ı, stream'e kayıt

                            prd.ProductPicture = "/ProductPicture/" + fileName;   // veritabanına kayıt (wwwroot belirtmeye gerek yok)

                        }
                    }
                    else
                    {                    
                        // Update işleminde çalışır
                        if (pdto.ID != 0)
                        {
                            Product prdv2 = await _ipm.GetByIdAsync(pdto.ID);

                            if (prdv2.ProductPicture != null) // önceden veritabanında resim varsa ve resim seçilmedi ise..
                            {
                                prd.ProductPicture = prdv2.ProductPicture;
                            }
                        }

                    }

                    if (prd.ID == 0)
                    {
                        await _ipm.AddAsync(prd);
                    }
                    else
                    {
                        _ipm.Update(prd);

                    }

                    return RedirectToAction("ProductList", new { id = (int)TempData["category_id"] }); // comment'te alınırsa TempData["mesaj"] = "Ürün adı ve statü giriniz.."; da çalışır..
                }

            }
            else
            {
                _ipm.Delete(await _ipm.GetByIdAsync(pdto.ID));

                // Category ctg = cdto.Adapt<Category>();

                // _icm.Delete(ctg);

                TempData["mesaj"] = "Ürün silindi..";

                TempData["Deleted"] = null;

                //  return RedirectToAction("ProductList");
                return RedirectToAction("ProductList", new { id = (int)TempData["category_id"] });
            }

            TempData["mesaj"] = "Ürün adı ve statü giriniz..";
            //ModelState.AddModelError("", "Ürün adı ve statü giriniz..");

            return RedirectToAction("ProductList", new { id = (int)TempData["category_id"] });

        }




    }
}
