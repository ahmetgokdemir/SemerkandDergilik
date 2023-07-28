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
using Technosoft_Project.ViewModels;
using Technosoft_Project.VMClasses;
using System;
using System.Collections;
using System.Data;
using System.Xml.Linq;
using Project.ENTITIES.Enums;

namespace Technosoft_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Food")]
    [Authorize(Roles = "Admin")] // case sensitive  
    public class FoodController : Controller
    {
        readonly IFoodManager _ipm;
        readonly ICategoryofFoodManager _icm;

        public FoodController(IFoodManager ipm, ICategoryofFoodManager icm) // services.AddRepManServices(); 
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

            int CategoryofFood_id = id;
            int CategoryofFood_status = status;

            CategoryofFood c = await _icm.FirstOrDefault(x => x.ID == CategoryofFood_id);

            IEnumerable<Food> FoodEnumerableList = await _ipm.Get_FoodsByCategoryID_Async(CategoryofFood_id);



            List<Food> FoodsList = new List<Food>();
            FoodsList = FoodEnumerableList.ToList();

            FoodVM pvm = new FoodVM
            {
                FoodDTOs = FoodEnumerableList.Adapt<IEnumerable<FoodDTO>>().ToList(),
                JavascriptToRun = JSpopupPage,
                CategoryofFoodDTO = FoodsList.Count > 0 ? /*FoodsList[0].CategoryofFood.Adapt<CategoryofFoodDTO>()*/ null : null // *** İsmail Bey - Değişiklikleri
            };

            /*
            if (pvm.Foods.Count > 0)
            {
                TempData["CategoryofFoodName"] = pvm.Foods[0].CategoryofFood.CategoryofFoodName;
                TempData["CategoryofFoodID"] = pvm.Foods[0].CategoryofFood.ID;
                TempData["CategoryofFoodStatus"] = pvm.Foods[0].CategoryofFood.Status;
                TempData["CategoryofFoodPicture"] = pvm.Foods[0].CategoryofFood.CategoryofFoodPicture;
            }
            */
            //if (pvm.FoodDTOs.Count > 0)
            //{
            //    TempData["CategoryofFoodName"] = FoodsList[0].CategoryofFood.CategoryofFoodName;
            //}
            
            TempData["CategoryofFoodName"] = c.CategoryName_of_Foods;
            // TempData["CategoryofFoodName"] = FoodsList[0].CategoryofFood.CategoryofFoodName; --> ArgumentOutOfRangeException hatası


            TempData["CategoryofFood_id"] = CategoryofFood_id;
            TempData["CategoryofFood_status"] = CategoryofFood_status;

            return View(pvm);
        }


        [Route("AddFoodAjax")]
        public async Task<PartialViewResult> AddFoodAjax()
        {
            IEnumerable<string> CategoryofFoodNames = await _icm.GetActivesCategoryofFoodNamesAsync();
            ViewBag.CategoryofFoodNames = new SelectList(CategoryofFoodNames); // html kısmında select tag'ı kullanıldığı için SelectList kullanıldı

            ViewBag.Status = new SelectList(Enum.GetNames(typeof(ExistentStatus)));


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


            // *string CategoryofFoodNameAccordingToFood = await _icm.GetCategoryofFoodNameAccordingToFoodAsync((int)TempData["CategoryofFood_id"]);

            // ViewBag.CategoryofFoodName = CategoryofFoodNameAccordingToFood; --> asp-for, ViewBag kabul etmediği için pdto, cdto, cdto.CategoryofFoodName değerleri tanımlandı..
            // asp-for="CategoryofFood.CategoryofFoodName" değer atamak için pdto, cdto, cdto.CategoryofFoodName değerleri tanımlandı..



            CategoryofFoodDTO cdto = new CategoryofFoodDTO(); // yazılmazsa null referance hatası verir.. 
                                                  //cdto.CategoryofFoodName = CategoryofFoodNameAccordingToFood; // 2.yol

            cdto.CategoryName_of_Foods = TempData["CategoryofFoodName"].ToString();
            cdto.ID = (int)TempData["CategoryofFood_id"];

            string kontrol = TempData["CategoryofFood_status"].ToString();
            cdto.ExistentStatus = (ExistentStatus)TempData["CategoryofFood_status"];


            // Food'ın CategoryofFood_id'si 
            pdto.CategoryofFoodID = (int)TempData["CategoryofFood_id"]; // <input type="hidden" asp-for="FoodDTO.CategoryofFoodID" /> kısmı için bu kod gerekli..
            // pdto.CategoryofFood = cdto; // yazılmazsa null referance hatası verir... 


            TempData["CategoryofFood_id"] = cdto.ID;
            TempData["CategoryofFoodName"] = cdto.CategoryName_of_Foods; // 2.yol kullanılırsa gerekli olacak kod..
            TempData["CategoryofFood_status"] = cdto.ExistentStatus;

            FoodVM pvm = new FoodVM
            {
                CategoryofFoodDTO = cdto,
                FoodDTO = pdto
            };

            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(pvm.FoodDTO.FoodName))
                {
                    ModelState.AddModelError("FoodDTO.FoodName", "Ürün adı giriniz.");
                }
                if (pvm.FoodDTO.FoodPrice <= 0)
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

            return PartialView("_CrudFoodPartial", pvm); // pdto değeri döndürmemizin nedeni cdto.CategoryofFoodName nin html'de dolu olması diğer değerler boş gelecek...


            /*else
            {
                 

                CategoryofFoodDTO cdto = new CategoryofFoodDTO(); // yazılmazsa null referance hatası verir.. 
                                                      //cdto.CategoryofFoodName = CategoryofFoodNameAccordingToFood; // 2.yol

                cdto.CategoryofFoodName = TempData["CategoryofFoodName"].ToString();
                cdto.ID = (int)TempData["CategoryofFood_id"];

                TempData["CategoryofFood_id"] = cdto.ID;
                TempData["CategoryofFoodName"] = cdto.CategoryofFoodName; // 2.yol kullanılırsa gerekli olacak kod..

                FoodVM pvm = new FoodVM
                {
                    CategoryofFoodDTO = cdto,
                    FoodDTO =  pdto_reloaddata                

                };


                TempData["CategoryofFood_id"] = cdto.ID;
                TempData["CategoryofFoodName"] = cdto.CategoryofFoodName; // 2.yol kullanılırsa gerekli olacak kod..

                return PartialView("_CrudFoodPartial", pvm); // pdto değeri döndürmemizin nedeni cdto.CategoryofFoodName nin html'de dolu olması diğer değerler boş gelecek...

            }
            */
        }



        [Route("UpdateFoodAjax")]
        public async Task<PartialViewResult> UpdateFoodAjax(int id)
        {
            IEnumerable<string> CategoryofFoodNames = await _icm.GetActivesCategoryofFoodNamesAsync();
            ViewBag.CategoryofFoodNames = new SelectList(CategoryofFoodNames);

            ViewBag.Status = new SelectList(Enum.GetNames(typeof(ExistentStatus)));



            // Food Food_item_control = await _ipm.GetFoodByIdwithCategoryofFoodValueAsync(id);
            // yukarıdaki kod Ürünü, kategori bilgileri ile getirir buna gerek yok.. Food_item.Adapt<FoodDTO>() yeterli



            FoodDTO fdto = new FoodDTO();

            var result = new FoodDTO();

            if (TempData["HttpContext"] != null)
            {
                result = HttpContext.Session.GetObject<FoodDTO>("manipulatedData");
                fdto = result;

                // HttpContext.Session.SetObject("manipulatedData", null);
            }
            else
            {
                Food Food_item = await _ipm.GetByIdAsync(id);

                fdto = Food_item.Adapt<FoodDTO>();
            }




            /*
            string CategoryofFoodNameAccordingToFood = await _icm.GetCategoryofFoodNameAccordingToFoodAsync((int)TempData["CategoryofFood_id"]);      
            */

            CategoryofFoodDTO cdto = new CategoryofFoodDTO(); // yazılmazsa cdto.CategoryofFoodName null referance hatası verir.. 
            cdto.CategoryName_of_Foods = TempData["CategoryofFoodName"].ToString(); // asp-for="CategoryofFood.CategoryofFoodName" değer atamak için 
            // cdto.CategoryofFoodName = CategoryofFoodNameAccordingToFood;  2.yol
            // pDTO.CategoryofFood = cdto; // yazılmazsa null referance hatası verir.. 
            cdto.ID = (int)TempData["CategoryofFood_id"];

            string kontrol = TempData["CategoryofFood_status"].ToString();
            cdto.ExistentStatus = (ExistentStatus)TempData["CategoryofFood_status"];
             /*
             Food Food_item = await _ipm.GetByIdAsync(id); kod sayesinde pdto.CategoryofFoodID geldiği için 
             AddFoodAjax'taki gibi pdto.CategoryofFoodID = (int)TempData["CategoryofFood_id"]; koda gerek kalmadı...
            
             */


            FoodVM pvm = new FoodVM
            {
                FoodDTO = fdto,
                CategoryofFoodDTO = cdto
            };

            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(pvm.FoodDTO.FoodName))
                {
                    ModelState.AddModelError("FoodDTO.FoodName", "Ürün adı giriniz.");
                }
                if (fdto.FoodPrice <= 0)
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
             int CategoryofFood_id = (int)TempData["CategoryofFood_id"];
            TempData["CategoryofFood_id"] = CategoryofFood_id;
            */
            TempData["CategoryofFood_id"] = cdto.ID;
            TempData["CategoryofFoodName"] = cdto.CategoryName_of_Foods; // 2.yol kullanılırsa gerekli olacak kod..
            TempData["CategoryofFood_status"] = cdto.ExistentStatus;

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
                // ModelState.Remove("CategoryofFood");
                ModelState.Remove("FoodDTOs");
                ModelState.Remove("JavascriptToRun");
                ModelState.Remove("CategoryofFoodDTO");

            

                // CategoryofFoodDTO cdto = new CategoryofFoodDTO();// yazılmazsa null referance hatası verir.. 
                // // cdto.ID = (int) TempData["CategoryofFoodID"];
                // pdto.CategoryofFood = cdto;
                // pdto.CategoryofFood.ID = (int)TempData["CategoryofFoodID"];
                // pdto.CategoryofFood.CategoryofFoodName = TempData["CategoryofFoodName"].ToString();
                // pdto.CategoryofFood.Status = (Status)TempData["CategoryofFoodStatus"];
                // pdto.CategoryofFood.CategoryofFoodPicture = TempData["CategoryofFoodPicture"].ToString();



                if (ModelState.IsValid)
                {
                    Food fd = pvm_post.FoodDTO.Adapt<Food>();

                    //prd.Status = (int)pvm_post.FoodDTO.Status; // casting bu olmadan dene
                    
                    //  <input type="hidden" asp-for="CategoryofFoodID" /> bunu kullandığımız için prd.CategoryofFoodID = (int)TempData["CategoryofFood_id"]; ama bu koda gerek kalmadı... zira FoodDTO'da CategoryofFoodID ile veriyi aldık.. 
                    // prd.CategoryofFood = null;
                    // prd.CategoryofFoodID = pvm_post.CategoryofFoodDTO.ID; bu koda gerek kalmadı çünkü <input type="hidden" asp-for="FoodDTO.CategoryofFoodID" /> bunu kullandığımız için.. bunu da pdto.CategoryofFoodID = (int)TempData["CategoryofFood_id"]; bu kodla sağladık.. 

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

                            /* !!! !!! prd.FoodPicture = "/FoodPicture/" + fileName;   // veritabanına kayıt (wwwroot belirtmeye gerek yok)  !!! !!! */

                        }
                    }
                    else
                    {
                        // Update işleminde çalışır
                        if (pvm_post.FoodDTO.ID != 0)
                        {
                            Food prdv2 = await _ipm.GetByIdAsync(pvm_post.FoodDTO.ID);

                            /* !!! !!! if (prdv2.FoodPicture != null) // önceden veritabanında resim varsa ve resim seçilmedi ise..
                            {
                                prd.FoodPicture = prdv2.FoodPicture;
                            }
                             !!! !!! */
                        }

                    }

                    if (fd.ID == 0)
                    {
                        await _ipm.AddAsync(fd);
                        TempData["messageFood"] = "Ürün eklendi";
                    }
                    else
                    {
                        _ipm.Update(fd);
                        TempData["messageFood"] = "Ürün güncellendi";
                    }

                    /*
                    int CategoryofFood_id = (int)TempData["CategoryofFood_id"];
                    TempData["CategoryofFood_id"] = CategoryofFood_id;
                    */

                            // var deneme = cdto.ID;
                            // TempData["CategoryofFoodID"] = cdto.ID;                    
                            // TempData["CategoryofFoodName"] = cdto.CategoryofFoodName;
                            // TempData["Status"] = (Status) cdto.Status; 
                            // TempData["CategoryofFoodPicture"] = cdto.CategoryofFoodPicture;

                            return RedirectToAction("FoodList", new { id = (int)TempData["CategoryofFood_id"], status = (int)TempData["CategoryofFood_status"] }); // comment'te alınırsa TempData["mesaj"] = "Ürün adı ve statü giriniz.."; da çalışır..

                    /*
                        return RedirectToAction("FoodList", new { id = (int)TempData["CategoryofFood_id"], status = (int)TempData["CategoryofFood_status"], JSpopupPage = TempData["JSpopupPage"].ToString() });
                     
                     */


                }

            }
            else
            {
                // Food prd = pvm_post.FoodDTO.Adapt<Food>();

                // _ipm.Delete(prd);

                _ipm.Delete(await _ipm.GetByIdAsync(pvm_post.FoodDTO.ID));

                // CategoryofFood ctg = cdto.Adapt<CategoryofFood>();

                // _icm.Delete(ctg);

                TempData["messageFood"] = "Ürün silindi";

                TempData["Deleted"] = null;

                //  return RedirectToAction("FoodList");
                return RedirectToAction("FoodList", new { id = (int)TempData["CategoryofFood_id"], status = (int)TempData["CategoryofFood_status"] });
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
                return RedirectToAction("FoodList", new { id = (int)TempData["CategoryofFood_id"], status = (int)TempData["CategoryofFood_status"], JSpopupPage = TempData["JSpopupPage"].ToString() });

            }
            else // add // (pvm_post.FoodDTO.ID == 0) çevir...
            {
                // pvm.JavascriptToRun = $"ShowErrorPopup( {pvm_post.FoodDTO} )";

                // pvm.JavascriptToRun = $"ShowErrorInsertOperationPopup()";

                TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";
                return RedirectToAction("FoodList", new { id = (int)TempData["CategoryofFood_id"], status = (int)TempData["CategoryofFood_status"], JSpopupPage = TempData["JSpopupPage"].ToString() });
            }




        }




    }
}
