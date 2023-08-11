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

            // IEnumerable<Food> FoodList = await _icm.GetActivesAsync();

            IEnumerable<object> UserFoodJunctionList = await _iufjm.Get_ByUserID_Async(CurrentUser.Id); // IdentityUser'dan gelen Id (Guid tipli)


            FoodVM cvm = new FoodVM
            {
                UserFoodJunctionDTOs = UserFoodJunctionList.Adapt<IEnumerable<UserFoodJunctionDTO>>().ToList(),
                // FoodDTOs = FoodList.Adapt<IEnumerable<FoodDTO>>().ToList(),
                JavascriptToRun = JSpopupPage
            };

            return View("FoodListforMember", cvm);
        }

        [Route("AddFoodAjax")]
        public async Task<PartialViewResult> AddFoodAjax()
        {           
            var result_fd = new FoodDTO();
            FoodDTO fDTO = new FoodDTO();

            var result_ufdj = new UserFoodJunctionDTO();
            UserFoodJunctionDTO ufjDTO = new UserFoodJunctionDTO();


            // kullacının girdiği verileri tekrar girmemesi için
            if (HttpContext.Session.GetObject<FoodDTO>("manipulatedData_fd") != null)
            {
                result_fd = HttpContext.Session.GetObject<FoodDTO>("manipulatedData_fd");
                fDTO = result_fd;
            }

            if (HttpContext.Session.GetObject<UserFoodJunctionDTO>("manipulatedData_ufdj") != null)
            {
                result_ufdj = HttpContext.Session.GetObject<UserFoodJunctionDTO>("manipulatedData_ufdj");
                ufjDTO = result_ufdj;
            }


            // validationdan geçti ama aynı veri db'de varsa !!
            if (TempData["ValidError_NameExist"] != null)
            {

                result_fd = HttpContext.Session.GetObject<FoodDTO>("manipulatedData_NameExist");
                fDTO = result_fd;

                ModelState.AddModelError("FoodDTO.Food_Name", "Yemek listenizde mevcut ya da Havuz listesinde bulunmaktadır. Listenizde bulunmamaktaysa havuz listesinden kendi listenize de ekleyebilirsiniz.");

                TempData["ValidError_NameExist"] = null;
            }
            else // validationdan geçemedi
            {
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
            }



            FoodVM fVM = new FoodVM
            {
                UserFoodJunctionDTO = ufjDTO,
                FoodDTO = fDTO
            };

            HttpContext.Session.SetObject("manipulatedData_fd", null);
            HttpContext.Session.SetObject("manipulatedData_ufdj", null);

            Thread.Sleep(500); // pop-up sayfasını tekrar açmayı tetikleyince bazen gelmiyor o yüzden bu kod eklendi..

            return PartialView("_CrudFood_Partial", fVM);

        }

        [Route("UpdateFoodAjax")]
        public async Task<PartialViewResult> UpdateFoodAjax(short foodID/*, Guid userID*/)
        {

            Guid _userID = CurrentUser.Id; // userID ÇEVİR

            var result_fd = new FoodDTO();
            FoodDTO fDTO = new FoodDTO();

            var result_ufdj = new List<UserFoodJunctionDTO>();
            List<UserFoodJunctionDTO> ufjDTO = new List<UserFoodJunctionDTO>();
            var result_2 = new UserFoodJunctionDTO();  // ***** ***** ***** *


            // kullacının girdiği verileri tekrar girmemesi için
            if (HttpContext.Session.GetObject<FoodDTO>("manipulatedData_fd") != null)
            {
                result_fd = HttpContext.Session.GetObject<FoodDTO>("manipulatedData_fd");
                fDTO = result_fd;
            }

            if (HttpContext.Session.GetObject<UserFoodJunctionDTO>("manipulatedData_ufdj") != null)
            {
                result_ufdj = HttpContext.Session.GetObject<List<UserFoodJunctionDTO>>("manipulatedData_ufdj");
                ufjDTO = result_ufdj;
            }

            IEnumerable<object> ucj = null; // ***** ***** ***** *
            // Food Food_item = null;




            if (TempData["ValidError_NameExist"] != null) // 1.validasyon kontrolü 
            {
                fDTO = HttpContext.Session.GetObject<FoodDTO>("manipulatedData_NameExist");  // ***** ***** ***** *


                // List<object> ucj2 = new List<object>();                             

                result_2 = HttpContext.Session.GetObject<UserFoodJunctionDTO>("manipulatedData");  // ***** ***** ***** *
                ufjDTO.Add(result_2);
                

                ModelState.AddModelError("FoodDTO.Food_Name", $"{TempData["existinPool"]}" + " " + " Yemek listenizde mevcut ya da Havuz listesinde bulunmaktadır. Listenizde bulunmamaktaysa havuz listesinden kendi listenize de ekleyebilirsiniz.");
                
                TempData["ValidError_NameExist"] = null; // "FoodDTO.Food_Name", $"{TempData["existinPool"]}"   ***** ***** ***** *
                TempData["existinPool"] = null;

                //HttpContext.Session.SetObject("will_be_deleted_UserFoodJuncData", ufjDTO[0]);
                //HttpContext.Session.SetObject("will_be_deleted_FoodData", fDTO);

            }
            else if (TempData["ValidError_General"] != null) // 2.validasyon kontrolü 
            {                              

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
                    if (ufjDTO[0].Food_Description.Length >= 256)
                    {
                        ModelState.AddModelError("UserFoodJunctionDTO.Food_Description", "Açıklama 256 karakterden fazla olmamalı.");
                    }

                    TempData["ValidError_Description"] = null;
                }    
                
                             
                result_2 = HttpContext.Session.GetObject<UserFoodJunctionDTO>("manipulatedData_Status");
                ufjDTO.Add(result_2);

                fDTO = HttpContext.Session.GetObject<FoodDTO>("manipulatedData_Name");

                //HttpContext.Session.SetObject("willbedeletedUserFoodJuncData", ufjDTO[0]);
                //HttpContext.Session.SetObject("willbedeletedFoodData", fDTO);

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

            HttpContext.Session.SetObject("will_be_deleted_UserFoodJuncData", ufjDTO[0]);
            HttpContext.Session.SetObject("will_be_deleted_FoodData", fDTO);

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
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(_FoodPicture.FileName); // path oluşturma

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
                            HttpContext.Session.SetObject("manipulatedData_NameExist", fvm_post.FoodDTO);

                            TempData["ValidError_NameExist"] = "valid";
                            TempData["JavascriptToRun"] = "valid";

                            HttpContext.Session.SetObject("manipulatedData_fd", fvm_post.FoodDTO);
                            HttpContext.Session.SetObject("manipulatedData_ufdj", fvm_post.UserFoodJunctionDTO);



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
                        // Yeni bir Yemek iSMİ
                        if (old_fd.Food_Name != fd_update.Food_Name)
                        {

                            // Yeni Yemek eğer havuzda zaten varsa 
                            if (await _ifm.Any(x => x.Food_Name == fd_update.Food_Name))
                            {
                                // eski veri tekrardan set edildi...
                                HttpContext.Session.SetObject("manipulatedData_NameExist", old_fd);

                                TempData["existinPool"] = fvm_post.FoodDTO.Food_Name;

                                TempData["ValidError_NameExist"] = "valid";
                                TempData["JavascriptToRun"] = "valid";

                                old_ufj.Food_Status = fvm_post.UserFoodJunctionDTO.Food_Status;



                                HttpContext.Session.SetObject("manipulatedData_Status", old_ufj);

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
                                ufj.FoodID = fd_update.ID;
                                ufj.AppUser = CurrentUser; // IdentityUser'ın Id'si de tabloya eklnemiş oldu..

                                await _iufjm.AddAsync(ufj);

                                // builder.HasKey(x => new { x.AccessibleID, x.FoodID }); sayesinde 
                                // _iucjm.Update_OldCategory_with_NewOne(CurrentUser.AccessibleID, old_categoryID, ucj); // await yok // IdentityUser'dan gelen Id (Guid tipli)

                                // eski usercategory pasife alınacak...
                                // _iucjm.Delete(old_ufj);


                                //  OLD food become passive FOR THİS USER
                                UserFoodJunction passive_UserFoodJunction = old_ufj.Adapt<UserFoodJunction>();

                                passive_UserFoodJunction.DataStatus = DataStatus.Deleted;
                                passive_UserFoodJunction.DeletedDate = DateTime.Now;
                                passive_UserFoodJunction.AccessibleID = CurrentUser.AccessibleID;
                                passive_UserFoodJunction.AppUser = CurrentUser;
                                passive_UserFoodJunction.Food_Status = ExistentStatus.Pasif;

                                _iufjm.Delete_OldFood_from_User(CurrentUser.AccessibleID, passive_UserFoodJunction);

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
                            TempData["messageFood"] = "Yemek güncellendi";

                            // else { değişiklik olmadı mesajı }

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

                    }
                    // *!*
                    else// update için olan validation error
                    {

                        //  ama girdiği isim havuzda varsa  ....
                        if (await _ifm.Any(x => x.Food_Name == fvm_post.FoodDTO.Food_Name))
                        {
                            TempData["foodinPool"] = $"{fvm_post.FoodDTO.Food_Name}" + ", yemek listenizde mevcut ya da Havuz listesinde bulunmaktadır. Listenizde bulunmamaktaysa havuz listesinden kendi listenize de ekleyebilirsiniz.";
                        }

                        if (fvm_post.UserFoodJunctionDTO.Food_Status == 0)
                        {
                            TempData["ValidError_Status"] = "valid";
                        }

                        if (String.IsNullOrEmpty(fvm_post.FoodDTO.Food_Name) || fvm_post.FoodDTO.Food_Name.Length >= 30)
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

                        TempData["ValidError_General"] = "valid";

                    }

                    if (fvm_post.FoodDTO != null)
                    {
                        HttpContext.Session.SetObject("manipulatedData_fd", fvm_post.FoodDTO);
                    }

                    if (fvm_post.UserFoodJunctionDTO != null)
                    {
                        HttpContext.Session.SetObject("manipulatedData_ufdj", fvm_post.UserFoodJunctionDTO);
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
                Guid new_userID = CurrentUser.Id;

                UserFoodJunction UserFoodJunction = new UserFoodJunction();

                UserFoodJunction.FoodID = fvm_post.FoodDTO.ID;
                UserFoodJunction.DataStatus = DataStatus.Deleted;
                UserFoodJunction.DeletedDate = DateTime.Now;
                UserFoodJunction.AccessibleID = CurrentUser.AccessibleID;
                UserFoodJunction.AppUser = CurrentUser;
                UserFoodJunction.Food_Status = ExistentStatus.Pasif;

                _iufjm.Delete_OldFood_from_User(CurrentUser.AccessibleID, UserFoodJunction);


                TempData["messageFood"] = "Yemek listenizden silindi";
                TempData["Deleted"] = null;

                return RedirectToAction("FoodList_forMember", new { onlyOnce = "1" });

            }


        }



    }
}
