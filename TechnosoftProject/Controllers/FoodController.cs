using Mapster;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [Authorize(Policy = "Confirmed_Member_Policy")]
    [Route("Food")]
    public class FoodController : BaseController
    {

        readonly IFoodManager _ifm;
        readonly IUserFoodJunctionManager _iufjm;

        public FoodController(UserManager<AppUser> userManager, RoleManager<AppRole>    roleManager, IFoodManager ifm, IUserFoodJunctionManager iufjm) : base  (userManager, null, roleManager)
        {
            _ifm = ifm;
            _iufjm = iufjm;
        }

        [Route("FoodList_forMember")]
        public async Task<IActionResult> FoodList_forMember(string? JSpopupPage, string? onlyOnce)
        {
            //SeedRoles.Seedv2();
            
            if (HttpContext.Session.GetObject<string>("hold_new_valid_food_name") != null)
            {
                HttpContext.Session.SetObject("hold_new_valid_food_name", null);
            }


            TempData["onlyOnce"] = onlyOnce;

            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null; // pop-up sıfırlanır yoksa sayfayı reflesleyince geliyor
            }

            string msj;
            if (TempData["messageFood_InOtherUsersList"] != null)
            {
                msj = TempData["messageFood_InOtherUsersList"].ToString();
                TempData["messageFood_InOtherUsersList"] = msj;
            }


            IEnumerable<object> UserFoodJunctionList = await _iufjm.Get_ByUserID_Async(CurrentUser.Id); // IdentityUser'dan gelen Id (Guid tipli)

            //IEnumerable<int> _diziler = await _iufjm.diziler(); // IdentityUser'dan gelen Id (Guid tipli)

            /*
             * 
             TypeAdapterConfig<object, UserFoodJunctionDTO>
            .NewConfig()
            .Map(dest => dest.ImageofFoodDTO, src => src.); 

            */

            FoodVM cvm = new FoodVM
            {
                UserFoodJunctionDTOs = UserFoodJunctionList.Adapt<IEnumerable<UserFoodJunctionDTO>>().ToList(),
                //diziler = _diziler.ToArray(),
                JavascriptToRun = JSpopupPage
            };

            return View("FoodListforMember", cvm);
        }

        [Route("AddFoodAjax")] // insert
        public async Task<PartialViewResult> AddFoodAjax()
        {
            FoodDTO fDTO = new FoodDTO();
            UserFoodJunctionDTO ufjDTO = new UserFoodJunctionDTO();


            // validationdan geçti ama aynı veri db'de varsa !!
            if (TempData["ValidError_NameExist"] != null)
            {

                //result_fd = HttpContext.Session.GetObject<FoodDTO>("manipulatedData_fd");
                //fDTO = result_fd;

                ufjDTO = HttpContext.Session.GetObject<UserFoodJunctionDTO>("manipulatedData_ufdj");
                // ufjDTO = result_ufdj;


                ModelState.AddModelError("FoodDTO.Food_Name", $"{TempData["existinPool"]}" + " " + " Yemek listenizde mevcut ya da Havuz listesinde bulunmaktadır. Listenizde bulunmamaktaysa havuz listesinden kendi listenize de ekleyebilirsiniz.");

                TempData["ValidError_NameExist"] = null;
                TempData["existinPool"] = null;

                HttpContext.Session.SetObject("manipulatedData_ufdj", null);

                // HttpContext.Session.SetObject("manipulatedData_fd", null);

            }
            else if (TempData["ValidError_General"] != null) // validationdan geçemedi
            {
                //if (TempData["ValidError_NameExist"] != null)
                //{
                //    ModelState.AddModelError("FoodDTO.Food_Name", $"{TempData["existinPool"]}" + " " + " yemek listenizde mevcut ya da Havuz listesinde bulunmaktadır. Listenizde bulunmamaktaysa havuz listesinden kendi listenize de ekleyebilirsiniz.");

                //    TempData["ValidError_NameExist"] = null; // "FoodDTO.Food_Name", $"{TempData["existinPool"]}"   ***** ***** ***** *
                //    TempData["existinPool"] = null;

                //}

                // kullacının girdiği verileri tekrar girmemesi için
                if (HttpContext.Session.GetObject<FoodDTO>("manipulatedData_fd") != null)
                {
                    fDTO = HttpContext.Session.GetObject<FoodDTO>("manipulatedData_fd");
                    // fDTO = result_fd;
                }

                if (HttpContext.Session.GetObject<UserFoodJunctionDTO>("manipulatedData_ufdj") != null)
                {
                    ufjDTO = HttpContext.Session.GetObject<UserFoodJunctionDTO>("manipulatedData_ufdj");
                    //ufjDTO = result_ufdj;
                }



                
                if (TempData["ValidError_Name"] != null)
                {

                    if (string.IsNullOrEmpty(fDTO.Food_Name))
                    {
                        ModelState.AddModelError("FoodDTO.Food_Name", "Yemek adı giriniz.");
                    }
                    else if (fDTO.Food_Name.Length >= 30)
                    {
                        ModelState.AddModelError("FoodDTO.Food_Name", "Yemek 30 karakterden fazla olamaz.");
                    }

                    TempData["ValidError_Name"] = null;
                }

                if (TempData["ValidError_Status"] != null)
                {
                    ModelState.AddModelError("UserFoodJunctionDTO.Food_Status", "Yemek durumunu giriniz.");

                    TempData["ValidError_Status"] = null;
                }

                // Price 1000 tl
                if (TempData["ValidError_Price"] != null)
                {
                    ModelState.AddModelError("UserFoodJunctionDTO.Food_Price", "Yemek ücreti geçersiz.");

                    TempData["ValidError_Price"] = null;

                }

                if (TempData["ValidError_Description"] != null)
                {
                    if (ufjDTO.Food_Description.Length >= 256)
                    {
                        ModelState.AddModelError("UserFoodJunctionDTO.Food_Description", "Açıklama 256 karakterden fazla olmamalı.");
                    }

                    TempData["ValidError_Description"] = null;
                }


                TempData["ValidError_General"] = null;

                HttpContext.Session.SetObject("manipulatedData_fd", null);
                HttpContext.Session.SetObject("manipulatedData_ufdj", null);
            }



            FoodVM fVM = new FoodVM
            {
                UserFoodJunctionDTO = ufjDTO,
                FoodDTO = fDTO
            };



            Thread.Sleep(500); // pop-up sayfasını tekrar açmayı tetikleyince bazen gelmiyor o yüzden bu kod eklendi..

            return PartialView("_CrudFood_Partial", fVM);

        }

        [Route("UpdateFoodAjax")] // update
        public async Task<PartialViewResult> UpdateFoodAjax(short foodID/*, Guid userID*/)
        {

            Guid _userID = CurrentUser.Id; // userID ÇEVİR

            // FoodDTO result_fd = new FoodDTO();
            FoodDTO fDTO = new FoodDTO();

            // var result_ufdj = new List<UserFoodJunctionDTO>();
            List<UserFoodJunctionDTO> ufjDTO = new List<UserFoodJunctionDTO>();
            var result_ufdj = new UserFoodJunctionDTO();  // ***** ***** ***** *


            // kullacının girdiği verileri tekrar girmemesi için
            if (HttpContext.Session.GetObject<FoodDTO>("manipulatedData_fd") != null)
            {
                fDTO = HttpContext.Session.GetObject<FoodDTO>("manipulatedData_fd");
                
            }
            
            if (HttpContext.Session.GetObject<UserFoodJunctionDTO>("manipulatedData_ufdj") != null)
            {
                // result_ufdj = HttpContext.Session.GetObject<List<UserFoodJunctionDTO>>("manipulatedData_ufdj");

                result_ufdj = HttpContext.Session.GetObject<UserFoodJunctionDTO>("manipulatedData_ufdj");
                ufjDTO.Add(result_ufdj);
                // ufjDTO = result_ufdj;
            }

            IEnumerable<object> ucj = null; // ***** ***** ***** *
            // Food Food_item = null;




            if (TempData["ValidError_NameExist"] != null) // 1.validasyon kontrolü 
            {
                //result_ufdj = HttpContext.Session.GetObject<UserFoodJunctionDTO>("manipulatedData_ufdj");
                //ufjDTO.Add(result_ufdj);
                ////ufjDTO = result_ufdj;


                //fDTO = HttpContext.Session.GetObject<FoodDTO>("manipulatedData_fd");


                ModelState.AddModelError("FoodDTO.Food_Name", $"{TempData["existinPool"]}" + " " + " Yemek listenizde mevcut ya da Havuz listesinde bulunmaktadır. Listenizde bulunmamaktaysa havuz listesinden kendi listenize de ekleyebilirsiniz.");
                
                TempData["ValidError_NameExist"] = null;
                TempData["existinPool"] = null;


                HttpContext.Session.SetObject("manipulatedData_fd", null);
                HttpContext.Session.SetObject("manipulatedData_ufdj", null);


                //HttpContext.Session.SetObject("will_be_deleted_UserFoodJuncData", ufjDTO[0]);
                //HttpContext.Session.SetObject("will_be_deleted_FoodData", fDTO);

            }
            else if (TempData["ValidError_General"] != null) // 2.validasyon kontrolü 
            {
                //if (TempData["ValidError_NameExist"] != null)
                //{
                //    ModelState.AddModelError("FoodDTO.Food_Name", $"{TempData["existinPool"]}" + " " + " yemek listenizde mevcut ya da Havuz listesinde bulunmaktadır. Listenizde bulunmamaktaysa havuz listesinden kendi listenize de ekleyebilirsiniz.");

                //    TempData["ValidError_NameExist"] = null; // "FoodDTO.Food_Name", $"{TempData["existinPool"]}"   ***** ***** ***** *
                //    TempData["existinPool"] = null;                   

                //}

                //if (HttpContext.Session.GetObject<FoodDTO>("manipulatedData_fd") != null)
                //{
                //    fDTO = HttpContext.Session.GetObject<FoodDTO>("manipulatedData_fd");
                //    // fDTO = result_fd;
                //}

                //if (HttpContext.Session.GetObject<UserFoodJunctionDTO>("manipulatedData_ufdj") != null)
                //{
                //    result_ufdj = HttpContext.Session.GetObject<UserFoodJunctionDTO>("manipulatedData_ufdj");
                //    ufjDTO.Add(result_ufdj);
                //}



                if (TempData["ValidError_Name"] != null)
                {

                    //if (string.IsNullOrEmpty(fDTO.Food_Name))
                    //{
                    //    ModelState.AddModelError("FoodDTO.Food_Name", "Yemek adı giriniz.");
                    //}
                    if (TempData["NullOrEmpty"] != null)
                    {
                        ModelState.AddModelError("FoodDTO.Food_Name", "Yemek adı giriniz.");
                    }
                    else if (TempData["existinpool"] != null)
                    {
                        ModelState.AddModelError("FoodDTO.Food_Name", $"{TempData["existinPool"]}" + " " + " yemek listenizde mevcut ya da Havuz listesinde bulunmaktadır. Listenizde bulunmamaktaysa havuz listesinden kendi listenize de ekleyebilirsiniz.");
                    }
                    //else if (fDTO.Food_Name.Length >= 30)
                    //{
                    //    ModelState.AddModelError("FoodDTO.Food_Name", "Yemek 30 karakterden fazla olamaz.");
                    //}

                    TempData["ValidError_Name"] = null;
                }

                if (TempData["ValidError_Status"] != null)
                {
                    ModelState.AddModelError("UserFoodJunctionDTO.Food_Status", "Yemek durumunu giriniz.");

                    TempData["ValidError_Status"] = null;
                }

                // Price 1000 tl
                if (TempData["ValidError_Price"] != null)
                {
                    ModelState.AddModelError("UserFoodJunctionDTO.Food_Price", "Yemek ücreti geçersiz.");

                    TempData["ValidError_Price"] = null;

                }

                //if (TempData["ValidError_Description"] != null)
                //{
                //    if (ufjDTO[0].Food_Description.Length >= 256)
                //    {
                //        ModelState.AddModelError("UserFoodJunctionDTO.Food_Description", "Açıklama 256 karakterden fazla olmamalı.");
                //    }

                //    TempData["ValidError_Description"] = null;
                //}

                TempData["ValidError_General"] = null;
                //result_2 = HttpContext.Session.GetObject<UserFoodJunctionDTO>("manipulatedData_ufdj");
                //ufjDTO.Add(result_2);

                // fDTO = HttpContext.Session.GetObject<FoodDTO>("manipulatedData_fd");

                //HttpContext.Session.SetObject("willbedeletedUserFoodJuncData", ufjDTO[0]);
                //HttpContext.Session.SetObject("willbedeletedFoodData", fDTO);
                HttpContext.Session.SetObject("manipulatedData_fd", null);
                HttpContext.Session.SetObject("manipulatedData_ufdj", null);
            }
            
            else // ilk güncelleme denemesi ise
            {
                // Food_item = await _ifm.GetByIdAsync(foodID);
                fDTO = (await _ifm.GetByIdAsync(foodID)).Adapt<FoodDTO>();

                ucj = await _iufjm.Get_ByUserID_with_FoodID_Async(_userID, foodID);
                ufjDTO = ucj.Adapt<IEnumerable<UserFoodJunctionDTO>>().ToList();
                //UserCategoryJunctionDTOs = UserCategoryJunctionList.Adapt<IEnumerable<UserCategoryJunctionDTO>>().ToList(),
                //                 UserCategoryJunctionDTOs = UserCategoryJunctionList.Adapt<IEnumerable<UserCategoryJunctionDTO>>().ToList(),

                // GetByIdAsync (Repository) int --> short 

                //HttpContext.Session.SetObject("will_be_deleted_UserFoodJuncData", ufjDTO[0]);
                //HttpContext.Session.SetObject("will_be_deleted_FoodData", fDTO);

            }
             
            // HttpContext.Session.SetObject("manipulatedData_ufdj", null);
            // HttpContext.Session.SetObject("manipulatedData_fd", null);


            HttpContext.Session.SetObject("will_be_deleted_UserFoodJuncData", ufjDTO[0]);
            HttpContext.Session.SetObject("will_be_deleted_FoodData", fDTO);

            // validation'Dan geliyorsa 
            if (HttpContext.Session.GetObject<string>("hold_new_valid_food_name") != null)
            {
                //  HttpContext.Session.SetObject("old_ctg_name_for_comparison", old_cof.CategoryName_of_Foods);

                fDTO.Food_Name = HttpContext.Session.GetObject<string>("hold_new_valid_food_name");
            }

            FoodVM fVM = new FoodVM
            {
                UserFoodJunctionDTO = ufjDTO[0],
                FoodDTO = fDTO
            };

            Thread.Sleep(500);
            return PartialView("_CrudFood_Partial", fVM);
        }

        [Route("DeleteFoodAjax")]
        public async Task<PartialViewResult> DeleteFoodAjax(short foodID)
        {

            Food CategoryofFood_item = await _ifm.GetByIdAsync(foodID);
            FoodDTO fDTO = CategoryofFood_item.Adapt<FoodDTO>();


            //IEnumerable<object> ucj = null;
            //Guid new_userID = CurrentUser.Id;
            //List<UserCategoryJunctionDTO> ucjDTO = new List<UserCategoryJunctionDTO>();


            //ucj = await _iucjm.Get_ByUserID_with_CategoryID_Async(new_userID, categoryID);
            //ucjDTO = ucj.Adapt<IEnumerable<UserCategoryJunctionDTO>>().ToList();



            ViewBag.FoodNameDelete = fDTO.Food_Name;
            ViewBag.CRUD = "delete_operation";

            FoodVM fVM = new FoodVM
            {
                FoodDTO = fDTO
            };

            return PartialView("_CrudFood_Partial", fVM);
        }



        [Route("CRUDFood")]
        [HttpPost]
        public async Task<IActionResult> CRUDFood(FoodVM fvm_post, IFormFile _FoodPicture)
        {

            UserFoodJunctionDTO old_ufj = null;
            FoodDTO old_fd = null;


            if (HttpContext.Session.GetObject<UserFoodJunctionDTO>("will_be_deleted_UserFoodJuncData") != null && HttpContext.Session.GetObject<FoodDTO>("will_be_deleted_FoodData") != null)
            {

                old_ufj = new UserFoodJunctionDTO();
                old_ufj = HttpContext.Session.GetObject<UserFoodJunctionDTO>("will_be_deleted_UserFoodJuncData");

                old_fd = new FoodDTO();
                old_fd = HttpContext.Session.GetObject<FoodDTO>("will_be_deleted_FoodData");

                HttpContext.Session.SetObject("will_be_deleted_UserFoodJuncData", null);
                HttpContext.Session.SetObject("will_be_deleted_FoodData", null);

            }

            if (TempData["Deleted"] == null)
            {

                //ModelState.Remove("FoodPicture");
                //ModelState.Remove("FoodDTOs");
                //ModelState.Remove("JavascriptToRun");

                ModelState.Remove("ExistentStatus");
                ModelState.Remove("_FoodPicture"); // IFormFile _FoodPicture İÇİN

                // ModelState.Remove("UserFoodJunctionDTO.CategoryName_of_Foods");
                ModelState.Remove("UserFoodJunctionDTO.Food_Name");

                // DÜZENLENMESİ GEREKİYOR  --> FoodDTO.cs 
                // ModelState.Remove("FoodDTO.Food_Name");
                ModelState.Remove("FoodDTO.FoodPrice");
                ModelState.Remove("FoodDTO.ExistentStatus");
                ModelState.Remove("UserFoodJunctionDTO.ImageofFoodDTO");
                ModelState.Remove("UserFoodJunctionDTO.ImageofFoods");

                

                if (ModelState.IsValid)
                {
                    Food fd_add = null;
                    Food fd_update = null;
                    short old_foodID = 0;

                    if (fvm_post.FoodDTO.ID == 0)
                    {
                        fd_add = fvm_post.FoodDTO.Adapt<Food>();
                    }
                    else
                    {
                        old_foodID = fvm_post.FoodDTO.ID;
                        fd_update = new Food();
                        fd_update.Food_Name = fvm_post.FoodDTO.Food_Name;
                    }

                    UserFoodJunction ufj = fvm_post.UserFoodJunctionDTO.Adapt<UserFoodJunction>();

                    /* !!! !!! ctg.Status = (int)fvm_post.FoodDTO.Status; !!! !!!*/

                    if (_FoodPicture != null && _FoodPicture.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid().ToString()} {Path.GetExtension(_FoodPicture.FileName)}"; // path oluşturma

                        /* var fileName = Guid.NewGuid().ToString() + Path.GetExtension(_FoodPicture.FileName); */

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/FoodPicture", fileName); // server'a kayıt edilecek path => wwwroot/UserPicture/fileName

                        // kayıt işlemi
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await _FoodPicture.CopyToAsync(stream); // userPicture'ı, stream'e kayıt

                            /* !!! !!! ctg.FoodPicture = "/FoodPicture/" + fileName;   // veritabanına kayıt (wwwroot belirtmeye gerek yok) !!! !!! */

                            ufj.Food_Picture = "/FoodPicture/" + fileName;

                        }
                    }// *!*
                    //else // resim yüklenmedi ise...
                    //{
                    //    // mevcut da resmi db ' de varsa ... mevcut resmi db den çekip tekrar set etmeye gerek yok gibi ?! --> _icm.Update(ctg); kısmında bir kontrol et !!!

                    //    /*if (ucj.DataStatus == DataStatus.Updated || .Inserted)
                    //    {*/
                    //    UserFoodJunctionDTO ucj_controller_2 = null;
                    //    IEnumerable<object> ucj_controller = await _iufjm.Get_ByUserID_with_CategoryID_Async(CurrentUser.Id, fvm_post.FoodDTO.ID);//await _iucjm.GetByIdAsync(fvm_post.FoodDTO.ID);

                    //    ucj_controller_2 = ucj_controller.FirstOrDefault().Adapt<UserFoodJunctionDTO>();

                    //    if (ucj_controller_2 != null)
                    //    {

                    //        if (ucj_controller_2.Food_Picture != null)
                    //        {
                    //            ucj.Food_Picture = ucj_controller_2.Food_Picture;
                    //        }

                    //    }

                    //    /*}*/
                    //}

                    if (fvm_post.FoodDTO.ID == 0) // insert
                    {
                        // Aynı isimli kayıt db'de zaten varsa 
                        if (await _ifm.Any(x => x.Food_Name == fd_add.Food_Name))
                        {
                            // HttpContext.Session.SetObject("manipulatedData_NameExist", fvm_post.FoodDTO);

                            TempData["ValidError_NameExist"] = "valid";

                            HttpContext.Session.SetObject("manipulatedData_ufdj", fvm_post.UserFoodJunctionDTO);

                            TempData["existinPool"] = fvm_post.FoodDTO.Food_Name;

                            TempData["JavascriptToRun"] = "valid";
                            TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";


                            return RedirectToAction("FoodList_forMember", new { JSpopupPage = TempData["JSpopupPage"].ToString(), onlyOnce = "1" });


                        }

                        await _ifm.AddAsync(fd_add);

                        ufj.AccessibleID = CurrentUser.AccessibleID;
                        ufj.FoodID = fd_add.ID;

                        // short control = CurrentUser.ID; olmadı // AppUser'dan gelen Id olmuyor (short tipli), ignore edilmişti zaten
                        // Guid control2 = CurrentUser.Id;
                        ufj.AppUser = CurrentUser; // IdentityUser'ın Id'si de tabloya eklnemiş oldu..
                        //ucj.AppUser.Id = CurrentUser.Id;

                        await _iufjm.AddAsync(ufj);
                        TempData["messageFood"] = "Yemek eklendi";



                        return RedirectToAction("FoodList_forMember", new { onlyOnce = "1" });

                    }
                    
                    else // update 
                    {
                        // Yeni bir yemek ismi
                        if (old_fd.Food_Name != fd_update.Food_Name)
                        {

                            // Yeni Yemek eğer havuzda zaten varsa 
                            if (await _ifm.Any(x => x.Food_Name == fd_update.Food_Name))
                            {


                                TempData["ValidError_NameExist"] = "valid";
                                TempData["JavascriptToRun"] = "valid";

                                TempData["existinPool"] = fvm_post.FoodDTO.Food_Name;
                                // eski veri tekrardan set edildi...
                                HttpContext.Session.SetObject("manipulatedData_ufdj", fvm_post.UserFoodJunctionDTO);
                                HttpContext.Session.SetObject("manipulatedData_fd", old_fd);

                                // ,{CurrentUser.Id}
                                TempData["JSpopupPage"] = $"ShowErrorUpdateOperationPopup({fvm_post.FoodDTO.ID})"; // diğer paramaetre de eklenecek


                                return RedirectToAction("FoodList_forMember", new { JSpopupPage = TempData["JSpopupPage"].ToString(), onlyOnce = "1" });
                            }

                            // Yeni Yemek iSMİ eğer havuzda da yoksa 
                            else
                            {
                                // yeni bir Yemek olarak eklenecek ... güncellenmeyecek... güncelleme olmayacak
                                await _ifm.AddAsync(fd_update);
                                // _icm.Update(ctg);

                                // usercategory olarak da eklenecek...
                                ufj.AccessibleID = CurrentUser.AccessibleID;
                                // ucj.AppUser.Id = CurrentUser.Id; erişilemiyor
                                ufj.FoodID = fd_update.ID; //*** 
                                ufj.AppUser = CurrentUser; // IdentityUser'ın Id'si de tabloya eklnemiş oldu..

                                await _iufjm.AddAsync(ufj);

                                // builder.HasKey(x => new { x.AccessibleID, x.FoodID }); sayesinde 
                                // _iucjm.Update_OldCategory_with_NewOne(CurrentUser.AccessibleID, old_categoryID, ucj); // await yok // IdentityUser'dan gelen Id (Guid tipli)

                                // eski usercategory pasife alınacak...
                                // _iucjm.Delete(old_ufj);


                                //  OLD food become passive FOR THİS USER
                                /* UserFoodJunction passive_UserFoodJunction = old_ufj.Adapt<UserFoodJunction>();

                                passive_UserFoodJunction.DataStatus = DataStatus.Deleted;
                                passive_UserFoodJunction.DeletedDate = DateTime.Now;
                                passive_UserFoodJunction.AccessibleID = CurrentUser.AccessibleID;
                                passive_UserFoodJunction.AppUser = CurrentUser;
                                passive_UserFoodJunction.Food_Status = ExistentStatus.Pasif;
                                */

                                // passive_UserFoodJunction.FoodID = old_fd.ID;
                                _iufjm.Delete_OldFood_from_User(old_ufj.FoodID, CurrentUser);

                                if (HttpContext.Session.GetObject<string>("hold_new_valid_food_name") != null)
                                {
                                    HttpContext.Session.SetObject("hold_new_valid_food_name", null);
                                }

                                TempData["messageFood"] = "Yemek güncellendi";

                                return RedirectToAction("FoodList_forMember", new { onlyOnce = "1" });

                            }


                        }

                        else // eski Yemek İSMİ / SADECE diğer veriler update olunacak ...
                             // old_fd.CategoryName_of_Foods == fd_update.CategoryName_of_Foods
                        {
                            //old_ufj.AppUser.Id = CurrentUser.Id;

                            //if (await _icm.Any(x => x.CategoryName_of_Foods == fd_update.CategoryName_of_Foods) && await _iucjm.Any(x => x. == fd_update.CategoryName_of_Foods))
                            //{
                            //    HttpContext.Session.SetObject("manipulatedData_NameExist", fvm_post.FoodDTO);

                            //    TempData["ValidError_NameExist"] = "valid";
                            //    TempData["JavascriptToRun"] = "valid";

                            //    HttpContext.Session.SetObject("manipulatedData_Status", fvm_post.UserFoodJunctionDTO);

                            //    // ,{CurrentUser.Id}
                            //    TempData["JSpopupPage"] = $"ShowErrorUpdateOperationPopup({fvm_post.FoodDTO.ID})"; // diğer paramaetre de eklenecek

                            //    return RedirectToAction("FoodList", new { JSpopupPage = TempData["JSpopupPage"].ToString() });
                            //}


                            // ucj tablosunda değişiklik var
                            // if (old_ufj.Food_Status != fvm_post.UserFoodJunctionDTO.Food_Status) {

                            // _iucjm.Update(ucj);
                            // old_categoryID = fvm_post.FoodDTO.ID;

                            ufj.DataStatus = DataStatus.Updated;
                            ufj.ModifiedDate = DateTime.Now;
                            ufj.AccessibleID = CurrentUser.AccessibleID;
                            ufj.AppUser = CurrentUser;

                            ufj.Food = fvm_post.FoodDTO.Adapt<Food>();
                            ufj.FoodID = fvm_post.FoodDTO.ID;
                            //  ucj.Food_Picture = "/FoodPicture/" + fileName;


                            _iufjm.Update_UserFoodJuncTable(CurrentUser.AccessibleID, fvm_post.FoodDTO.ID, ufj);

                            if (HttpContext.Session.GetObject<string>("hold_new_valid_food_name") != null)
                            {
                                HttpContext.Session.SetObject("hold_new_valid_food_name", null);
                            }

                            TempData["messageFood"] = "Yemek güncellendi";

                            // else { değişiklik olmadı mesajı }
                            Thread.Sleep(3800);

                            return RedirectToAction("FoodList_forMember", new { onlyOnce = "1" });

                        }



                    }


                }
                // else --> validation olmayan kod kısmı
                else
                {
                    // TempData["mesaj"] = "Yemek adı ve statü giriniz..";
                    // ModelState.AddModelError("", "Ürün adı ve statü giriniz..");

                    if (fvm_post.FoodDTO.ID == 0)
                    {
                        if (fvm_post.UserFoodJunctionDTO.Food_Status == 0)
                        {
                            TempData["ValidError_Status"] = "valid";
                        }

                        if (String.IsNullOrEmpty(fvm_post.FoodDTO.Food_Name) || fvm_post.FoodDTO.Food_Name.Length >= 30 )
                        {
                            TempData["ValidError_Name"] = "valid";
                        }

                        if (fvm_post.UserFoodJunctionDTO.Food_Description == null)
                        {
                            fvm_post.UserFoodJunctionDTO.Food_Description = "";
                        }

                        if (fvm_post.UserFoodJunctionDTO.Food_Description.Length >= 256)
                        {
                            fvm_post.UserFoodJunctionDTO.Food_Description = "";
                            TempData["ValidError_Description"] = "valid";
                        }

                        if (fvm_post.UserFoodJunctionDTO.Food_Price == null || fvm_post.UserFoodJunctionDTO.Food_Price == 0 || fvm_post.UserFoodJunctionDTO.Food_Price >= 1000)
                        {
                            TempData["ValidError_Price"] = "valid";
                        }

                        if (fvm_post.FoodDTO != null)
                        {
                            HttpContext.Session.SetObject("manipulatedData_fd", fvm_post.FoodDTO);
                        }

                        if (fvm_post.UserFoodJunctionDTO != null)
                        {
                            HttpContext.Session.SetObject("manipulatedData_ufdj", fvm_post.UserFoodJunctionDTO);
                        }



                        TempData["ValidError_General"] = "valid";

                        if (fvm_post.FoodDTO != null)
                        {
                            HttpContext.Session.SetObject("manipulatedData_fd", fvm_post.FoodDTO);
                        }

                        if (fvm_post.UserFoodJunctionDTO != null)
                        {
                            HttpContext.Session.SetObject("manipulatedData_ufdj", fvm_post.UserFoodJunctionDTO);

                        }

                    }                    
                    else// update için olan validation error
                    {
                        UserFoodJunctionDTO _tempFJ = new UserFoodJunctionDTO();
                        FoodDTO _tempF = new FoodDTO();

                        _tempF.ID = fvm_post.FoodDTO.ID;
                        _tempFJ.FoodID = fvm_post.FoodDTO.ID;
                        ////  ama girdiği isim havuzda varsa  ....
                        //if (await _ifm.Any(x => x.Food_Name == fvm_post.FoodDTO.Food_Name))
                        //{
                        //    TempData["ValidError_NameExist"] = "valid";
                        //    TempData["existinPool"] = fvm_post.FoodDTO.Food_Name;
                        //}

                        if (fvm_post.UserFoodJunctionDTO.Food_Status == 0)
                        {
                            TempData["ValidError_Status"] = "valid";
                            _tempFJ.Food_Status = old_ufj.Food_Status;
                            // HttpContext.Session.SetObject("manipulatedData_ufdj", old_ufj);
                        }
                        else
                        {
                            _tempFJ.Food_Status = fvm_post.UserFoodJunctionDTO.Food_Status;
                        }

                        //// 
                        if (String.IsNullOrEmpty(fvm_post.FoodDTO.Food_Name) /*|| fvm_post.FoodDTO.Food_Name.Length >= 30*/)
                        {
                            TempData["NullOrEmpty"] = "valid";
                            TempData["ValidError_Name"] = "valid";
                            _tempF.Food_Name = old_fd.Food_Name;
                        }
                        else
                        {
                            if (await _ifm.Any(x => x.Food_Name == fvm_post.FoodDTO.Food_Name))
                            {
                                if (fvm_post.FoodDTO.Food_Name != old_fd.Food_Name)
                                {

                                    TempData["existinPool"] = fvm_post.FoodDTO.Food_Name;

                                    _tempF.Food_Name = old_fd.Food_Name;

                                    TempData["ValidError_Name"] = "valid";
                                } // aynı isim kalmış demektir
                                else
                                {
                                    _tempF.Food_Name = fvm_post.FoodDTO.Food_Name;
                                }
                            }

                            else // valid durumu
                            {
                                // _tempF.Food_Name = fvm_post.FoodDTO.Food_Name;
                                _tempF.Food_Name = old_fd.Food_Name;


                                HttpContext.Session.SetObject("hold_new_valid_food_name", fvm_post.FoodDTO.Food_Name);
                                // HttpContext.Session.SetObject("manipulatedData_ctg", old_cof);


                                // TempData["ValidError_Name"] = "valid";
                            } 

                        }

                        if (fvm_post.UserFoodJunctionDTO.Food_Description == null)
                        {
                            fvm_post.UserFoodJunctionDTO.Food_Description = "";
                        }

                        // KENDİ YAPIYPR... 
                        //if (fvm_post.UserFoodJunctionDTO.Food_Description.Length >= 256)
                        //{
                        //    // fvm_post.UserFoodJunctionDTO.Food_Description = "";
                        //    TempData["ValidError_Description"] = "valid";
                        //    if (String.IsNullOrEmpty(old_ufj.Food_Description))
                        //    {
                        //        _tempFJ.Food_Description = "";
                        //    }
                        //    else
                        //    {
                        //        _tempFJ.Food_Description = old_ufj.Food_Description;
                        //    }

                        //}

                        if (fvm_post.UserFoodJunctionDTO.Food_Price == null || fvm_post.UserFoodJunctionDTO.Food_Price == 0 || fvm_post.UserFoodJunctionDTO.Food_Price >= 1000)
                        {
                            TempData["ValidError_Price"] = "valid";
                            _tempFJ.Food_Price = old_ufj.Food_Price;
                        }
                        else
                        {
                            _tempFJ.Food_Price = fvm_post.UserFoodJunctionDTO.Food_Price;
                        }

                        TempData["ValidError_General"] = "valid";

                            HttpContext.Session.SetObject("manipulatedData_fd", _tempF);
                        


                            HttpContext.Session.SetObject("manipulatedData_ufdj", _tempFJ);

                        

                    }




                    // FoodVM fVM = new FoodVM();

                    TempData["JavascriptToRun"] = "valid";

                    // !*!
                    if (fvm_post.FoodDTO.ID != 0) //update
                    {
                        // fVM.JavascriptToRun = $"ShowErrorUpdateOperationPopup({fvm_post.FoodDTO.ID})";
                        TempData["JSpopupPage"] = $"ShowErrorUpdateOperationPopup({fvm_post.FoodDTO.ID})";
                        return RedirectToAction("FoodList_forMember", new { JSpopupPage = TempData["JSpopupPage"].ToString() /*fVM.JavascriptToRun*/, onlyOnce = "1" });

                    }
                    else // add // (pvm_post.FoodDTO.ID == 0) 
                    {
                        TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";
                        return RedirectToAction("FoodList_forMember", new { JSpopupPage = TempData["JSpopupPage"].ToString(), onlyOnce = "1" });

                    }

                } //  validation olmayan kod kısmının sonu

            }
            // *!* DELETE 
            else
            {
                //Guid new_userID = CurrentUser.Id;

               // UserFoodJunction UserFoodJunction = new UserFoodJunction();

                //UserFoodJunction.FoodID = fvm_post.FoodDTO.ID;
                //UserFoodJunction.DataStatus = DataStatus.Deleted;
                //UserFoodJunction.DeletedDate = DateTime.Now;
                //UserFoodJunction.AccessibleID = CurrentUser.AccessibleID;
                //UserFoodJunction.AppUser = CurrentUser;
                //UserFoodJunction.Food_Status = ExistentStatus.Pasif;

                _iufjm.Delete_OldFood_from_User(fvm_post.FoodDTO.ID, CurrentUser);


                TempData["messageFood"] = "Yemek listenizden silindi";
                TempData["Deleted"] = null;

                return RedirectToAction("FoodList_forMember", new { onlyOnce = "1" });

            }

           
        }

        // Food_InPool_Ajax --> _FoodLayout.cshtml
        [Route("Food_InPool_Ajax")]
        public async Task<IActionResult> Food_InPool_Ajax(string poolID, string? JSpopupPage)
        {
            if (poolID.ToLower() == "false")
            {
                return RedirectToAction("FoodList_forMember");

            }

            else // poolID.ToLower() == "true"
            {
                return RedirectToAction("FoodList_forOtherUsers");
            }
        }

        [Route("FoodList_forOtherUsers")]
        public async Task<IActionResult> FoodList_forOtherUsers(string poolID, string? JSpopupPage)
        {
            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null; // pop-up sıfırlanır yoksa sayfayı reflesleyince geliyor
            }

            List<Food> UserFoodJunctionList = await _iufjm.Get_ByAll_exceptUserID_Async(CurrentUser.Id); // IdentityUser'dan gelen Id (Guid tipli)

            FoodVM fvm = new FoodVM
            {
                FoodDTOs = UserFoodJunctionList.Adapt<List<FoodDTO>>(),
                JavascriptToRun = JSpopupPage
            };

            return View("FoodListforOtherUsers", fvm);

        }

        // 
        [Route("Add_Food_toMyList_Ajax")]
        public async Task<PartialViewResult> Add_Food_toMyList_Ajax(short foodID)
        {

            Food food_Item = await _ifm.GetByIdAsync(foodID);
            FoodDTO fDTO = food_Item.Adapt<FoodDTO>();

            ViewBag.FoodName_toMyList = fDTO.Food_Name;
            ViewBag.CRUD = "add_operation";

            FoodVM fVM = new FoodVM
            {
                FoodDTO = fDTO
            };

            return PartialView("_CrudFood_InOtherUsersList_Partial", fVM);
        }

        [Route("CRUDFood_InOtherUsersList")]
        [HttpPost]
        public async Task<IActionResult> CRUDFood_InOtherUsersList(FoodVM fvm_post)
        {
            // Guid userID = CurrentUser.Id;
            // Guid accessibleID = CurrentUser.AccessibleID;
            AppUser _userInfo = CurrentUser;

            if (TempData["Added"] != null)
            {
                // TempData["_accessibleID"] = accessibleID;
                string result_Message = await _iufjm.Control_IsExisted_InMyListBefore_Async(_userInfo, fvm_post.FoodDTO.ID);
                // _userInfo.Id, fvm_post.CategoryofFoodDTO.ID, _userInfo
                TempData["messageFood_InOtherUsersList"] = result_Message;

                TempData["Added"] = null;

                return RedirectToAction("FoodList_forMember", new { onlyOnce = "1" });

            }

            return RedirectToAction("FoodList_forMember", new { onlyOnce = "1" });


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

        [Route("FoodDetails")]
        public async Task<IActionResult> FoodDetails(short foodID)
        {
            IEnumerable<object> UserFoodJunctionList = await _iufjm.GetFoodDetails_of_Member_Async(CurrentUser, foodID); // IdentityUser'dan gelen Id (Guid tipli)

            FoodVM fvm = new FoodVM
            {
                UserFoodJunctionDTO = UserFoodJunctionList.FirstOrDefault().Adapt<UserFoodJunctionDTO>()
            };

            return View("FoodDetails", fvm);

         }

    }
}
