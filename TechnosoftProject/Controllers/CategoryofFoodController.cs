using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Enums;
using Project.ENTITIES.Identity_Models;
using Project.ENTITIES.Models;
using Technosoft_Project.CommonTools;
using Technosoft_Project.ViewModels;
using Technosoft_Project.VMClasses;

namespace Technosoft_Project.Controllers
{
    [Authorize]
    [Authorize(Policy = "Confirmed_Member_Policy")]
    public class CategoryofFoodController : BaseController
    {
        readonly ICategoryofFoodManager _icm;
        readonly IUserCategoryJunctionManager _iucjm;

        public CategoryofFoodController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ICategoryofFoodManager icm, IUserCategoryJunctionManager iucjm) : base(userManager, null, roleManager)
        {
            _icm = icm;
            _iucjm = iucjm;
        }


        [Route("CategoryofFoodList_forMember")]
        public async Task<IActionResult> CategoryofFoodList_forMember(string? JSpopupPage, string? onlyOnce)
        {
            TempData["onlyOnce"] = onlyOnce;

            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null; // pop-up sıfırlanır yoksa sayfayı reflesleyince geliyor
            }

            string msj;
            if (TempData["messageCategoryofFood_InOtherUsersList"] != null)
            {
                msj = TempData["messageCategoryofFood_InOtherUsersList"].ToString();
                TempData["messageCategoryofFood_InOtherUsersList"] = msj;
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

            return View("CategoryofFoodListforMember", cvm);
        }

        [Route("AddCategoryofFoodAjax")] // insert
        public async Task<PartialViewResult> AddCategoryofFoodAjax()
        {
            // ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status))); => yerine                                                                 asp-items="Html.GetEnumSelectList<Technosoft_Project.Enums.Status>()" kullanıldı..

            CategoryofFoodDTO cDTO = new CategoryofFoodDTO();
            UserCategoryJunctionDTO ucjDTO = new UserCategoryJunctionDTO();



            if (TempData["ValidError_NameExist"] != null)
            {

                //result = HttpContext.Session.GetObject<CategoryofFoodDTO>("manipulatedData_NameExist");
                //cDTO = result;

                ucjDTO = HttpContext.Session.GetObject<UserCategoryJunctionDTO>("manipulatedData_ucj");
                // ucjDTO = result_2;


                ModelState.AddModelError("CategoryofFoodDTO.CategoryName_of_Foods", $"{TempData["existinPool"]}" + " " + " Kategori listenizde mevcut ya da Havuz listesinde bulunmaktadır. Listenizde bulunmamaktaysa havuz listesinden kendi listenize de ekleyebilirsiniz.");

                TempData["ValidError_NameExist"] = null;
                TempData["existinPool"] = null;

                HttpContext.Session.SetObject("manipulatedData_ucj", null);


            }
            else if (TempData["ValidError_General"] != null) // 2.validasyon kontrolü 
            {

                if (HttpContext.Session.GetObject<CategoryofFoodDTO>("manipulatedData_ctg") != null)
                {
                    cDTO = HttpContext.Session.GetObject<CategoryofFoodDTO>("manipulatedData_ctg");
                }

                if (HttpContext.Session.GetObject<UserFoodJunctionDTO>("manipulatedData_ucj") != null)
                {
                    ucjDTO = HttpContext.Session.GetObject<UserCategoryJunctionDTO>("manipulatedData_ucj");
                }


                if (TempData["ValidError_Name"] != null)
                {

                    if (string.IsNullOrEmpty(cDTO.CategoryName_of_Foods))
                    {
                        ModelState.AddModelError("CategoryofFoodDTO.CategoryName_of_Foods", "Kategori adı giriniz.");
                    }
                    else if (cDTO.CategoryName_of_Foods.Length >= 30)
                    {
                        ModelState.AddModelError("CategoryofFoodDTO.CategoryName_of_Foods", "Kategori 30 karakterden fazla olamaz.");
                    }

                    TempData["ValidError_Name"] = null;

                }


                if (TempData["ValidError_Status"] != null)
                {

                    ModelState.AddModelError("UserCategoryJunctionDTO.CategoryofFood_Status", "Kategori durumunu giriniz.");

                    TempData["ValidError_Status"] = null;
                }

                TempData["ValidError_General"] = null;

                HttpContext.Session.SetObject("manipulatedData_ctg", null);
                HttpContext.Session.SetObject("manipulatedData_ucj", null);

            }
            

            CategoryofFoodVM cVM = new CategoryofFoodVM
            {
                UserCategoryJunctionDTO = ucjDTO,
                CategoryofFoodDTO = cDTO
            };


            Thread.Sleep(500); // pop-up sayfasını tekrar açmayı tetikleyince bazen gelmiyor o yüzden bu kod eklendi..

            return PartialView("_CrudCategoryofFood_Partial", cVM);
        }




        [Route("UpdateCategoryofFoodAjax")]
        public async Task<PartialViewResult> UpdateCategoryofFoodAjax(short categoryID/*, Guid userID*/)
        {

            Guid new_userID = CurrentUser.Id;


            CategoryofFoodDTO cDTO = new CategoryofFoodDTO();
            List<UserCategoryJunctionDTO> ucjDTO = new List<UserCategoryJunctionDTO>();
            UserCategoryJunctionDTO result_ucj = new UserCategoryJunctionDTO();

            IEnumerable<object> ucj = null;
            // CategoryofFoodDTO result_ctg = new CategoryofFoodDTO();



            if (TempData["ValidError_NameExist"] != null) // 1.validasyon kontrolü 
            {
                // kullacının girdiği verileri tekrar girmemesi için
                //if (HttpContext.Session.GetObject<CategoryofFood>("manipulatedData_ctg") != null)
                //{
                //    result_ctg = HttpContext.Session.GetObject<CategoryofFoodDTO>("manipulatedData_ctg");
                //    cDTO = result_ctg;
                //}

                //if (HttpContext.Session.GetObject<UserCategoryJunctionDTO>("manipulatedData_ufdj") != null)
                //{
                result_ucj = HttpContext.Session.GetObject<UserCategoryJunctionDTO>("manipulatedData_ucj");
                    ucjDTO.Add(result_ucj);
                //}


                cDTO = HttpContext.Session.GetObject<CategoryofFoodDTO>("manipulatedData_ctg");


                //result_ufdj = HttpContext.Session.GetObject<UserCategoryJunctionDTO>("manipulatedData_ucj");
                //ucjDTO.Add(result_ufdj);
                // ucjDTO[0] = result_2;

                ModelState.AddModelError("CategoryofFoodDTO.CategoryName_of_Foods", $"{TempData["existinPool"]}" + " " + "kategorisi havuzda mevcuttur. Havuz listesinden kendi listenize de ekleyebilirsiniz.");

                TempData["ValidError_NameExist"] = null;
                TempData["existinPool"] = null;

                // TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()"; $"{TempData["existinPool"}" 

                HttpContext.Session.SetObject("manipulatedData_ctg", null);
                HttpContext.Session.SetObject("manipulatedData_ucj", null);



            }
            else if (TempData["ValidError_General"] != null)  // 2.validasyon kontrolü 
            {
                // kullacının girdiği verileri tekrar girmemesi için
                if (HttpContext.Session.GetObject<CategoryofFood>("manipulatedData_ctg") != null)
                {
                    cDTO = HttpContext.Session.GetObject<CategoryofFoodDTO>("manipulatedData_ctg");
                    // cDTO = result_ctg;
                }

                if (HttpContext.Session.GetObject<UserCategoryJunctionDTO>("manipulatedData_ucj") != null)
                {
                    result_ucj = HttpContext.Session.GetObject<UserCategoryJunctionDTO>("manipulatedData_ucj");
                    ucjDTO.Add(result_ucj);
                }


 

                if (TempData["ValidError_Name"] != null)
                {

                    if (TempData["NullOrEmpty"] != null)
                    {
                        ModelState.AddModelError("CategoryofFoodDTO.CategoryName_of_Foods", "Kategori ismi giriniz.");
                    }
                    /*else if (cDTO.CategoryName_of_Foods.Length >= 30)
                    {
                        ModelState.AddModelError("CategoryofFoodDTO.CategoryName_of_Foods", "Kategori ismi 30 karakterden fazla olamaz.");
                    }*/
                    else if (TempData["existinpool"] != null)
                    {
                        ModelState.AddModelError("CategoryofFoodDTO.CategoryName_of_Foods", $"{TempData["existinPool"]}" + " " + " kategori listenizde mevcut ya da Havuz listesinde bulunmaktadır. Listenizde bulunmamaktaysa havuz listesinden kendi listenize de ekleyebilirsiniz.");
                    }

                    TempData["ValidError_Name"] = null;
                }

                if (TempData["ValidError_Status"] != null)
                {
                    ModelState.AddModelError("UserCategoryJunctionDTO.CategoryofFood_Status", "Yemek durumunu giriniz.");

                    TempData["ValidError_Status"] = null;
                }// "UserCategoryJunctionDTO.CategoryofFood_Status", "Kategori durumunu giriniz."

                //result_ucj = HttpContext.Session.GetObject<UserCategoryJunctionDTO>("manipulatedData_Status");
                //ucjDTO.Add(result_ucj);


                //cDTO = HttpContext.Session.GetObject<CategoryofFoodDTO>("manipulatedData_Name");

                TempData["ValidError_General"] = null;


                HttpContext.Session.SetObject("manipulatedData_ctg", null);
                HttpContext.Session.SetObject("manipulatedData_ucj", null);

                //HttpContext.Session.SetObject("willbedeletedUserCategoryJuncData", ucjDTO[0]);
                //HttpContext.Session.SetObject("willbedeletedCategoryofFoodData", cDTO);

            }
            else // ilk güncelleme denemesi ise
            {
                //result_ctg = await _icm.GetByIdAsync(categoryID);
                cDTO = (await _icm.GetByIdAsync(categoryID)).Adapt<CategoryofFoodDTO>();

                // cDTO = result_ctg.Adapt<CategoryofFoodDTO>();

                ucj = await _iucjm.Get_ByUserID_with_CategoryID_Async(new_userID, categoryID);
                ucjDTO = ucj.Adapt<IEnumerable<UserCategoryJunctionDTO>>().ToList();
                //UserCategoryJunctionDTOs = UserCategoryJunctionList.Adapt<IEnumerable<UserCategoryJunctionDTO>>().ToList(),
                //                 UserCategoryJunctionDTOs = UserCategoryJunctionList.Adapt<IEnumerable<UserCategoryJunctionDTO>>().ToList(),


                // GetByIdAsync (Repository) int --> short 

                //HttpContext.Session.SetObject("willbedeletedUserCategoryJuncData", ucjDTO[0]);
                //HttpContext.Session.SetObject("willbedeletedCategoryofFoodData", cDTO);


            }


            HttpContext.Session.SetObject("will_be_deleted_UserCategoryJuncData", ucjDTO[0]);
            HttpContext.Session.SetObject("will_be_deleted_CategoryofFoodData", cDTO);

            CategoryofFoodVM cVM = new CategoryofFoodVM
            {
                UserCategoryJunctionDTO = ucjDTO[0],
                CategoryofFoodDTO = cDTO
            };

            Thread.Sleep(500);

            return PartialView("_CrudCategoryofFood_Partial", cVM);
        }


        [Route("DeleteCategoryofFoodAjax")]
        public async Task<PartialViewResult> DeleteCategoryofFoodAjax(short categoryID)
        {

            CategoryofFood CategoryofFood_item = await _icm.GetByIdAsync(categoryID);
            CategoryofFoodDTO cDTO = CategoryofFood_item.Adapt<CategoryofFoodDTO>();


            //IEnumerable<object> ucj = null;
            //Guid new_userID = CurrentUser.Id;
            //List<UserCategoryJunctionDTO> ucjDTO = new List<UserCategoryJunctionDTO>();


            //ucj = await _iucjm.Get_ByUserID_with_CategoryID_Async(new_userID, categoryID);
            //ucjDTO = ucj.Adapt<IEnumerable<UserCategoryJunctionDTO>>().ToList();


            // HttpContext.Session.SetObject("willbedeletedUserCategoryJuncData", ucjDTO[0]);
            // HttpContext.Session.SetObject("willbedeletedCategoryofFoodData", cDTO);


            ViewBag.CategoryofFoodNameDelete = cDTO.CategoryName_of_Foods;
            ViewBag.CRUD = "delete_operation";

            CategoryofFoodVM cVM = new CategoryofFoodVM
            {
                //UserCategoryJunctionDTO = ucjDTO[0],
                CategoryofFoodDTO = cDTO
            };

            return PartialView("_CrudCategoryofFood_Partial", cVM);
        }



        [Route("CRUDCategoryofFood")]
        [HttpPost]
        public async Task<IActionResult> CRUDCategoryofFood(CategoryofFoodVM cvm_post, IFormFile _CategoryofFoodPicture)
        {
            // CategoryofFoodList
            // RedirectToAction("CategoryofFoodList_forM

            /*
              var urlHelper = new UrlHelper(ControllerContext);
              var url = urlHelper.Action("About", "Home");
              var linkText = "Panelden yapılan değiliklik web e yansımıyor";
              
              var hyperlink = string.Format("<a href=\"{0}\">{1}</a>", url, linkText);
              
              var url2 = $"{Request.Scheme}://{Request.Host}/Home/About";
            */



            /* PasswordReset.cs'de SendGridClient --> Task Execute(string link, string emailAdress) kısmında yapılmış...*/


            UserCategoryJunctionDTO old_ucj = null;
            CategoryofFoodDTO old_cof = null;


            if (HttpContext.Session.GetObject<UserCategoryJunctionDTO>("will_be_deleted_UserCategoryJuncData") != null && HttpContext.Session.GetObject<CategoryofFoodDTO>("will_be_deleted_CategoryofFoodData") != null)
            {

                old_ucj = new UserCategoryJunctionDTO();
                old_ucj = HttpContext.Session.GetObject<UserCategoryJunctionDTO>("will_be_deleted_UserCategoryJuncData");

                old_cof = new CategoryofFoodDTO();
                old_cof = HttpContext.Session.GetObject<CategoryofFoodDTO>("will_be_deleted_CategoryofFoodData");

                HttpContext.Session.SetObject("will_be_deleted_UserCategoryJuncData", null);
                HttpContext.Session.SetObject("will_be_deleted_CategoryofFoodData", null);

            }

            if (TempData["Deleted"] == null)
            {

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

                        /*if (ucj.DataStatus == DataStatus.Updated || .Inserted)
                        {*/
                        UserCategoryJunctionDTO ucj_controller_2 = null;
                        IEnumerable<object> ucj_controller = await _iucjm.Get_ByUserID_with_CategoryID_Async(CurrentUser.Id, cvm_post.CategoryofFoodDTO.ID);//await _iucjm.GetByIdAsync(cvm_post.CategoryofFoodDTO.ID);

                        ucj_controller_2 = ucj_controller.FirstOrDefault().Adapt<UserCategoryJunctionDTO>();

                        if (ucj_controller_2 != null)
                        {

                            if (ucj_controller_2.CategoryofFood_Picture != null)
                            {
                                ucj.CategoryofFood_Picture = ucj_controller_2.CategoryofFood_Picture;
                            }

                        }

                        /*}*/
                    }

                    if (cvm_post.CategoryofFoodDTO.ID == 0) // insert
                    {
                        // Aynı isimli kayıt db'de zaten varsa 
                        if (await _icm.Any(x => x.CategoryName_of_Foods == ctg_add.CategoryName_of_Foods))
                        {
                            //HttpContext.Session.SetObject("manipulatedData_NameExist", cvm_post.CategoryofFoodDTO);

                            TempData["ValidError_NameExist"] = "valid";

                                HttpContext.Session.SetObject("manipulatedData_ucj", cvm_post.UserCategoryJunctionDTO);

                            TempData["existinPool"] = cvm_post.CategoryofFoodDTO.CategoryName_of_Foods;

                            TempData["JavascriptToRun"] = "valid";
                            TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";


                            return RedirectToAction("CategoryofFoodList_forMember", new { JSpopupPage = TempData["JSpopupPage"].ToString(), onlyOnce = "1" });


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

                        return RedirectToAction("CategoryofFoodList_forMember", new { onlyOnce = "1" });

                    }
                    else // update 
                    {
                        // Yeni bir kategori
                        if (old_cof.CategoryName_of_Foods != ctg_update.CategoryName_of_Foods)
                        {

                            // Yeni kategori eğer havuzda zaten varsa 
                            if (await _icm.Any(x => x.CategoryName_of_Foods == ctg_update.CategoryName_of_Foods))
                            {

                                TempData["ValidError_NameExist"] = "valid";
                                TempData["JavascriptToRun"] = "valid";

                                //old_ucj.CategoryofFood_Status = cvm_post.UserCategoryJunctionDTO.CategoryofFood_Status;

                                TempData["existinPool"] = cvm_post.CategoryofFoodDTO.CategoryName_of_Foods;
                                // eski veri tekrardan set edildi...
                                HttpContext.Session.SetObject("manipulatedData_ctg", old_cof);
                                HttpContext.Session.SetObject("manipulatedData_ucj", cvm_post.UserCategoryJunctionDTO);


                                // ,{CurrentUser.Id}
                                TempData["JSpopupPage"] = $"ShowErrorUpdateOperationPopup({cvm_post.CategoryofFoodDTO.ID})"; // diğer paramaetre de eklenecek

                                return RedirectToAction("CategoryofFoodList_forMember", new { JSpopupPage = TempData["JSpopupPage"].ToString(), onlyOnce = "1" });
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



                                UserCategoryJunction userCategoryJunction = old_ucj.Adapt<UserCategoryJunction>();

                                userCategoryJunction.DataStatus = DataStatus.Deleted;
                                userCategoryJunction.DeletedDate = DateTime.Now;
                                userCategoryJunction.AccessibleID = CurrentUser.AccessibleID;
                                userCategoryJunction.AppUser = CurrentUser;
                                userCategoryJunction.CategoryofFood_Status = ExistentStatus.Pasif;

                                _iucjm.Delete_OldCategory_from_User(CurrentUser.AccessibleID, userCategoryJunction);

                                TempData["messageCategoryofFood"] = "Kategori güncellendi";

                                return RedirectToAction("CategoryofFoodList_forMember", new { onlyOnce = "1" });

                            }


                        }

                        else // eski kategori old_cof.CategoryName_of_Foods == ctg_update.CategoryName_of_Foods
                        {
                            //old_ucj.AppUser.Id = CurrentUser.Id;

                            //if (await _icm.Any(x => x.CategoryName_of_Foods == ctg_update.CategoryName_of_Foods) && await _iucjm.Any(x => x. == ctg_update.CategoryName_of_Foods))
                            //{
                            //    HttpContext.Session.SetObject("manipulatedData_NameExist", cvm_post.CategoryofFoodDTO);

                            //    TempData["ValidError_NameExist"] = "valid";
                            //    TempData["JavascriptToRun"] = "valid";

                            //    HttpContext.Session.SetObject("manipulatedData_Status", cvm_post.UserCategoryJunctionDTO);

                            //    // ,{CurrentUser.Id}
                            //    TempData["JSpopupPage"] = $"ShowErrorUpdateOperationPopup({cvm_post.CategoryofFoodDTO.ID})"; // diğer paramaetre de eklenecek

                            //    return RedirectToAction("CategoryofFoodList", new { JSpopupPage = TempData["JSpopupPage"].ToString() });
                            //}


                            // ucj tablosunda değişiklik var
                            // if (old_ucj.CategoryofFood_Status != cvm_post.UserCategoryJunctionDTO.CategoryofFood_Status) {

                            // _iucjm.Update(ucj);
                            // old_categoryID = cvm_post.CategoryofFoodDTO.ID;

                            ucj.DataStatus = DataStatus.Updated;
                            ucj.ModifiedDate = DateTime.Now;
                            ucj.AccessibleID = CurrentUser.AccessibleID;
                            ucj.AppUser = CurrentUser;

                            ucj.CategoryofFood = cvm_post.CategoryofFoodDTO.Adapt<CategoryofFood>();
                            ucj.CategoryofFoodID = cvm_post.CategoryofFoodDTO.ID;
                            //  ucj.CategoryofFood_Picture = "/CategoryofFoodPicture/" + fileName;





                            _iucjm.Update_UserCategoryJuncTable(CurrentUser.AccessibleID, cvm_post.CategoryofFoodDTO.ID, ucj);
                            TempData["messageCategoryofFood"] = "Kategori güncellendi";

                            // else { değişiklik olmadı mesajı }

                            return RedirectToAction("CategoryofFoodList_forMember", new { onlyOnce = "1" });

                        }



                    }


                }
                // else --> validation olmayan kod kısmı
                else
                {
                    // TempData["mesaj"] = "Kategori adı ve statü giriniz..";
                    // ModelState.AddModelError("", "Ürün adı ve statü giriniz..");

                    if (cvm_post.CategoryofFoodDTO.ID == 0)
                    {
                        if (cvm_post.UserCategoryJunctionDTO.CategoryofFood_Status == 0)
                        {
                            TempData["ValidError_Status"] = "valid";
                        }

                        if (String.IsNullOrEmpty(cvm_post.CategoryofFoodDTO.CategoryName_of_Foods) || cvm_post.CategoryofFoodDTO.CategoryName_of_Foods.Length >= 30 )
                        {
                            TempData["ValidError_Name"] = "valid";
                        }


                        TempData["ValidError_General"] = "valid";

                        if (cvm_post.CategoryofFoodDTO != null)
                        {
                            HttpContext.Session.SetObject("manipulatedData_ctg", cvm_post.CategoryofFoodDTO);
                        }

                        if (cvm_post.UserCategoryJunctionDTO != null)
                        {
                            HttpContext.Session.SetObject("manipulatedData_ucj", cvm_post.UserCategoryJunctionDTO);

                        }


                    }
                    else// update için olan validation error
                    {
                        if (cvm_post.UserCategoryJunctionDTO.CategoryofFood_Status == 0)
                        {
                            TempData["ValidError_Status"] = "valid";

                            //if (!String.IsNullOrEmpty(cvm_post.CategoryofFoodDTO.CategoryName_of_Foods) || cvm_post.CategoryofFoodDTO.CategoryName_of_Foods.Length < 30)
                            //{
 
                            //    if (await _icm.Any(x => x.CategoryName_of_Foods == cvm_post.CategoryofFoodDTO.CategoryName_of_Foods))
                            //    {
                            //        TempData["existinPool"] = $"{cvm_post.CategoryofFoodDTO.CategoryName_of_Foods}" + ", Kategori listenizde mevcut ya da Havuz listesinde bulunmaktadır. Listenizde bulunmamaktaysa havuz listesinden kendi listenize de ekleyebilirsiniz.";

                            //        HttpContext.Session.SetObject("manipulatedData_ctg", old_cof);

                            //    }
                            //    else
                            //    {
                            //        HttpContext.Session.SetObject("manipulatedData_ctg", cvm_post.CategoryofFoodDTO);
                            //    }

                                

                            //}


                            HttpContext.Session.SetObject("manipulatedData_ucj", old_ucj);                          

                        }

                        if (String.IsNullOrEmpty(cvm_post.CategoryofFoodDTO.CategoryName_of_Foods) /* || cvm_post.CategoryofFoodDTO.CategoryName_of_Foods.Length >= 30 */)
                        {
                            TempData["ValidError_Name"] = "valid";
                            TempData["NullOrEmpty"] = "valid";
                            //if (cvm_post.UserCategoryJunctionDTO.CategoryofFood_Status != 0)
                            //{
                            //    old_ucj.CategoryofFood_Status = cvm_post.UserCategoryJunctionDTO.CategoryofFood_Status;

                            //    HttpContext.Session.SetObject("manipulatedData_ucj", old_ucj);
                            //} 





                            //if (cvm_post.CategoryofFoodDTO != null)
                            //{
                            //    HttpContext.Session.SetObject("manipulatedData_ctg", old_cof);
                            //}

                            //if (cvm_post.UserCategoryJunctionDTO != null)
                            //{
                            //    HttpContext.Session.SetObject("manipulatedData_ucj", old_ucj);

                            //}

                                HttpContext.Session.SetObject("manipulatedData_ctg", old_cof);
                            


                        }
                        else
                        {
                            if (await _icm.Any(x => x.CategoryName_of_Foods == cvm_post.CategoryofFoodDTO.CategoryName_of_Foods) )
                            {
                                if(cvm_post.CategoryofFoodDTO.CategoryName_of_Foods != old_cof.CategoryName_of_Foods) 
                                {

                                TempData["existinPool"] = cvm_post.CategoryofFoodDTO.CategoryName_of_Foods;

                                HttpContext.Session.SetObject("manipulatedData_ctg", old_cof);

                                TempData["ValidError_Name"] = "valid";
                                } // aynı isim kalmış demektir
                                else
                                {
                                    HttpContext.Session.SetObject("manipulatedData_ctg", cvm_post.CategoryofFoodDTO);
                                }
                            }
                            else // valid durumu
                            {
                                HttpContext.Session.SetObject("manipulatedData_ctg", cvm_post.CategoryofFoodDTO);

                                // TempData["ValidError_Name"] = "valid";
                            }

                        }

                        if (cvm_post.UserCategoryJunctionDTO.CategoryofFood_Status != 0)
                        {
                            HttpContext.Session.SetObject("manipulatedData_ucj", cvm_post.UserCategoryJunctionDTO);

                        }

                        //if (cvm_post.CategoryofFoodDTO != null)
                        //{
                        //    HttpContext.Session.SetObject("manipulatedData_ctg", cvm_post.CategoryofFoodDTO);
                        //}

                        TempData["ValidError_General"] = "valid";

                    }


                    CategoryofFoodVM cVM = new CategoryofFoodVM();

                    TempData["JavascriptToRun"] = "valid";


                    if (cvm_post.CategoryofFoodDTO.ID != 0) //update
                    {
                        // ,{CurrentUser.Id}
                        cVM.JavascriptToRun = $"ShowErrorUpdateOperationPopup({cvm_post.CategoryofFoodDTO.ID})";
                        return RedirectToAction("CategoryofFoodList_forMember", new { JSpopupPage = cVM.JavascriptToRun, onlyOnce = "1" });

                    }
                    else // add // (pvm_post.FoodDTO.ID == 0) 
                    {
                        // pvm.JavascriptToRun = $"ShowErrorPopup( {pvm_post.FoodDTO} )";

                        // pvm.JavascriptToRun = $"ShowErrorInsertOperationPopup()";

                        TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";
                        return RedirectToAction("CategoryofFoodList_forMember", new { JSpopupPage = TempData["JSpopupPage"].ToString(), onlyOnce = "1" });

                    }


                }

            }
            else // DELETE CRUD 
            {                 
                Guid new_userID = CurrentUser.Id;               

                UserCategoryJunction userCategoryJunction = new UserCategoryJunction();

                userCategoryJunction.CategoryofFoodID = cvm_post.CategoryofFoodDTO.ID;
                userCategoryJunction.DataStatus = DataStatus.Deleted;
                userCategoryJunction.DeletedDate = DateTime.Now;
                userCategoryJunction.AccessibleID = CurrentUser.AccessibleID;
                userCategoryJunction.AppUser = CurrentUser;
                userCategoryJunction.CategoryofFood_Status = ExistentStatus.Pasif;

                _iucjm.Delete_OldCategory_from_User(CurrentUser.AccessibleID, userCategoryJunction);

                TempData["messageCategoryofFood"] = "Kategori silindi";
                TempData["Deleted"] = null;

                return RedirectToAction("CategoryofFoodList_forMember", new {onlyOnce = "1" });

            }

        }


        //CategoryofFood_InPool_Ajax
        [Route("CategoryofFood_InPool_Ajax")]
        public async Task<IActionResult> CategoryofFood_InPool_Ajax(string poolID, string? JSpopupPage)
        {
            // string a = poolID;
            // var b = poolID2;

            if (poolID.ToLower() == "false")
            {
                return RedirectToAction("CategoryofFoodList_forMember");

            }

            else // poolID.ToLower() == "true"
            {
                return RedirectToAction("CategoryofFoodList_forOtherUsers");
            }

        }

        [Route("CategoryofFoodListforOtherUsers")]
        public async Task<IActionResult> CategoryofFoodList_forOtherUsers(string poolID, string? JSpopupPage)
        {
            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null; // pop-up sıfırlanır yoksa sayfayı reflesleyince geliyor
            }

            List<CategoryofFood> UserCategoryJunctionList = await _iucjm.Get_ByAll_exceptUserID_Async(CurrentUser.Id); // IdentityUser'dan gelen Id (Guid tipli)

            CategoryofFoodVM cvm = new CategoryofFoodVM
            {
                CategoryofFoodDTOs = UserCategoryJunctionList.Adapt<List<CategoryofFoodDTO>>(),
                JavascriptToRun = JSpopupPage
            };

            // return PartialView("_denemePartial");
            // 
            // return PartialView("_CategoryofFoodListforOtherUsers_Partial", cvm);
            return View("CategoryofFoodListforOtherUsers", cvm);

        }

        // 
        [Route("Add_CategoryofFood_toMyList_Ajax")]
        public async Task<PartialViewResult> Add_CategoryofFood_toMyList_Ajax(short categoryID)
        {

            CategoryofFood CategoryofFood_item = await _icm.GetByIdAsync(categoryID);
            CategoryofFoodDTO cDTO = CategoryofFood_item.Adapt<CategoryofFoodDTO>();


            ViewBag.CategoryofFoodName_toMyList = cDTO.CategoryName_of_Foods;
            ViewBag.CRUD = "add_operation";

            CategoryofFoodVM cVM = new CategoryofFoodVM
            {
                CategoryofFoodDTO = cDTO
            };

            return PartialView("_CrudCategoryofFood_InOtherUsersList_Partial", cVM);
        }

        [Route("CRUDCategoryofFood_InOtherUsersList")]
        [HttpPost]
        public async Task<IActionResult> CRUDCategoryofFood_InOtherUsersList(CategoryofFoodVM cvm_post)
        {
            // Guid userID = CurrentUser.Id;
            // Guid accessibleID = CurrentUser.AccessibleID;
            AppUser _userInfo = CurrentUser;

            if (TempData["Added"] != null)
            {
                // TempData["_accessibleID"] = accessibleID;
                string result_Message = await _iucjm.Control_IsExisted_InMyListBefore_Async(_userInfo.Id, cvm_post.CategoryofFoodDTO.ID, _userInfo);
 
                TempData["messageCategoryofFood_InOtherUsersList"] = result_Message;                

                TempData["Added"] = null;

                return RedirectToAction("CategoryofFoodList_forMember", new { onlyOnce = "1" });

            }

            return RedirectToAction("CategoryofFoodList_forMember", new { onlyOnce = "1" });


            //IEnumerable<object> ucj = null;
            //Guid userID = CurrentUser.Id;
            //List<UserCategoryJunction> ucj_Delete = new List<UserCategoryJunction>();


            //ucj_Delete = ucj.Adapt<IEnumerable<UserCategoryJunction>>().ToList();


            //ucj = await _iucjm.Get_ByUserID_with_CategoryID_Async(new_userID, cvm_post.CategoryofFoodDTO.ID);
            //ucj_Delete = ucj.Adapt<IEnumerable<UserCategoryJunction>>().ToList();

            //UserCategoryJunction userCategoryJunction = new UserCategoryJunction();

            //userCategoryJunction.CategoryofFoodID = cvm_post.CategoryofFoodDTO.ID;
            //userCategoryJunction.DataStatus = DataStatus.Deleted;
            //userCategoryJunction.DeletedDate = DateTime.Now;
            //userCategoryJunction.AccessibleID = CurrentUser.AccessibleID;
            //userCategoryJunction.AppUser = CurrentUser;
            //userCategoryJunction.CategoryofFood_Status = ExistentStatus.Pasif;

            //_iucjm.Delete_OldCategory_from_User(CurrentUser.AccessibleID, cvm_post.CategoryofFoodDTO.ID, userCategoryJunction);

            // _iucjm.Delete(ucj_Delete[0]);
            // _iucjm.Delete(ucj_Delete.FirstOrDefault());





        }


    }
}
