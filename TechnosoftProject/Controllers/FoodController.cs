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

        [Route("CRUDFood")]
        [HttpPost]
        public async Task<IActionResult> CRUDFood(FoodVM fvm_post, IFormFile _FoodPicture)
        {

            UserFoodJunctionDTO old_ucj = null;
            FoodDTO old_cof = null;


            if (HttpContext.Session.GetObject<UserFoodJunctionDTO>("willbedeletedUserFoodJuncData") != null && HttpContext.Session.GetObject<FoodDTO>("willbedeletedFoodData") != null)
            {

                old_ucj = new UserFoodJunctionDTO();
                old_ucj = HttpContext.Session.GetObject<UserFoodJunctionDTO>("willbedeletedUserFoodJuncData");

                old_cof = new FoodDTO();
                old_cof = HttpContext.Session.GetObject<FoodDTO>("willbedeletedFoodData");

                HttpContext.Session.SetObject("willbedeletedUserFoodJuncData", null);
                HttpContext.Session.SetObject("willbedeletedFoodData", null);

            }

            if (TempData["Deleted"] == null)
            {

                //ModelState.Remove("FoodPicture");
                //ModelState.Remove("FoodDTOs");
                //ModelState.Remove("JavascriptToRun");
                ModelState.Remove("ExistentStatus");
                ModelState.Remove("_FoodPicture"); // IFormFile _FoodPicture İÇİN
                ModelState.Remove("FoodDTO.ExistentStatus");
                // ModelState.Remove("UserFoodJunctionDTO.CategoryName_of_Foods");

                // DÜZENLENMESİ GEREKİYOR  --> FoodDTO.cs 
                ModelState.Remove("UserFoodJunctionDTO.Food_Name");
                ModelState.Remove("FoodDTO.Food_Name");
                ModelState.Remove("FoodDTO.FoodPrice");


                if (ModelState.IsValid)
                {
                    Food ftg_add = null;
                    Food ftg_update = null;
                    short old_categoryID = 0;

                    if (fvm_post.UserFoodJunctionDTO.FoodID == 0)
                    {
                        ftg_add = fvm_post.FoodDTO.Adapt<Food>();

                    }
                    else
                    {
                        old_categoryID = fvm_post.FoodDTO.ID;
                        ftg_update = new Food();
                        ftg_update.Food_Name = fvm_post.FoodDTO.Food_Name;
                    }

                    UserFoodJunction ucj = fvm_post.UserFoodJunctionDTO.Adapt<UserFoodJunction>();

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

                            ucj.Food_Picture = "/FoodPicture/" + fileName;

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
                        if (await _ifm.Any(x => x.Food_Name == ftg_add.Food_Name))
                        {
                            HttpContext.Session.SetObject("manipulatedData_NameExist", fvm_post.FoodDTO);

                            TempData["ValidError_NameExist"] = "valid";
                            TempData["JavascriptToRun"] = "valid";

                            HttpContext.Session.SetObject("manipulatedData_fd", fvm_post.FoodDTO);
                            HttpContext.Session.SetObject("manipulatedData_ufdj", fvm_post.UserFoodJunctionDTO);



                            TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";


                            return RedirectToAction("FoodList_forMember", new { JSpopupPage = TempData["JSpopupPage"].ToString(), onlyOnce = "1" });


                        }

                        await _ifm.AddAsync(ftg_add);

                        ucj.AccessibleID = CurrentUser.AccessibleID;
                        ucj.FoodID = ftg_add.ID;

                        // short control = CurrentUser.ID; olmadı // AppUser'dan gelen Id olmuyor (short tipli), ignore edilmişti zaten
                        // Guid control2 = CurrentUser.Id;
                        ucj.AppUser = CurrentUser; // IdentityUser'ın Id'si de tabloya eklnemiş oldu..
                        //ucj.AppUser.Id = CurrentUser.Id;

                        await _iufjm.AddAsync(ucj);
                        TempData["messageFood"] = "Yemek eklendi";

                        return RedirectToAction("FoodList_forMember", new { onlyOnce = "1" });

                    }// !*!
                    //else // update 
                    //{
                    //    // Yeni bir Yemek
                    //    if (old_cof.CategoryName_of_Foods != ftg_update.CategoryName_of_Foods)
                    //    {

                    //        // Yeni Yemek eğer havuzda zaten varsa 
                    //        if (await _icm.Any(x => x.CategoryName_of_Foods == ftg_update.CategoryName_of_Foods))
                    //        {
                    //            // eski veri tekrardan set edildi...
                    //            HttpContext.Session.SetObject("manipulatedData_NameExist", old_cof);

                    //            TempData["existinPool"] = fvm_post.FoodDTO.CategoryName_of_Foods;

                    //            TempData["ValidError_NameExist"] = "valid";
                    //            TempData["JavascriptToRun"] = "valid";

                    //            old_ucj.Food_Status = fvm_post.UserFoodJunctionDTO.Food_Status;



                    //            HttpContext.Session.SetObject("manipulatedData_Status", old_ucj);

                    //            // ,{CurrentUser.Id}
                    //            TempData["JSpopupPage"] = $"ShowErrorUpdateOperationPopup({fvm_post.FoodDTO.ID})"; // diğer paramaetre de eklenecek

                    //            return RedirectToAction("FoodList_forMember", new { JSpopupPage = TempData["JSpopupPage"].ToString(), onlyOnce = "1" });
                    //        }

                    //        // Yeni Yemek eğer havuzda da yoksa 
                    //        else
                    //        {
                    //            // yeni bir Yemek olarak eklenecek ... güncellenmeyecek
                    //            await _icm.AddAsync(ftg_update);
                    //            // _icm.Update(ctg);

                    //            // usercategory olarak da eklenecek...
                    //            ucj.AccessibleID = CurrentUser.AccessibleID;
                    //            // ucj.AppUser.Id = CurrentUser.Id; erişilemiyor
                    //            ucj.FoodID = ftg_update.ID;
                    //            ucj.AppUser = CurrentUser; // IdentityUser'ın Id'si de tabloya eklnemiş oldu..

                    //            await _iucjm.AddAsync(ucj);

                    //            // builder.HasKey(x => new { x.AccessibleID, x.FoodID }); sayesinde 
                    //            // _iucjm.Update_OldCategory_with_NewOne(CurrentUser.AccessibleID, old_categoryID, ucj); // await yok // IdentityUser'dan gelen Id (Guid tipli)

                    //            // eski usercategory pasife alınacak...
                    //            // _iucjm.Delete(old_ucj);



                    //            UserFoodJunction UserFoodJunction = old_ucj.Adapt<UserFoodJunction>();

                    //            UserFoodJunction.DataStatus = DataStatus.Deleted;
                    //            UserFoodJunction.DeletedDate = DateTime.Now;
                    //            UserFoodJunction.AccessibleID = CurrentUser.AccessibleID;
                    //            UserFoodJunction.AppUser = CurrentUser;
                    //            UserFoodJunction.Food_Status = ExistentStatus.Pasif;

                    //            _iucjm.Delete_OldCategory_from_User(CurrentUser.AccessibleID, old_categoryID, UserFoodJunction);

                    //            TempData["messageFood"] = "Yemek güncellendi";

                    //            return RedirectToAction("FoodList_forMember", new { onlyOnce = "1" });

                    //        }


                    //    }

                    //    else // eski Yemek old_cof.CategoryName_of_Foods == ftg_update.CategoryName_of_Foods
                    //    {
                    //        //old_ucj.AppUser.Id = CurrentUser.Id;

                    //        //if (await _icm.Any(x => x.CategoryName_of_Foods == ftg_update.CategoryName_of_Foods) && await _iucjm.Any(x => x. == ftg_update.CategoryName_of_Foods))
                    //        //{
                    //        //    HttpContext.Session.SetObject("manipulatedData_NameExist", fvm_post.FoodDTO);

                    //        //    TempData["ValidError_NameExist"] = "valid";
                    //        //    TempData["JavascriptToRun"] = "valid";

                    //        //    HttpContext.Session.SetObject("manipulatedData_Status", fvm_post.UserFoodJunctionDTO);

                    //        //    // ,{CurrentUser.Id}
                    //        //    TempData["JSpopupPage"] = $"ShowErrorUpdateOperationPopup({fvm_post.FoodDTO.ID})"; // diğer paramaetre de eklenecek

                    //        //    return RedirectToAction("FoodList", new { JSpopupPage = TempData["JSpopupPage"].ToString() });
                    //        //}


                    //        // ucj tablosunda değişiklik var
                    //        // if (old_ucj.Food_Status != fvm_post.UserFoodJunctionDTO.Food_Status) {

                    //        // _iucjm.Update(ucj);
                    //        // old_categoryID = fvm_post.FoodDTO.ID;

                    //        ucj.DataStatus = DataStatus.Updated;
                    //        ucj.ModifiedDate = DateTime.Now;
                    //        ucj.AccessibleID = CurrentUser.AccessibleID;
                    //        ucj.AppUser = CurrentUser;

                    //        ucj.Food = fvm_post.FoodDTO.Adapt<Food>();
                    //        ucj.FoodID = fvm_post.FoodDTO.ID;
                    //        //  ucj.Food_Picture = "/FoodPicture/" + fileName;





                    //        _iucjm.Update_UserCategoryJuncTable(CurrentUser.AccessibleID, fvm_post.FoodDTO.ID, ucj);
                    //        TempData["messageFood"] = "Yemek güncellendi";

                    //        // else { değişiklik olmadı mesajı }

                    //        return RedirectToAction("FoodList_forMember", new { onlyOnce = "1" });

                    //    }



                    //}


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

                        if (fvm_post.UserFoodJunctionDTO.Food_Price == null || fvm_post.UserFoodJunctionDTO.Food_Price == 0 || fvm_post.UserFoodJunctionDTO.Food_Price <= 1000)
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
                    //else// update için olan validation error
                    //{
                    //    if (fvm_post.UserFoodJunctionDTO.Food_Status == 0)
                    //    {
                    //        TempData["ValidError_Status"] = "valid";.....                    


                    FoodVM fVM = new FoodVM();

                    TempData["JavascriptToRun"] = "valid";

                    // !*!
                    if (fvm_post.FoodDTO.ID != 0) //update
                    {
                        // ,{CurrentUser.Id}
                        fVM.JavascriptToRun = $"ShowErrorUpdateOperationPopup({fvm_post.FoodDTO.ID})";
                        return RedirectToAction("FoodList_forMember", new { JSpopupPage = fVM.JavascriptToRun, onlyOnce = "1" });

                    }
                    else // add // (pvm_post.FoodDTO.ID == 0) 
                    {
                        // pvm.JavascriptToRun = $"ShowErrorPopup( {pvm_post.FoodDTO} )";

                        // pvm.JavascriptToRun = $"ShowErrorInsertOperationPopup()";

                        TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";
                        return RedirectToAction("FoodList_forMember", new { JSpopupPage = TempData["JSpopupPage"].ToString(), onlyOnce = "1" });

                    }


                } //  validation olmayan kod kısmının sonu

            }
            // *!* DELETE 
            else
            {
            //    IEnumerable<object> ucj = null;
            //    Guid new_userID = CurrentUser.Id;
            //    List<UserFoodJunction> ucj_Delete = new List<UserFoodJunction>();


            //    ucj = await _iufjm.Get_ByUserID_with_CategoryID_Async(new_userID, fvm_post.FoodDTO.ID);
            //    ucj_Delete = ucj.Adapt<IEnumerable<UserFoodJunction>>().ToList();


            //    ucj = await _iufjm.Get_ByUserID_with_CategoryID_Async(new_userID, fvm_post.FoodDTO.ID);
            //    ucj_Delete = ucj.Adapt<IEnumerable<UserFoodJunction>>().ToList();

            //    UserFoodJunction UserFoodJunction = new UserFoodJunction();

            //    UserFoodJunction.FoodID = fvm_post.FoodDTO.ID;
            //    UserFoodJunction.DataStatus = DataStatus.Deleted;
            //    UserFoodJunction.DeletedDate = DateTime.Now;
            //    UserFoodJunction.AccessibleID = CurrentUser.AccessibleID;
            //    UserFoodJunction.AppUser = CurrentUser;
            //    UserFoodJunction.Food_Status = ExistentStatus.Pasif;

            //    _iufjm.Delete_OldCategory_from_User(CurrentUser.AccessibleID, fvm_post.FoodDTO.ID, UserFoodJunction);

            //    // _iucjm.Delete(ucj_Delete[0]);
            //    // _iucjm.Delete(ucj_Delete.FirstOrDefault());

            //    TempData["messageFood"] = "Yemek listenizden silindi";
            //    TempData["Deleted"] = null;

                //return RedirectToAction("FoodList_forMember", new { onlyOnce = "1" });

            }


            // sonra kaldır 
            return RedirectToAction("FoodList_forMember", new { onlyOnce = "1" });

        }



    }
}
