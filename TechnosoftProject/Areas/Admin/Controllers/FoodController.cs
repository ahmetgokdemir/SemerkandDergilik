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
    [Route("Admin/Food")]
    [Authorize(Roles = "Admin")] // case sensitive  
    public class FoodController : Controller
    {
        readonly IFoodManager _ipm;
        readonly ICategory_of_FoodManager _icm;

        public FoodController(IFoodManager ipm, ICategory_of_FoodManager icm) // services.AddRepManServices(); 
        {
            _ipm = ipm;
            _icm = icm;
        }



        [Route("FoodIndex")]
        public IActionResult Index()
        {
            return View();
        }

        /*
            AppUser userv2 = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
            userViewModel.Gender = (Gender)userv2.Gender;
            user.Adapt<UserViewModel>();// bu kod cast işlemini yapıyor dolayısıyla ilk iki koda gerek kalmıyor..
         */


        [Route("FoodList")]
        // [HttpGet("{id}")] --> id parametresini, querystring'den almak yerine url'den almak ...
        public async Task<IActionResult> FoodList(int id,int status, string? JSpopupPage)
        {

            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null; // pop-up sıfırlanır yoksa sayfayı reflesleyince geliyor
            }

            int Category_of_Food_id = id;
            int Category_of_Food_status = status;

            Category_of_Food c = await _icm.FirstOrDefault(x => x.ID == Category_of_Food_id);

            IEnumerable<Food> FoodEnumerableList = await _ipm.Get_FoodsByCategoryID_Async(Category_of_Food_id);



            List<Food> FoodsList = new List<Food>();
            FoodsList = FoodEnumerableList.ToList();

            FoodVM pvm = new FoodVM
            {
                FoodDTOs = FoodEnumerableList.Adapt<IEnumerable<FoodDTO>>().ToList(),
                JavascriptToRun = JSpopupPage,
                Category_of_FoodDTO = FoodsList.Count > 0 ? /*FoodsList[0].Category_of_Food.Adapt<Category_of_FoodDTO>()*/ null : null // *** İsmail Bey - Değişiklikleri
            };

            /*
            if (pvm.Foods.Count > 0)
            {
                TempData["Category_of_FoodName"] = pvm.Foods[0].Category_of_Food.Category_of_FoodName;
                TempData["Category_of_FoodID"] = pvm.Foods[0].Category_of_Food.ID;
                TempData["Category_of_FoodStatus"] = pvm.Foods[0].Category_of_Food.Status;
                TempData["Category_of_FoodPicture"] = pvm.Foods[0].Category_of_Food.Category_of_FoodPicture;
            }
            */
            //if (pvm.FoodDTOs.Count > 0)
            //{
            //    TempData["Category_of_FoodName"] = FoodsList[0].Category_of_Food.Category_of_FoodName;
            //}
            
            TempData["Category_of_FoodName"] = c.Category_of_FoodName;
            // TempData["Category_of_FoodName"] = FoodsList[0].Category_of_Food.Category_of_FoodName; --> ArgumentOutOfRangeException hatası


            TempData["Category_of_Food_id"] = Category_of_Food_id;
            TempData["Category_of_Food_status"] = Category_of_Food_status;

            return View(pvm);
        }


        [Route("AddFoodAjax")]
        public async Task<PartialViewResult> AddFoodAjax()
        {
            IEnumerable<string> Category_of_FoodNames = await _icm.GetActivesCategory_of_FoodNamesAsync();
            ViewBag.Category_of_FoodNames = new SelectList(Category_of_FoodNames); // html kısmında select tag'ı kullanıldığı için SelectList kullanıldı

            ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));


            FoodDTO pdto = new FoodDTO();

            var result = new FoodDTO();


            // HttpContext.Session.SetObject("manipulatedData", pvm_post.FoodDTO);
            if (TempData["HttpContext"] != null)
            {
                pdto = new FoodDTO();
                result = HttpContext.Session.GetObject<FoodDTO>("manipulatedData");
                pdto = result;


            }
            /*
            if (result != null)
            {
                pdto = result;
            }
            */


            // *string Category_of_FoodNameAccordingToFood = await _icm.GetCategory_of_FoodNameAccordingToFoodAsync((int)TempData["Category_of_Food_id"]);

            // ViewBag.Category_of_FoodName = Category_of_FoodNameAccordingToFood; --> asp-for, ViewBag kabul etmediği için pdto, cdto, cdto.Category_of_FoodName değerleri tanımlandı..
            // asp-for="Category_of_Food.Category_of_FoodName" değer atamak için pdto, cdto, cdto.Category_of_FoodName değerleri tanımlandı..



            Category_of_FoodDTO cdto = new Category_of_FoodDTO(); // yazılmazsa null referance hatası verir.. 
                                                  //cdto.Category_of_FoodName = Category_of_FoodNameAccordingToFood; // 2.yol

            cdto.Category_of_FoodName = TempData["Category_of_FoodName"].ToString();
            cdto.ID = (int)TempData["Category_of_Food_id"];

            string kontrol = TempData["Category_of_Food_status"].ToString();
            cdto.Status = (Status)TempData["Category_of_Food_status"];


            // Food'ın Category_of_Food_id'si 
            pdto.Category_of_FoodID = (int)TempData["Category_of_Food_id"]; // <input type="hidden" asp-for="FoodDTO.Category_of_FoodID" /> kısmı için bu kod gerekli..
            // pdto.Category_of_Food = cdto; // yazılmazsa null referance hatası verir... 


            TempData["Category_of_Food_id"] = cdto.ID;
            TempData["Category_of_FoodName"] = cdto.Category_of_FoodName; // 2.yol kullanılırsa gerekli olacak kod..
            TempData["Category_of_Food_status"] = cdto.Status;

            FoodVM pvm = new FoodVM
            {
                Category_of_FoodDTO = cdto,
                FoodDTO = pdto
            };

            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(pvm.FoodDTO.FoodName))
                {
                    ModelState.AddModelError("FoodDTO.FoodName", "Ürün adı giriniz.");
                }
                if (pvm.FoodDTO.UnitPrice <= 0)
                {
                    ModelState.AddModelError("FoodDTO.UnitPrice", "Ürün fiyatı sıfırdan büyük sayısı olmalıdır.");
                }
                //if (pvm.FoodDTO.UnitsInStock <= 0)
                //{
                //    ModelState.AddModelError("FoodDTO.UnitsInStock", "Stok sayısı sıfırdan büyük sayısı olmalıdır.");
                //}
                if (pvm.FoodDTO.Discount < 0)
                {
                    ModelState.AddModelError("FoodDTO.Discount", "İskonto negatif sayı olamaz.");
                }

                HttpContext.Session.SetObject("manipulatedData", null);
            }

            Thread.Sleep(500);

            return PartialView("_CrudFoodPartial", pvm); // pdto değeri döndürmemizin nedeni cdto.Category_of_FoodName nin html'de dolu olması diğer değerler boş gelecek...


            /*else
            {
                 

                Category_of_FoodDTO cdto = new Category_of_FoodDTO(); // yazılmazsa null referance hatası verir.. 
                                                      //cdto.Category_of_FoodName = Category_of_FoodNameAccordingToFood; // 2.yol

                cdto.Category_of_FoodName = TempData["Category_of_FoodName"].ToString();
                cdto.ID = (int)TempData["Category_of_Food_id"];

                TempData["Category_of_Food_id"] = cdto.ID;
                TempData["Category_of_FoodName"] = cdto.Category_of_FoodName; // 2.yol kullanılırsa gerekli olacak kod..

                FoodVM pvm = new FoodVM
                {
                    Category_of_FoodDTO = cdto,
                    FoodDTO =  pdto_reloaddata                

                };


                TempData["Category_of_Food_id"] = cdto.ID;
                TempData["Category_of_FoodName"] = cdto.Category_of_FoodName; // 2.yol kullanılırsa gerekli olacak kod..

                return PartialView("_CrudFoodPartial", pvm); // pdto değeri döndürmemizin nedeni cdto.Category_of_FoodName nin html'de dolu olması diğer değerler boş gelecek...

            }
            */
        }



        [Route("UpdateFoodAjax")]
        public async Task<PartialViewResult> UpdateFoodAjax(int id)
        {
            IEnumerable<string> Category_of_FoodNames = await _icm.GetActivesCategory_of_FoodNamesAsync();
            ViewBag.Category_of_FoodNames = new SelectList(Category_of_FoodNames);

            ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));



            // Food Food_item_control = await _ipm.GetFoodByIdwithCategory_of_FoodValueAsync(id);
            // yukarıdaki kod Ürünü, kategori bilgileri ile getirir buna gerek yok.. Food_item.Adapt<FoodDTO>() yeterli



            FoodDTO pdto = new FoodDTO();

            var result = new FoodDTO();

            if (TempData["HttpContext"] != null)
            {
                result = HttpContext.Session.GetObject<FoodDTO>("manipulatedData");
                pdto = result;

                // HttpContext.Session.SetObject("manipulatedData", null);
            }
            else
            {
                Food Food_item = await _ipm.GetByIdAsync(id);

                pdto = Food_item.Adapt<FoodDTO>();
            }




            /*
            string Category_of_FoodNameAccordingToFood = await _icm.GetCategory_of_FoodNameAccordingToFoodAsync((int)TempData["Category_of_Food_id"]);      
            */

            Category_of_FoodDTO cdto = new Category_of_FoodDTO(); // yazılmazsa cdto.Category_of_FoodName null referance hatası verir.. 
            cdto.Category_of_FoodName = TempData["Category_of_FoodName"].ToString(); // asp-for="Category_of_Food.Category_of_FoodName" değer atamak için 
            // cdto.Category_of_FoodName = Category_of_FoodNameAccordingToFood;  2.yol
            // pDTO.Category_of_Food = cdto; // yazılmazsa null referance hatası verir.. 
            cdto.ID = (int)TempData["Category_of_Food_id"];

            string kontrol = TempData["Category_of_Food_status"].ToString();
            cdto.Status = (Status)TempData["Category_of_Food_status"];
             /*
             Food Food_item = await _ipm.GetByIdAsync(id); kod sayesinde pdto.Category_of_FoodID geldiği için 
             AddFoodAjax'taki gibi pdto.Category_of_FoodID = (int)TempData["Category_of_Food_id"]; koda gerek kalmadı...
            
             */


            FoodVM pvm = new FoodVM
            {
                FoodDTO = pdto,
                Category_of_FoodDTO = cdto
            };

            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(pvm.FoodDTO.FoodName))
                {
                    ModelState.AddModelError("FoodDTO.FoodName", "Ürün adı giriniz.");
                }
                if (pdto.UnitPrice <= 0)
                {
                    ModelState.AddModelError("FoodDTO.UnitPrice", "Ürün fiyatı sıfırdan büyük sayısı olmalıdır.");
                }
                //if (pdto.UnitsInStock <= 0)
                //{
                //    ModelState.AddModelError("FoodDTO.UnitsInStock", "Stok sayısı sıfırdan büyük sayısı olmalıdır.");
                //}
                if (pvm.FoodDTO.Discount < 0)
                {
                    ModelState.AddModelError("FoodDTO.Discount", "İskonto negatif sayı olamaz.");
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

            return PartialView("_CrudFoodPartial", pvm);
        }


        [Route("DeleteFoodAjax")]
        public async Task<PartialViewResult> DeleteFoodAjax(int id)
        {
            Food Food_item = await _ipm.GetByIdAsync(id);
            // FoodDTO pDTO = Food_item.Adapt<FoodDTO>();

            FoodVM pvm = new FoodVM
            {
                FoodDTO = Food_item.Adapt<FoodDTO>()
            };

            ViewBag.CRUD = "delete_operation";
            ViewBag.FoodNameDelete = pvm.FoodDTO.FoodName;

            return PartialView("_CrudFoodPartial", pvm);
        }


        [Route("CRUDFood")]
        [HttpPost]
        public async Task<IActionResult> CRUDFood(FoodVM pvm_post, IFormFile FoodPicture)
        {
            if (TempData["Deleted"] == null)
            {
                ModelState.Remove("FoodPicture");
                // ModelState.Remove("Category_of_Food");
                ModelState.Remove("FoodDTOs");
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
                    Food prd = pvm_post.FoodDTO.Adapt<Food>();

                    //prd.Status = (int)pvm_post.FoodDTO.Status; // casting bu olmadan dene
                    
                    //  <input type="hidden" asp-for="Category_of_FoodID" /> bunu kullandığımız için prd.Category_of_FoodID = (int)TempData["Category_of_Food_id"]; ama bu koda gerek kalmadı... zira FoodDTO'da Category_of_FoodID ile veriyi aldık.. 
                    // prd.Category_of_Food = null;
                    // prd.Category_of_FoodID = pvm_post.Category_of_FoodDTO.ID; bu koda gerek kalmadı çünkü <input type="hidden" asp-for="FoodDTO.Category_of_FoodID" /> bunu kullandığımız için.. bunu da pdto.Category_of_FoodID = (int)TempData["Category_of_Food_id"]; bu kodla sağladık.. 

                    //////
                    ///
                    if (FoodPicture != null && FoodPicture.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(FoodPicture.FileName); // path oluşturma

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/FoodPicture", fileName); // server'a kayıt edilecek path => wwwroot/UserPicture/fileName

                        // kayıt işlemi
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await FoodPicture.CopyToAsync(stream); // userPicture'ı, stream'e kayıt

                            prd.FoodPicture = "/FoodPicture/" + fileName;   // veritabanına kayıt (wwwroot belirtmeye gerek yok)

                        }
                    }
                    else
                    {
                        // Update işleminde çalışır
                        if (pvm_post.FoodDTO.ID != 0)
                        {
                            Food prdv2 = await _ipm.GetByIdAsync(pvm_post.FoodDTO.ID);

                            if (prdv2.FoodPicture != null) // önceden veritabanında resim varsa ve resim seçilmedi ise..
                            {
                                prd.FoodPicture = prdv2.FoodPicture;
                            }
                        }

                    }

                    if (prd.ID == 0)
                    {
                        await _ipm.AddAsync(prd);
                        TempData["messageFood"] = "Ürün eklendi";
                    }
                    else
                    {
                        _ipm.Update(prd);
                        TempData["messageFood"] = "Ürün güncellendi";
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

                    return RedirectToAction("FoodList", new { id = (int)TempData["Category_of_Food_id"], status = (int)TempData["Category_of_Food_status"] }); // comment'te alınırsa TempData["mesaj"] = "Ürün adı ve statü giriniz.."; da çalışır..

                    /*
                        return RedirectToAction("FoodList", new { id = (int)TempData["Category_of_Food_id"], status = (int)TempData["Category_of_Food_status"], JSpopupPage = TempData["JSpopupPage"].ToString() });
                     
                     */


                }

            }
            else
            {
                // Food prd = pvm_post.FoodDTO.Adapt<Food>();

                // _ipm.Delete(prd);

                _ipm.Delete(await _ipm.GetByIdAsync(pvm_post.FoodDTO.ID));

                // Category_of_Food ctg = cdto.Adapt<Category_of_Food>();

                // _icm.Delete(ctg);

                TempData["messageFood"] = "Ürün silindi";

                TempData["Deleted"] = null;

                //  return RedirectToAction("FoodList");
                return RedirectToAction("FoodList", new { id = (int)TempData["Category_of_Food_id"], status = (int)TempData["Category_of_Food_status"] });
            }

            // TempData["mesaj"] = "Ürün adı ve statü giriniz..";
            // ModelState.AddModelError("", "Ürünasdasd adı ve statü giriniz..");
            //ModelState.AddModelError("", item.Description);


            FoodVM pvm = new FoodVM();

            // TempData["manipulatedData"] = pvm_post.FoodDTO;
            // var key = "manipulatedData";
            //var str = JsonConvert.SerializeObject(pvm_post.FoodDTO);
            HttpContext.Session.SetObject("manipulatedData", pvm_post.FoodDTO);

            TempData["JavascriptToRun"] = "valid";
            TempData["HttpContext"] = "valid";

            if (pvm_post.FoodDTO.ID != 0) //update
            {
                TempData["JSpopupPage"] = $"ShowErrorUpdateOperationPopup({pvm_post.FoodDTO.ID})";
                return RedirectToAction("FoodList", new { id = (int)TempData["Category_of_Food_id"], status = (int)TempData["Category_of_Food_status"], JSpopupPage = TempData["JSpopupPage"].ToString() });

            }
            else // add // (pvm_post.FoodDTO.ID == 0) çevir...
            {
                // pvm.JavascriptToRun = $"ShowErrorPopup( {pvm_post.FoodDTO} )";

                // pvm.JavascriptToRun = $"ShowErrorInsertOperationPopup()";

                TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";
                return RedirectToAction("FoodList", new { id = (int)TempData["Category_of_Food_id"], status = (int)TempData["Category_of_Food_status"], JSpopupPage = TempData["JSpopupPage"].ToString() });
            }




        }




    }
}
