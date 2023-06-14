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
        readonly ICategory_of_FoodManager _icm;

        public ProductController(IProductManager ipm, ICategory_of_FoodManager icm) // services.AddRepManServices(); 
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

            int Category_of_Food_id = id;
            int Category_of_Food_status = status;

            IEnumerable<Product> productEnumerableList = await _ipm.GetActivesProductsByCategory_of_FoodIDAsync(Category_of_Food_id);

            Category_of_Food c = await _icm.FirstOrDefault(x=>x.ID == Category_of_Food_id);

            List<Product> productsList = new List<Product>();
            productsList = productEnumerableList.ToList();

            ProductVM pvm = new ProductVM
            {
                ProductDTOs = productEnumerableList.Adapt<IEnumerable<ProductDTO>>().ToList(),
                JavascriptToRun = JSpopupPage,
                Category_of_FoodDTO = productsList.Count > 0 ? productsList[0].Category_of_Food.Adapt<Category_of_FoodDTO>() : null
            };

            /*
            if (pvm.Products.Count > 0)
            {
                TempData["Category_of_FoodName"] = pvm.Products[0].Category_of_Food.Category_of_FoodName;
                TempData["Category_of_FoodID"] = pvm.Products[0].Category_of_Food.ID;
                TempData["Category_of_FoodStatus"] = pvm.Products[0].Category_of_Food.Status;
                TempData["Category_of_FoodPicture"] = pvm.Products[0].Category_of_Food.Category_of_FoodPicture;
            }
            */
            //if (pvm.ProductDTOs.Count > 0)
            //{
            //    TempData["Category_of_FoodName"] = productsList[0].Category_of_Food.Category_of_FoodName;
            //}
            
            TempData["Category_of_FoodName"] = c.Category_of_FoodName;
            // TempData["Category_of_FoodName"] = productsList[0].Category_of_Food.Category_of_FoodName; --> ArgumentOutOfRangeException hatası


            TempData["Category_of_Food_id"] = Category_of_Food_id;
            TempData["Category_of_Food_status"] = Category_of_Food_status;

            return View(pvm);
        }


        [Route("AddProductAjax")]
        public async Task<PartialViewResult> AddProductAjax()
        {
            IEnumerable<string> Category_of_FoodNames = await _icm.GetActivesCategory_of_FoodNamesAsync();
            ViewBag.Category_of_FoodNames = new SelectList(Category_of_FoodNames); // html kısmında select tag'ı kullanıldığı için SelectList kullanıldı

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


            // *string Category_of_FoodNameAccordingToProduct = await _icm.GetCategory_of_FoodNameAccordingToProductAsync((int)TempData["Category_of_Food_id"]);

            // ViewBag.Category_of_FoodName = Category_of_FoodNameAccordingToProduct; --> asp-for, ViewBag kabul etmediği için pdto, cdto, cdto.Category_of_FoodName değerleri tanımlandı..
            // asp-for="Category_of_Food.Category_of_FoodName" değer atamak için pdto, cdto, cdto.Category_of_FoodName değerleri tanımlandı..



            Category_of_FoodDTO cdto = new Category_of_FoodDTO(); // yazılmazsa null referance hatası verir.. 
                                                  //cdto.Category_of_FoodName = Category_of_FoodNameAccordingToProduct; // 2.yol

            cdto.Category_of_FoodName = TempData["Category_of_FoodName"].ToString();
            cdto.ID = (int)TempData["Category_of_Food_id"];

            string kontrol = TempData["Category_of_Food_status"].ToString();
            cdto.Status = (Status)TempData["Category_of_Food_status"];


            // product'ın Category_of_Food_id'si 
            pdto.Category_of_FoodID = (int)TempData["Category_of_Food_id"]; // <input type="hidden" asp-for="ProductDTO.Category_of_FoodID" /> kısmı için bu kod gerekli..
            // pdto.Category_of_Food = cdto; // yazılmazsa null referance hatası verir.. 


            TempData["Category_of_Food_id"] = cdto.ID;
            TempData["Category_of_FoodName"] = cdto.Category_of_FoodName; // 2.yol kullanılırsa gerekli olacak kod..
            TempData["Category_of_Food_status"] = cdto.Status;

            ProductVM pvm = new ProductVM
            {
                Category_of_FoodDTO = cdto,
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

            return PartialView("_CrudProductPartial", pvm); // pdto değeri döndürmemizin nedeni cdto.Category_of_FoodName nin html'de dolu olması diğer değerler boş gelecek...


            /*else
            {
                 

                Category_of_FoodDTO cdto = new Category_of_FoodDTO(); // yazılmazsa null referance hatası verir.. 
                                                      //cdto.Category_of_FoodName = Category_of_FoodNameAccordingToProduct; // 2.yol

                cdto.Category_of_FoodName = TempData["Category_of_FoodName"].ToString();
                cdto.ID = (int)TempData["Category_of_Food_id"];

                TempData["Category_of_Food_id"] = cdto.ID;
                TempData["Category_of_FoodName"] = cdto.Category_of_FoodName; // 2.yol kullanılırsa gerekli olacak kod..

                ProductVM pvm = new ProductVM
                {
                    Category_of_FoodDTO = cdto,
                    ProductDTO =  pdto_reloaddata                

                };


                TempData["Category_of_Food_id"] = cdto.ID;
                TempData["Category_of_FoodName"] = cdto.Category_of_FoodName; // 2.yol kullanılırsa gerekli olacak kod..

                return PartialView("_CrudProductPartial", pvm); // pdto değeri döndürmemizin nedeni cdto.Category_of_FoodName nin html'de dolu olması diğer değerler boş gelecek...

            }
            */
        }



        [Route("UpdateProductAjax")]
        public async Task<PartialViewResult> UpdateProductAjax(int id)
        {
            IEnumerable<string> Category_of_FoodNames = await _icm.GetActivesCategory_of_FoodNamesAsync();
            ViewBag.Category_of_FoodNames = new SelectList(Category_of_FoodNames);

            ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));



            // Product product_item_control = await _ipm.GetProductByIdwithCategory_of_FoodValueAsync(id);
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
            string Category_of_FoodNameAccordingToProduct = await _icm.GetCategory_of_FoodNameAccordingToProductAsync((int)TempData["Category_of_Food_id"]);      
            */

            Category_of_FoodDTO cdto = new Category_of_FoodDTO(); // yazılmazsa cdto.Category_of_FoodName null referance hatası verir.. 
            cdto.Category_of_FoodName = TempData["Category_of_FoodName"].ToString(); // asp-for="Category_of_Food.Category_of_FoodName" değer atamak için 
            // cdto.Category_of_FoodName = Category_of_FoodNameAccordingToProduct;  2.yol
            // pDTO.Category_of_Food = cdto; // yazılmazsa null referance hatası verir.. 
            cdto.ID = (int)TempData["Category_of_Food_id"];

            string kontrol = TempData["Category_of_Food_status"].ToString();
            cdto.Status = (Status)TempData["Category_of_Food_status"];
             /*
             Product product_item = await _ipm.GetByIdAsync(id); kod sayesinde pdto.Category_of_FoodID geldiği için 
             AddProductAjax'taki gibi pdto.Category_of_FoodID = (int)TempData["Category_of_Food_id"]; koda gerek kalmadı...
            
             */


            ProductVM pvm = new ProductVM
            {
                ProductDTO = pdto,
                Category_of_FoodDTO = cdto
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
             int Category_of_Food_id = (int)TempData["Category_of_Food_id"];
            TempData["Category_of_Food_id"] = Category_of_Food_id;
            */
            TempData["Category_of_Food_id"] = cdto.ID;
            TempData["Category_of_FoodName"] = cdto.Category_of_FoodName; // 2.yol kullanılırsa gerekli olacak kod..
            TempData["Category_of_Food_status"] = cdto.Status;

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
                // ModelState.Remove("Category_of_Food");
                ModelState.Remove("ProductDTOs");
                ModelState.Remove("JavascriptToRun");
                ModelState.Remove("Category_of_FoodDTO");

            

                // Category_of_FoodDTO cdto = new Category_of_FoodDTO();// yazılmazsa null referance hatası verir.. 
                // // cdto.ID = (int) TempData["Category_of_FoodID"];
                // pdto.Category_of_Food = cdto;
                // pdto.Category_of_Food.ID = (int)TempData["Category_of_FoodID"];
                // pdto.Category_of_Food.Category_of_FoodName = TempData["Category_of_FoodName"].ToString();
                // pdto.Category_of_Food.Status = (Status)TempData["Category_of_FoodStatus"];
                // pdto.Category_of_Food.Category_of_FoodPicture = TempData["Category_of_FoodPicture"].ToString();



                if (ModelState.IsValid)
                {
                    Product prd = pvm_post.ProductDTO.Adapt<Product>();

                    //prd.Status = (int)pvm_post.ProductDTO.Status; // casting bu olmadan dene
                    
                    //  <input type="hidden" asp-for="Category_of_FoodID" /> bunu kullandığımız için prd.Category_of_FoodID = (int)TempData["Category_of_Food_id"]; ama bu koda gerek kalmadı... zira ProductDTO'da Category_of_FoodID ile veriyi aldık.. 
                    // prd.Category_of_Food = null;
                    // prd.Category_of_FoodID = pvm_post.Category_of_FoodDTO.ID; bu koda gerek kalmadı çünkü <input type="hidden" asp-for="ProductDTO.Category_of_FoodID" /> bunu kullandığımız için.. bunu da pdto.Category_of_FoodID = (int)TempData["Category_of_Food_id"]; bu kodla sağladık.. 

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
                    int Category_of_Food_id = (int)TempData["Category_of_Food_id"];
                    TempData["Category_of_Food_id"] = Category_of_Food_id;
                    */

                    // var deneme = cdto.ID;
                    // TempData["Category_of_FoodID"] = cdto.ID;                    
                    // TempData["Category_of_FoodName"] = cdto.Category_of_FoodName;
                    // TempData["Status"] = (Status) cdto.Status; 
                    // TempData["Category_of_FoodPicture"] = cdto.Category_of_FoodPicture;

                    return RedirectToAction("ProductList", new { id = (int)TempData["Category_of_Food_id"], status = (int)TempData["Category_of_Food_status"] }); // comment'te alınırsa TempData["mesaj"] = "Ürün adı ve statü giriniz.."; da çalışır..

                    /*
                        return RedirectToAction("ProductList", new { id = (int)TempData["Category_of_Food_id"], status = (int)TempData["Category_of_Food_status"], JSpopupPage = TempData["JSpopupPage"].ToString() });
                     
                     */


                }

            }
            else
            {
                // Product prd = pvm_post.ProductDTO.Adapt<Product>();

                // _ipm.Delete(prd);

                _ipm.Delete(await _ipm.GetByIdAsync(pvm_post.ProductDTO.ID));

                // Category_of_Food ctg = cdto.Adapt<Category_of_Food>();

                // _icm.Delete(ctg);

                TempData["messageProduct"] = "Ürün silindi";

                TempData["Deleted"] = null;

                //  return RedirectToAction("ProductList");
                return RedirectToAction("ProductList", new { id = (int)TempData["Category_of_Food_id"], status = (int)TempData["Category_of_Food_status"] });
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
                return RedirectToAction("ProductList", new { id = (int)TempData["Category_of_Food_id"], status = (int)TempData["Category_of_Food_status"], JSpopupPage = TempData["JSpopupPage"].ToString() });

            }
            else // add // (pvm_post.ProductDTO.ID == 0) çevir...
            {
                // pvm.JavascriptToRun = $"ShowErrorPopup( {pvm_post.ProductDTO} )";

                // pvm.JavascriptToRun = $"ShowErrorInsertOperationPopup()";

                TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";
                return RedirectToAction("ProductList", new { id = (int)TempData["Category_of_Food_id"], status = (int)TempData["Category_of_Food_status"], JSpopupPage = TempData["JSpopupPage"].ToString() });
            }




        }




    }
}
