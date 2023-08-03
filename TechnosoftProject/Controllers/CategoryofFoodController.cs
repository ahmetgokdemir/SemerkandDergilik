using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Enums;
using Project.ENTITIES.Identity_Models;
using Project.ENTITIES.Models;
using Technosoft_Project.CommonTools;
using Technosoft_Project.ViewModels;
using Technosoft_Project.VMClasses;

namespace Technosoft_Project.Controllers
{
    public class CategoryofFoodController : BaseController
    {
        readonly ICategoryofFoodManager _icm;
        readonly IUserCategoryJunctionManager _iucjm;

        public CategoryofFoodController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ICategoryofFoodManager icm, IUserCategoryJunctionManager iucjm) : base(userManager, null, roleManager)
        {
            _icm = icm;
            _iucjm = iucjm;
        }

        //[Route("CategoryofFoodIndex")]
        //public IActionResult Index()
        //{
        //    return View();
        //}


        [Route("CategoryofFoodList")]
        public async Task<IActionResult> CategoryofFoodList(string? JSpopupPage)
        {

            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null; // pop-up sıfırlanır yoksa sayfayı reflesleyince geliyor
            }

            // IEnumerable<CategoryofFood> CategoryofFoodList = await _icm.GetActivesAsync();

            IEnumerable<object> UserCategoryJunctionList = await _iucjm.Get_ByUserID_Async(CurrentUser.Id); // IdentityUser'dan gelen Id (Guid tipli)


            // Adapt<IEnumerable<UserCategoryJunctionDTO>>().ToList()
            // IdentityUser --> Id
            // .Adapt<IEnumerable<CategoryofFoodDTO>>().ToList()
            // IEnumerable<object> Menu_Categories 
            CategoryofFoodVM cvm = new CategoryofFoodVM
            {
                UserCategoryJunctionDTOs = UserCategoryJunctionList.Adapt<IEnumerable<UserCategoryJunctionDTO>>().ToList(),
                // CategoryofFoodDTOs = CategoryofFoodList.Adapt<IEnumerable<CategoryofFoodDTO>>().ToList(),
                JavascriptToRun = JSpopupPage
            };

            return View(cvm);
        }

        [Route("AddCategoryofFoodAjax")]
        public PartialViewResult AddCategoryofFoodAjax()
        {
            // ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status))); => yerine                                                                 asp-items="Html.GetEnumSelectList<Technosoft_Project.Enums.Status>()" kullanıldı..

            var result = new CategoryofFoodDTO();
            CategoryofFoodDTO cDTO = new CategoryofFoodDTO();

            var result_2 = new UserCategoryJunctionDTO();
            UserCategoryJunctionDTO ucjDTO = new UserCategoryJunctionDTO();


            // UserCategoryJunctionDTO

            // HttpContext.Session.SetObject("manipulatedData", pvm_post.CategoryofFoodDTO);
            
            if (TempData["ValidError_Name"] != null)
            {
                if (TempData["ValidError_Status"] == null)
                {
                    result_2 = HttpContext.Session.GetObject<UserCategoryJunctionDTO>("manipulatedData_Status");
                    ucjDTO = result_2;
                }

                if (HttpContext.Session.GetObject<CategoryofFoodDTO>("manipulatedData_Name") != null)
                {
                    result = HttpContext.Session.GetObject<CategoryofFoodDTO>("manipulatedData_Name");
                    cDTO = result;                    

                }


                if (string.IsNullOrEmpty(cDTO.CategoryName_of_Foods))
                {                  
                    ModelState.AddModelError("CategoryofFoodDTO.CategoryName_of_Foods", "Kategori adı giriniz.");
                }
                else if (cDTO.CategoryName_of_Foods.Length >= 128)
                {
                    ModelState.AddModelError("CategoryofFoodDTO.CategoryName_of_Foods", "Kategori 128 karakterden fazla olamaz.");
                }

                TempData["ValidError_Name"] = null;


            } // aynı veri db'de varsa !!
            else if (TempData["ValidError_NameExist"] != null)
            {
                
                result = HttpContext.Session.GetObject<CategoryofFoodDTO>("manipulatedData_NameExist");
                cDTO = result;

                if (TempData["ValidError_Status"] == null)
                {
                    result_2 = HttpContext.Session.GetObject<UserCategoryJunctionDTO>("manipulatedData_Status");
                    ucjDTO = result_2;
                }

                ModelState.AddModelError("CategoryofFoodDTO.CategoryName_of_Foods", "Kategori listede mevcuttur.");
                TempData["ValidError_NameExist"] = null;
            }


            if (TempData["ValidError_Status"] != null)
            {
                if (HttpContext.Session.GetObject<CategoryofFoodDTO>("manipulatedData_Name") != null)
                {
                    result = HttpContext.Session.GetObject<CategoryofFoodDTO>("manipulatedData_Name");
                    cDTO = result;

                }

                ModelState.AddModelError("UserCategoryJunctionDTO.CategoryofFood_Status", "Kategori durumunu giriniz.");
                TempData["ValidError_Status"] = null;
            }

            CategoryofFoodVM cVM = new CategoryofFoodVM
            {
                UserCategoryJunctionDTO = ucjDTO,
                CategoryofFoodDTO = cDTO
            };

            HttpContext.Session.SetObject("manipulatedData_Name", null);
            HttpContext.Session.SetObject("manipulatedData_Status", null);
            HttpContext.Session.SetObject("manipulatedData_NameExist", null);



                //if (TempData["HttpContext2"] != null)
                //{
                //    TempData["HttpContext2"] = null;

                //    if (cVM.UserCategoryJunctionDTO.CategoryofFood_Status == 0)
                //    {
                //        ModelState.AddModelError("UserCategoryJunctionDTO.CategoryofFood_Status", "Kategori durumunu giriniz.");
                //    }

                //    HttpContext.Session.SetObject("manipulatedData_Status", null);

                //}
         

            Thread.Sleep(500); // pop-up sayfasını tekrar açmayı tetikleyince bazen gelmiyor o yüzden bu kod eklendi..

            return PartialView("_CrudCategoryofFood_Partial", cVM);
        }




        [Route("UpdateCategoryofFoodAjax")]
        public async Task<PartialViewResult> UpdateCategoryofFoodAjax(short categoryID, Guid userID)
        {
            // CategoryofFood CategoryofFood_item = await _icm.GetByIdAsync(id);
            // CategoryofFoodDTO cDTO = CategoryofFood_item.Adapt<CategoryofFoodDTO>();

            // ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status))); => yerine                                                                 asp-items="Html.GetEnumSelectList<Technosoft_Project.Enums.Status>()" kullanıldı..


            CategoryofFoodDTO cDTO = new CategoryofFoodDTO();
            List<UserCategoryJunctionDTO> ucjDTO = new List<UserCategoryJunctionDTO>();
            // UserCategoryJunctionDTO ucjDTO;
 
            var result = new CategoryofFoodDTO();

            if (TempData["HttpContext"] != null)
            {
                result = HttpContext.Session.GetObject<CategoryofFoodDTO>("manipulatedData");
                cDTO = result;

                // HttpContext.Session.SetObject("manipulatedData", null);
            }
            else
            {
                CategoryofFood CategoryofFood_item = await _icm.GetByIdAsync(categoryID);
                cDTO = CategoryofFood_item.Adapt<CategoryofFoodDTO>();

                IEnumerable<object> ucj = await _iucjm.Get_ByUserID_with_CategoryID_Async(userID,categoryID);
                ucjDTO = ucj.Adapt<IEnumerable<UserCategoryJunctionDTO>>().ToList();
                //UserCategoryJunctionDTOs = UserCategoryJunctionList.Adapt<IEnumerable<UserCategoryJunctionDTO>>().ToList(),
                //                 UserCategoryJunctionDTOs = UserCategoryJunctionList.Adapt<IEnumerable<UserCategoryJunctionDTO>>().ToList(),


                // GetByIdAsync (Repository) int --> short 

                HttpContext.Session.SetObject("willbedeletedUserCategoryJuncData", ucj.FirstOrDefault());
                HttpContext.Session.SetObject("willbedeletedCategoryofFoodData", CategoryofFood_item);


            }


            CategoryofFoodVM cVM = new CategoryofFoodVM
            {
                UserCategoryJunctionDTO = ucjDTO[0],
                CategoryofFoodDTO = cDTO
            };



            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(cVM.CategoryofFoodDTO.CategoryName_of_Foods))
                {
                    ModelState.AddModelError("CategoryofFoodDTO.CategoryofFoodName", "Kategori adı giriniz.");
                }

                HttpContext.Session.SetObject("manipulatedData", null);
            }

            Thread.Sleep(500);

            return PartialView("_CrudCategoryofFood_Partial", cVM);
        }


        [Route("DeleteCategoryofFoodAjax")]
        public async Task<PartialViewResult> DeleteCategoryofFoodAjax(int id)
        {
            CategoryofFood CategoryofFood_item = await _icm.GetByIdAsync((short)id);
            CategoryofFoodDTO cDTO = CategoryofFood_item.Adapt<CategoryofFoodDTO>();

            //ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));
            ViewBag.CategoryofFoodNameDelete = cDTO.CategoryName_of_Foods;

            ViewBag.CRUD = "delete_operation";

            CategoryofFoodVM cVM = new CategoryofFoodVM
            {
                CategoryofFoodDTO = cDTO
            };

            return PartialView("_CrudCategoryofFood_Partial", cVM);
        }



        [Route("CRUDCategoryofFood")]
        [HttpPost]
        public async Task<IActionResult> CRUDCategoryofFood(CategoryofFoodVM cvm_post, IFormFile _CategoryofFoodPicture)
        {


            /*
              var urlHelper = new UrlHelper(ControllerContext);
              var url = urlHelper.Action("About", "Home");
              var linkText = "Panelden yapılan değiliklik web e yansımıyor";
              
              var hyperlink = string.Format("<a href=\"{0}\">{1}</a>", url, linkText);
              
              var url2 = $"{Request.Scheme}://{Request.Host}/Home/About";
            */



            /* PasswordReset.cs'de SendGridClient --> Task Execute(string link, string emailAdress) kısmında yapılmış...*/


            if (TempData["Deleted"] == null)
            {
                
                UserCategoryJunction old_ucj = null;
                CategoryofFood old_cof = null;


                if (HttpContext.Session.GetObject<UserCategoryJunction>("willbedeletedUserCategoryJuncData") != null && HttpContext.Session.GetObject<CategoryofFood>("willbedeletedCategoryofFoodData") != null)
                {

                    old_ucj = new UserCategoryJunction();
                    old_ucj = HttpContext.Session.GetObject<UserCategoryJunction>("willbedeletedUserCategoryJuncData");

                    old_cof = new CategoryofFood();
                    old_cof = HttpContext.Session.GetObject<CategoryofFood>("willbedeletedCategoryofFoodData");

                    HttpContext.Session.SetObject("willbedeletedUserCategoryJuncData", null);
                    HttpContext.Session.SetObject("willbedeletedCategoryofFoodData", null);


                }



                //ModelState.Remove("CategoryofFoodPicture");
                //ModelState.Remove("CategoryofFoodDTOs");
                //ModelState.Remove("JavascriptToRun");
                ModelState.Remove("ExistentStatus");
                ModelState.Remove("_CategoryofFoodPicture"); // IFormFile _CategoryofFoodPicture İÇİN
                ModelState.Remove("CategoryofFoodDTO.ExistentStatus");
                ModelState.Remove("UserCategoryJunctionDTO.CategoryName_of_Foods");

                if (ModelState.IsValid)
                {
                    CategoryofFood ctg_add = null;
                    CategoryofFood ctg_update = null;
                    short old_categoryID = 0;

                    if (cvm_post.CategoryofFoodDTO.ID == 0)
                    {
                        ctg_add = cvm_post.CategoryofFoodDTO.Adapt<CategoryofFood>();

                    }
                    else
                    {
                        old_categoryID = cvm_post.CategoryofFoodDTO.ID;
                        ctg_update = new CategoryofFood();
                        ctg_update.CategoryName_of_Foods = cvm_post.CategoryofFoodDTO.CategoryName_of_Foods;
                    }

                    UserCategoryJunction ucj = cvm_post.UserCategoryJunctionDTO.Adapt<UserCategoryJunction>();

                    /* !!! !!! ctg.Status = (int)cvm_post.CategoryofFoodDTO.Status; !!! !!!*/

                    if (_CategoryofFoodPicture != null && _CategoryofFoodPicture.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(_CategoryofFoodPicture.FileName); // path oluşturma

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CategoryofFoodPicture", fileName); // server'a kayıt edilecek path => wwwroot/UserPicture/fileName

                        // kayıt işlemi
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await _CategoryofFoodPicture.CopyToAsync(stream); // userPicture'ı, stream'e kayıt

                            /* !!! !!! ctg.CategoryofFoodPicture = "/CategoryofFoodPicture/" + fileName;   // veritabanına kayıt (wwwroot belirtmeye gerek yok) !!! !!! */

                            ucj.CategoryofFood_Picture = "/CategoryofFoodPicture/" + fileName;

                        }
                    }
                    else // resim yüklenmedi ise...
                    {
                        // mevcut da resmi db ' de varsa ... mevcut resmi db den çekip tekrar set etmeye gerek yok gibi ?! --> _icm.Update(ctg); kısmında bir kontrol et !!!

                        /* if (ucj.DataStatus == DataStatus.Updated)
                        {
                            UserCategoryJunction ucj_controller = await _iucjm.GetByIdAsync(cvm_post.CategoryofFoodDTO.ID);

                            if (ucj_controller != null)
                            {

                                if (ucj_controller.CategoryofFood_Picture != null)
                                {
                                    ucj.CategoryofFood_Picture = ucj_controller.CategoryofFood_Picture;
                                }

                            }
                        }*/
                    }

                    if (cvm_post.CategoryofFoodDTO.ID == 0) // insert
                    {
                        // Aynı isimli kayıt db'de zaten varsa 
                        if (await _icm.Any(x => x.CategoryName_of_Foods == ctg_add.CategoryName_of_Foods))
                        {
                            HttpContext.Session.SetObject("manipulatedData_NameExist", cvm_post.CategoryofFoodDTO);

                            TempData["ValidError_NameExist"] = "valid";
                            TempData["JavascriptToRun"] = "valid";

                            if (cvm_post.UserCategoryJunctionDTO.CategoryofFood_Status != 0)
                            {
                                HttpContext.Session.SetObject("manipulatedData_Status", cvm_post.UserCategoryJunctionDTO);                                
                            }
                            else
                            {
                                TempData["ValidError_Status"] = "valid";
                            }

                            TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";
                            return RedirectToAction("CategoryofFoodList", new { JSpopupPage = TempData["JSpopupPage"].ToString() });


                        }

                        await _icm.AddAsync(ctg_add);

                        ucj.AccessibleID = CurrentUser.AccessibleID;
                        ucj.CategoryofFoodID = ctg_add.ID;

                        // short control = CurrentUser.ID; olmadı // AppUser'dan gelen Id olmuyor (short tipli), ignore edilmişti zaten
                        // Guid control2 = CurrentUser.Id;
                        ucj.AppUser = CurrentUser; // IdentityUser'ın Id'si de tabloya eklnemiş oldu..
                        //ucj.AppUser.Id = CurrentUser.Id;

                        await _iucjm.AddAsync(ucj);
                        TempData["messageCategoryofFood"] = "Kategori eklendi";
                    }
                    else // update 
                    {
                        // Yeni bir kategori
                        if (old_cof.CategoryName_of_Foods != ctg_update.CategoryName_of_Foods)
                        {

                            // Yeni kategori eğer havuzda zaten varsa 
                            if (await _icm.Any(x => x.CategoryName_of_Foods == ctg_update.CategoryName_of_Foods))
                            {
                                HttpContext.Session.SetObject("manipulatedData_NameExist", cvm_post.CategoryofFoodDTO);

                                TempData["ValidError_NameExist"] = "valid";
                                TempData["JavascriptToRun"] = "valid";

                                HttpContext.Session.SetObject("manipulatedData_Status", cvm_post.UserCategoryJunctionDTO);


                                TempData["JSpopupPage"] = $"ShowErrorUpdateOperationPopup({cvm_post.CategoryofFoodDTO.ID})"; // diğer paramaetre de eklenecek

                                return RedirectToAction("CategoryofFoodList", new { JSpopupPage = TempData["JSpopupPage"].ToString() });
                            }

                            // Yeni kategori eğer havuzda da yoksa 
                            else
                            {
                                // yeni bir kategori olarak eklenecek ... güncellenmeyecek
                                await _icm.AddAsync(ctg_update);
                                // _icm.Update(ctg);

                                // usercategory olarak da eklenecek...
                                ucj.AccessibleID = CurrentUser.AccessibleID;
                                // ucj.AppUser.Id = CurrentUser.Id; erişilemiyor
                                ucj.CategoryofFoodID = ctg_update.ID;
                                ucj.AppUser = CurrentUser; // IdentityUser'ın Id'si de tabloya eklnemiş oldu..

                                await _iucjm.AddAsync(ucj);

                                // builder.HasKey(x => new { x.AccessibleID, x.CategoryofFoodID }); sayesinde 
                                // _iucjm.Update_OldCategory_with_NewOne(CurrentUser.AccessibleID, old_categoryID, ucj); // await yok // IdentityUser'dan gelen Id (Guid tipli)

                                // eski usercategory pasife alınacak...
                                // _iucjm.Delete(old_ucj);

                                old_ucj.DataStatus = DataStatus.Deleted;
                                old_ucj.ModifiedDate = DateTime.Now;
                                old_ucj.AccessibleID = CurrentUser.AccessibleID;
                                old_ucj.AppUser = CurrentUser;
                                old_ucj.CategoryofFood_Status = ExistentStatus.Pasif;
                                _iucjm.Delete_OldCategory_from_User(CurrentUser.AccessibleID, old_categoryID, old_ucj);

                                TempData["messageCategoryofFood"] = "Kategori güncellendi";                                

                            }

                            return RedirectToAction("CategoryofFoodList");
                        }

                        else // eski kategori
                        {
                            // ucj tablosunda değişiklik var
                            if (old_ucj.CategoryofFood_Status != cvm_post.UserCategoryJunctionDTO.CategoryofFood_Status) {

                                // _iucjm.Update(ucj);
                                // old_categoryID = cvm_post.CategoryofFoodDTO.ID;

                                ucj.DataStatus = DataStatus.Updated;
                                ucj.ModifiedDate = DateTime.Now;
                                ucj.AccessibleID = CurrentUser.AccessibleID;
                                ucj.AppUser = CurrentUser;

                                ucj.CategoryofFood = cvm_post.CategoryofFoodDTO.Adapt<CategoryofFood>();
                                ucj.CategoryofFoodID = cvm_post.CategoryofFoodDTO.ID;

                                _iucjm.Update_UserCategoryJuncTable(CurrentUser.AccessibleID, cvm_post.CategoryofFoodDTO.ID, ucj);
                                TempData["messageCategoryofFood"] = "Kategori güncellendi";

                            }

                            return RedirectToAction("CategoryofFoodList");

                        }



                    }

                  
                }
                // else --> validation olmayan kod kısmını buraya al

            }
            else
            {
                _icm.Delete(await _icm.GetByIdAsync((short)cvm_post.CategoryofFoodDTO.ID));

                // CategoryofFood ctg = cdto.Adapt<CategoryofFood>();

                // _icm.Delete(ctg);
                TempData["messageCategoryofFood"] = "Kategori silindi";

                TempData["Deleted"] = null;

                return RedirectToAction("CategoryofFoodList");
            }

            // TempData["mesaj"] = "Kategori adı ve statü giriniz..";
            // ModelState.AddModelError("", "Ürün adı ve statü giriniz..");

            if (cvm_post.UserCategoryJunctionDTO.CategoryofFood_Status == 0)
            {
                TempData["ValidError_Status"] = "valid";

                if (!String.IsNullOrEmpty(cvm_post.CategoryofFoodDTO.CategoryName_of_Foods))
                {
                    HttpContext.Session.SetObject("manipulatedData_Name", cvm_post.CategoryofFoodDTO);
                }
                
            }

            if (String.IsNullOrEmpty(cvm_post.CategoryofFoodDTO.CategoryName_of_Foods) /* || cvm_post.CategoryofFoodDTO.CategoryName_of_Foods.Lengt >= 128 */)
            {               
                TempData["ValidError_Name"] = "valid";

                if (cvm_post.UserCategoryJunctionDTO.CategoryofFood_Status != 0)
                {
                    HttpContext.Session.SetObject("manipulatedData_Status", cvm_post.UserCategoryJunctionDTO);
                }
            }

            CategoryofFoodVM cVM = new CategoryofFoodVM();

            TempData["JavascriptToRun"] = "valid";


            if (cvm_post.CategoryofFoodDTO.ID != 0) //update
            {
                cVM.JavascriptToRun = $"ShowErrorUpdateOperationPopup({cvm_post.CategoryofFoodDTO.ID})";
                return RedirectToAction("CategoryofFoodList", new { JSpopupPage = cVM.JavascriptToRun });

            }
            else // add // (pvm_post.FoodDTO.ID == 0) 
            {
                // pvm.JavascriptToRun = $"ShowErrorPopup( {pvm_post.FoodDTO} )";

                // pvm.JavascriptToRun = $"ShowErrorInsertOperationPopup()";

                TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";
                return RedirectToAction("CategoryofFoodList", new { JSpopupPage = TempData["JSpopupPage"].ToString() });
            }


        }
    }
}
