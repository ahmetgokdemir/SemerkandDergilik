using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Models;
using Semerkand_Dergilik.Enums;
using Semerkand_Dergilik.ViewModels;
using Semerkand_Dergilik.VMClasses;
using System.Collections;
using System.Data;

namespace Semerkand_Dergilik.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Authorize(Roles = "Admin")] // case sensitive  
    public class ProductController : Controller
    {
        readonly IProductManager _ipm;
        readonly ICategoryManager _icm;

        public ProductController(IProductManager ipm, ICategoryManager icm) // services.AddRepManServices(); 
        {
            _ipm = ipm;
            _icm = icm;
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

            IEnumerable<Product> productEnumerableList = await _ipm.GetActivesProductsByCategoryIDAsync(category_id);
            
            List<Product> productsList = new List<Product>();
            productsList = productEnumerableList.ToList();

            ProductVM pvm = new ProductVM
            {
                Products = productEnumerableList.Adapt<IEnumerable<ProductDTO>>().ToList(),

            };

            /*
            if (pvm.Products.Count > 0)
            {
                TempData["CategoryName"] = pvm.Products[0].Category.CategoryName;
                TempData["CategoryID"] = pvm.Products[0].Category.ID;
                TempData["CategoryStatus"] = pvm.Products[0].Category.Status;
                TempData["CategoryPicture"] = pvm.Products[0].Category.CategoryPicture;
            }
            */

            TempData["category_id"] = category_id;
            TempData["CategoryName"] = productsList[0].Category.CategoryName;

            return View(pvm);
        }


        [Route("AddProductAjax")]
        public async Task<PartialViewResult> AddProductAjax()
        {

            IEnumerable<string> categoryNames = await _icm.GetActivesCategoryNamesAsync();
            ViewBag.CategoryNames = new SelectList(categoryNames); // html kısmında select tag'ı kullanıldığı için SelectList kullanıldı

            // *string categoryNameAccordingToProduct = await _icm.GetCategoryNameAccordingToProductAsync((int)TempData["category_id"]);

            ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));
                        

            // ViewBag.CategoryName = categoryNameAccordingToProduct; --> asp-for, ViewBag kabul etmediği için pdto, cdto, cdto.CategoryName değerleri tanımlandı..
            // asp-for="Category.CategoryName" değer atamak için pdto, cdto, cdto.CategoryName değerleri tanımlandı..
            ProductDTO pdto = new ProductDTO(); 

            CategoryDTO cdto = new CategoryDTO(); // yazılmazsa null referance hatası verir.. 
            //cdto.CategoryName = categoryNameAccordingToProduct; // 2.yol
            
            cdto.CategoryName = TempData["CategoryName"].ToString();
            pdto.CategoryID = (int)TempData["category_id"];
            pdto.Category = cdto; // yazılmazsa null referance hatası verir.. 


            TempData["category_id"] = pdto.CategoryID;
            TempData["CategoryName"] = cdto.CategoryName; // 2.yol kullanılırsa gerekli olacak kod..

            return PartialView("_CrudProductPartial", pdto); // pdto değeri döndürmemizin nedeni cdto.CategoryName nin html'de dolu olması diğer değerler boş gelecek...
        }



        [Route("UpdateProductAjax")]
        public async Task<PartialViewResult> UpdateProductAjax(int id)
        {
            Product product_item = await _ipm.GetByIdAsync(id);

            // * Product product_item = await _ipm.GetProductByIdwithCategoryValueAsync(id);
            // yukarıdaki kod Ürünü, kategori bilgileri ile getirir buna gerek yok.. product_item.Adapt<ProductDTO>() yeterli

            ProductDTO pDTO = product_item.Adapt<ProductDTO>();

            /*
            string categoryNameAccordingToProduct = await _icm.GetCategoryNameAccordingToProductAsync((int)TempData["category_id"]);      
            */
            
            CategoryDTO cdto = new CategoryDTO(); // yazılmazsa cdto.CategoryName null referance hatası verir.. 
            cdto.CategoryName = TempData["CategoryName"].ToString(); // asp-for="Category.CategoryName" değer atamak için 
            // cdto.CategoryName = categoryNameAccordingToProduct;  2.yol
            pDTO.Category = cdto; // yazılmazsa null referance hatası verir.. 

            TempData["CategoryName"] = cdto.CategoryName;


            IEnumerable<string> categoryNames = await _icm.GetActivesCategoryNamesAsync();
            ViewBag.CategoryNames = new SelectList(categoryNames);


            ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));

            /*
             int category_id = (int)TempData["category_id"];
            TempData["category_id"] = category_id;
            */

            return PartialView("_CrudProductPartial", pDTO);
        }


        [Route("DeleteProductAjax")]
        public async Task<PartialViewResult> DeleteProductAjax(int id)
        {
            Product product_item = await _ipm.GetByIdAsync(id);
            ProductDTO pDTO = product_item.Adapt<ProductDTO>();


            ViewBag.CRUD = "delete_operation";
            ViewBag.ProductNameDelete = pDTO.ProductName;

            return PartialView("_CrudProductPartial", pDTO);
        }


        [Route("CRUDProduct")]
        [HttpPost]
        public async Task<IActionResult> CRUDProduct(ProductDTO pdto, IFormFile productPicture)
        {
            if (TempData["Deleted"] == null)
            {
                ModelState.Remove("ProductPicture");
                ModelState.Remove("Category");

               
                // CategoryDTO cdto = new CategoryDTO();// yazılmazsa null referance hatası verir.. 
                // // cdto.ID = (int) TempData["CategoryID"];
                // pdto.Category = cdto;
                // pdto.Category.ID = (int)TempData["CategoryID"];
                // pdto.Category.CategoryName = TempData["CategoryName"].ToString();
                // pdto.Category.Status = (Status)TempData["CategoryStatus"];
                // pdto.Category.CategoryPicture = TempData["CategoryPicture"].ToString();

              

                if (ModelState.IsValid)
                {
                    Product prd = pdto.Adapt<Product>();

                    prd.Status = (int)pdto.Status; // casting bu olmadan dene
                    //  <input type="hidden" asp-for="CategoryID" /> bunu kullandığımız için prd.CategoryID = (int)TempData["category_id"]; ama bu koda gerek kalmadı... zira ProductDTO'da CategoryID ile veriyi aldık.. 
                    // prd.Category = null;

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
                        TempData["mesaj"] = "Ürün eklendi";
                    }
                    else
                    {
                        _ipm.Update(prd);
                        TempData["mesaj"] = "Ürün güncellendi";


                    }

                    /*
                    int category_id = (int)TempData["category_id"];
                    TempData["category_id"] = category_id;
                    */

                    // var deneme = cdto.ID;
                    // TempData["CategoryID"] = cdto.ID;                    
                    // TempData["CategoryName"] = cdto.CategoryName;
                    // TempData["Status"] = (Status) cdto.Status; 
                    // TempData["CategoryPicture"] = cdto.CategoryPicture;

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
