using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Models;
using Technosoft_Project.CommonTools;
using Technosoft_Project.Enums;
using Technosoft_Project.ViewModels;
using Technosoft_Project.VMClasses;
using System;
using System.Collections;
using System.Data;
using System.Xml.Linq;

namespace Technosoft_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Product")]
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

        /*
            AppUser userv2 = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
            userViewModel.Gender = (Gender)userv2.Gender;
            user.Adapt<UserViewModel>();// bu kod cast işlemini yapıyor dolayısıyla ilk iki koda gerek kalmıyor..
         */


        [Route("ProductList")]
        // [HttpGet("{id}")] --> id parametresini, querystring'den almak yerine url'den almak ...
        public async Task<IActionResult> ProductList(int id,int status, string? JSpopupPage)
        {

            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null; // pop-up sıfırlanır yoksa sayfayı reflesleyince geliyor
            }

            int category_id = id;
            int category_status = status;

            IEnumerable<Product> productEnumerableList = await _ipm.GetActivesProductsByCategoryIDAsync(category_id);

            Category c = await _icm.FirstOrDefault(x=>x.ID == category_id);

            List<Product> productsList = new List<Product>();
            productsList = productEnumerableList.ToList();

            ProductVM pvm = new ProductVM
            {
                ProductDTOs = productEnumerableList.Adapt<IEnumerable<ProductDTO>>().ToList(),
                JavascriptToRun = JSpopupPage,
                CategoryDTO = productsList.Count > 0 ? productsList[0].Category.Adapt<CategoryDTO>() : null
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
            //if (pvm.ProductDTOs.Count > 0)
            //{
            //    TempData["CategoryName"] = productsList[0].Category.CategoryName;
            //}
            
            TempData["CategoryName"] = c.CategoryName;
            // TempData["CategoryName"] = productsList[0].Category.CategoryName; --> ArgumentOutOfRangeException hatası


            TempData["category_id"] = category_id;
            TempData["category_status"] = category_status;

            return View(pvm);
        }


        [Route("AddProductAjax")]
        public async Task<PartialViewResult> AddProductAjax()
        {
            IEnumerable<string> categoryNames = await _icm.GetActivesCategoryNamesAsync();
            ViewBag.CategoryNames = new SelectList(categoryNames); // html kısmında select tag'ı kullanıldığı için SelectList kullanıldı

            ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));


            ProductDTO pdto = new ProductDTO();

            var result = new ProductDTO();


            // HttpContext.Session.SetObject("manipulatedData", pvm_post.ProductDTO);
            if (TempData["HttpContext"] != null)
            {
                pdto = new ProductDTO();
                result = HttpContext.Session.GetObject<ProductDTO>("manipulatedData");
                pdto = result;


            }
            /*
            if (result != null)
            {
                pdto = result;
            }
            */


            // *string categoryNameAccordingToProduct = await _icm.GetCategoryNameAccordingToProductAsync((int)TempData["category_id"]);

            // ViewBag.CategoryName = categoryNameAccordingToProduct; --> asp-for, ViewBag kabul etmediği için pdto, cdto, cdto.CategoryName değerleri tanımlandı..
            // asp-for="Category.CategoryName" değer atamak için pdto, cdto, cdto.CategoryName değerleri tanımlandı..



            CategoryDTO cdto = new CategoryDTO(); // yazılmazsa null referance hatası verir.. 
                                                  //cdto.CategoryName = categoryNameAccordingToProduct; // 2.yol

            cdto.CategoryName = TempData["CategoryName"].ToString();
            cdto.ID = (int)TempData["category_id"];

            string kontrol = TempData["category_status"].ToString();
            cdto.Status = (Status)TempData["category_status"];


            // product'ın category_id'si 
            pdto.CategoryID = (int)TempData["category_id"]; // <input type="hidden" asp-for="ProductDTO.CategoryID" /> kısmı için bu kod gerekli..
            // pdto.Category = cdto; // yazılmazsa null referance hatası verir.. 


            TempData["category_id"] = cdto.ID;
            TempData["CategoryName"] = cdto.CategoryName; // 2.yol kullanılırsa gerekli olacak kod..
            TempData["category_status"] = cdto.Status;

            ProductVM pvm = new ProductVM
            {
                CategoryDTO = cdto,
                ProductDTO = pdto
            };

            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(pvm.ProductDTO.ProductName))
                {
                    ModelState.AddModelError("ProductDTO.ProductName", "Ürün adı giriniz.");
                }
                if (pvm.ProductDTO.UnitPrice <= 0)
                {
                    ModelState.AddModelError("ProductDTO.UnitPrice", "Ürün fiyatı sıfırdan büyük sayısı olmalıdır.");
                }
                //if (pvm.ProductDTO.UnitsInStock <= 0)
                //{
                //    ModelState.AddModelError("ProductDTO.UnitsInStock", "Stok sayısı sıfırdan büyük sayısı olmalıdır.");
                //}
                if (pvm.ProductDTO.Discount < 0)
                {
                    ModelState.AddModelError("ProductDTO.Discount", "İskonto negatif sayı olamaz.");
                }

                HttpContext.Session.SetObject("manipulatedData", null);
            }

            Thread.Sleep(500);

            return PartialView("_CrudProductPartial", pvm); // pdto değeri döndürmemizin nedeni cdto.CategoryName nin html'de dolu olması diğer değerler boş gelecek...


            /*else
            {
                 

                CategoryDTO cdto = new CategoryDTO(); // yazılmazsa null referance hatası verir.. 
                                                      //cdto.CategoryName = categoryNameAccordingToProduct; // 2.yol

                cdto.CategoryName = TempData["CategoryName"].ToString();
                cdto.ID = (int)TempData["category_id"];

                TempData["category_id"] = cdto.ID;
                TempData["CategoryName"] = cdto.CategoryName; // 2.yol kullanılırsa gerekli olacak kod..

                ProductVM pvm = new ProductVM
                {
                    CategoryDTO = cdto,
                    ProductDTO =  pdto_reloaddata                

                };


                TempData["category_id"] = cdto.ID;
                TempData["CategoryName"] = cdto.CategoryName; // 2.yol kullanılırsa gerekli olacak kod..

                return PartialView("_CrudProductPartial", pvm); // pdto değeri döndürmemizin nedeni cdto.CategoryName nin html'de dolu olması diğer değerler boş gelecek...

            }
            */
        }



        [Route("UpdateProductAjax")]
        public async Task<PartialViewResult> UpdateProductAjax(int id)
        {
            IEnumerable<string> categoryNames = await _icm.GetActivesCategoryNamesAsync();
            ViewBag.CategoryNames = new SelectList(categoryNames);

            ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));



            // Product product_item_control = await _ipm.GetProductByIdwithCategoryValueAsync(id);
            // yukarıdaki kod Ürünü, kategori bilgileri ile getirir buna gerek yok.. product_item.Adapt<ProductDTO>() yeterli



            ProductDTO pdto = new ProductDTO();

            var result = new ProductDTO();

            if (TempData["HttpContext"] != null)
            {
                result = HttpContext.Session.GetObject<ProductDTO>("manipulatedData");
                pdto = result;

                // HttpContext.Session.SetObject("manipulatedData", null);
            }
            else
            {
                Product product_item = await _ipm.GetByIdAsync(id);

                pdto = product_item.Adapt<ProductDTO>();
            }




            /*
            string categoryNameAccordingToProduct = await _icm.GetCategoryNameAccordingToProductAsync((int)TempData["category_id"]);      
            */

            CategoryDTO cdto = new CategoryDTO(); // yazılmazsa cdto.CategoryName null referance hatası verir.. 
            cdto.CategoryName = TempData["CategoryName"].ToString(); // asp-for="Category.CategoryName" değer atamak için 
            // cdto.CategoryName = categoryNameAccordingToProduct;  2.yol
            // pDTO.Category = cdto; // yazılmazsa null referance hatası verir.. 
            cdto.ID = (int)TempData["category_id"];

            string kontrol = TempData["category_status"].ToString();
            cdto.Status = (Status)TempData["category_status"];
             /*
             Product product_item = await _ipm.GetByIdAsync(id); kod sayesinde pdto.CategoryID geldiği için 
             AddProductAjax'taki gibi pdto.CategoryID = (int)TempData["category_id"]; koda gerek kalmadı...
            
             */


            ProductVM pvm = new ProductVM
            {
                ProductDTO = pdto,
                CategoryDTO = cdto
            };

            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(pvm.ProductDTO.ProductName))
                {
                    ModelState.AddModelError("ProductDTO.ProductName", "Ürün adı giriniz.");
                }
                if (pdto.UnitPrice <= 0)
                {
                    ModelState.AddModelError("ProductDTO.UnitPrice", "Ürün fiyatı sıfırdan büyük sayısı olmalıdır.");
                }
                //if (pdto.UnitsInStock <= 0)
                //{
                //    ModelState.AddModelError("ProductDTO.UnitsInStock", "Stok sayısı sıfırdan büyük sayısı olmalıdır.");
                //}
                if (pvm.ProductDTO.Discount < 0)
                {
                    ModelState.AddModelError("ProductDTO.Discount", "İskonto negatif sayı olamaz.");
                }

                HttpContext.Session.SetObject("manipulatedData", null);
            }

            /*
             int category_id = (int)TempData["category_id"];
            TempData["category_id"] = category_id;
            */
            TempData["category_id"] = cdto.ID;
            TempData["CategoryName"] = cdto.CategoryName; // 2.yol kullanılırsa gerekli olacak kod..
            TempData["category_status"] = cdto.Status;

            Thread.Sleep(500);

            return PartialView("_CrudProductPartial", pvm);
        }


        [Route("DeleteProductAjax")]
        public async Task<PartialViewResult> DeleteProductAjax(int id)
        {
            Product product_item = await _ipm.GetByIdAsync(id);
            // ProductDTO pDTO = product_item.Adapt<ProductDTO>();

            ProductVM pvm = new ProductVM
            {
                ProductDTO = product_item.Adapt<ProductDTO>()
            };

            ViewBag.CRUD = "delete_operation";
            ViewBag.ProductNameDelete = pvm.ProductDTO.ProductName;

            return PartialView("_CrudProductPartial", pvm);
        }


        [Route("CRUDProduct")]
        [HttpPost]
        public async Task<IActionResult> CRUDProduct(ProductVM pvm_post, IFormFile productPicture)
        {
            if (TempData["Deleted"] == null)
            {
                ModelState.Remove("ProductPicture");
                // ModelState.Remove("Category");
                ModelState.Remove("ProductDTOs");
                ModelState.Remove("JavascriptToRun");
                ModelState.Remove("CategoryDTO");

            

                // CategoryDTO cdto = new CategoryDTO();// yazılmazsa null referance hatası verir.. 
                // // cdto.ID = (int) TempData["CategoryID"];
                // pdto.Category = cdto;
                // pdto.Category.ID = (int)TempData["CategoryID"];
                // pdto.Category.CategoryName = TempData["CategoryName"].ToString();
                // pdto.Category.Status = (Status)TempData["CategoryStatus"];
                // pdto.Category.CategoryPicture = TempData["CategoryPicture"].ToString();



                if (ModelState.IsValid)
                {
                    Product prd = pvm_post.ProductDTO.Adapt<Product>();

                    //prd.Status = (int)pvm_post.ProductDTO.Status; // casting bu olmadan dene
                    
                    //  <input type="hidden" asp-for="CategoryID" /> bunu kullandığımız için prd.CategoryID = (int)TempData["category_id"]; ama bu koda gerek kalmadı... zira ProductDTO'da CategoryID ile veriyi aldık.. 
                    // prd.Category = null;
                    // prd.CategoryID = pvm_post.CategoryDTO.ID; bu koda gerek kalmadı çünkü <input type="hidden" asp-for="ProductDTO.CategoryID" /> bunu kullandığımız için.. bunu da pdto.CategoryID = (int)TempData["category_id"]; bu kodla sağladık.. 

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
                        if (pvm_post.ProductDTO.ID != 0)
                        {
                            Product prdv2 = await _ipm.GetByIdAsync(pvm_post.ProductDTO.ID);

                            if (prdv2.ProductPicture != null) // önceden veritabanında resim varsa ve resim seçilmedi ise..
                            {
                                prd.ProductPicture = prdv2.ProductPicture;
                            }
                        }

                    }

                    if (prd.ID == 0)
                    {
                        await _ipm.AddAsync(prd);
                        TempData["messageProduct"] = "Ürün eklendi";
                    }
                    else
                    {
                        _ipm.Update(prd);
                        TempData["messageProduct"] = "Ürün güncellendi";
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

                    return RedirectToAction("ProductList", new { id = (int)TempData["category_id"], status = (int)TempData["category_status"] }); // comment'te alınırsa TempData["mesaj"] = "Ürün adı ve statü giriniz.."; da çalışır..

                    /*
                        return RedirectToAction("ProductList", new { id = (int)TempData["category_id"], status = (int)TempData["category_status"], JSpopupPage = TempData["JSpopupPage"].ToString() });
                     
                     */


                }

            }
            else
            {
                // Product prd = pvm_post.ProductDTO.Adapt<Product>();

                // _ipm.Delete(prd);

                _ipm.Delete(await _ipm.GetByIdAsync(pvm_post.ProductDTO.ID));

                // Category ctg = cdto.Adapt<Category>();

                // _icm.Delete(ctg);

                TempData["messageProduct"] = "Ürün silindi";

                TempData["Deleted"] = null;

                //  return RedirectToAction("ProductList");
                return RedirectToAction("ProductList", new { id = (int)TempData["category_id"], status = (int)TempData["category_status"] });
            }

            // TempData["mesaj"] = "Ürün adı ve statü giriniz..";
            // ModelState.AddModelError("", "Ürünasdasd adı ve statü giriniz..");
            //ModelState.AddModelError("", item.Description);


            ProductVM pvm = new ProductVM();

            // TempData["manipulatedData"] = pvm_post.ProductDTO;
            // var key = "manipulatedData";
            //var str = JsonConvert.SerializeObject(pvm_post.ProductDTO);
            HttpContext.Session.SetObject("manipulatedData", pvm_post.ProductDTO);

            TempData["JavascriptToRun"] = "valid";
            TempData["HttpContext"] = "valid";

            if (pvm_post.ProductDTO.ID != 0) //update
            {
                TempData["JSpopupPage"] = $"ShowErrorUpdateOperationPopup({pvm_post.ProductDTO.ID})";
                return RedirectToAction("ProductList", new { id = (int)TempData["category_id"], status = (int)TempData["category_status"], JSpopupPage = TempData["JSpopupPage"].ToString() });

            }
            else // add // (pvm_post.ProductDTO.ID == 0) çevir...
            {
                // pvm.JavascriptToRun = $"ShowErrorPopup( {pvm_post.ProductDTO} )";

                // pvm.JavascriptToRun = $"ShowErrorInsertOperationPopup()";

                TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";
                return RedirectToAction("ProductList", new { id = (int)TempData["category_id"], status = (int)TempData["category_status"], JSpopupPage = TempData["JSpopupPage"].ToString() });
            }




        }




    }
}
