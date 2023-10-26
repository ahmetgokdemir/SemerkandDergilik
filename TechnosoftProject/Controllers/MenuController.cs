//using Mapster;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Project.BLL.ManagerServices.Abstracts;
//using Project.ENTITIES.Identity_Models;
//using Project.ENTITIES.Models;
//using Technosoft_Project.CommonTools;
//using Technosoft_Project.ViewModels;
//using Technosoft_Project.VMClasses;

//namespace Technosoft_Project.Controllers
//{

//    [Authorize]
//    [Authorize(Policy ="Confirmed_Member_Policy")]
//    [Route("Menu")]
//    public class MenuController : BaseController
//    {
//        [Route("MenuIndex")]
//        public IActionResult Index()
//        {
//            return View();
//        }

//        readonly IMenuManager _imn;
//        readonly IMenuDetailManager _imdm;
        
//        public MenuController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMenuManager imn, IMenuDetailManager imdm) : base(userManager, null, roleManager)
//        {
//            _imn = imn;
//            _imdm = imdm;  
//        }


//        [Route("MenuList_forMember")]
//        public async Task<IActionResult> MenuList_forMember(string? JSpopupPage, string? onlyOnce)
//        {
//            if(HttpContext.Session.GetObject<string>("hold_new_valid_menu_name") != null)
//            {
//                HttpContext.Session.SetObject("hold_new_valid_menu_name", null);
//            }

//            TempData["onlyOnce"] = onlyOnce;

//            if (TempData["JavascriptToRun"] == null)
//            {
//                JSpopupPage = null;
//            }

//            // IEnumerable<Menu> MenuList = await _imn.GetActivesAsync(); CurrentUser
//            IEnumerable<object> MenuList = await _imn.Get_ByUserID_Async(CurrentUser.Id);
//            MenuVM mvm = new MenuVM { 
            
//                MenuDTOs = MenuList.Adapt<IEnumerable<MenuDTO>>().ToList(),
//                JavascriptToRun = JSpopupPage
//            };

//            return View("MenuListforMember", mvm);

//        }

//        [Route("AddMenuAjax")]
//        public PartialViewResult AddMenuAjax()
//        {
//            // ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status))); => yerine                                                                 asp-items="Html.GetEnumSelectList<Technosoft_Project.Enums.Status>()" kullanıldı..

//            var result = new MenuDTO();
//            MenuDTO mDTO = new MenuDTO();

//            // HttpContext.Session.SetObject("manipulatedData", pvm_post.MenuDTO);
//            if (TempData["HttpContext"] != null)
//            {
//                //mDTO = new MenuDTO();
//                result = HttpContext.Session.GetObject<MenuDTO>("manipulatedData");
//                mDTO = result;

//            }

//            MenuVM mVM = new MenuVM
//            {
//                MenuDTO = mDTO
//            };

//            if (TempData["HttpContext"] != null)
//            {
//                TempData["HttpContext"] = null;

//                if (string.IsNullOrEmpty(mVM.MenuDTO.Menu_Name))
//                {
//                    ModelState.AddModelError("MenuDTO.MenuName", "Menü adı giriniz.");
//                }

//                HttpContext.Session.SetObject("manipulatedData", null);
//            }

//            Thread.Sleep(500); // pop-up sayfasını tekrar açmayı tetikleyince bazen gelmiyor o yüzden bu kod eklendi..

//            return PartialView("_CrudMenuPartial", mVM);
//        }

//        [Route("CRUDMenu")]
//        [HttpPost]
//        public async Task<IActionResult> CRUDMenu(MenuVM mvm_post)
//        {

//            /*
//              var urlHelper = new UrlHelper(ControllerContext);
//              var url = urlHelper.Action("About", "Home");
//              var linkText = "Panelden yapılan değiliklik web e yansımıyor";
              
//              var hyperlink = string.Format("<a href=\"{0}\">{1}</a>", url, linkText);
              
//              var url2 = $"{Request.Scheme}://{Request.Host}/Home/About";
//            */



//            /* PasswordReset.cs'de SendGridClient --> Task Execute(string link, string emailAdress) kısmında yapılmış...*/


//            if (TempData["Deleted"] == null)
//            {
//                ModelState.Remove("MenuDTOs");
//                ModelState.Remove("JavascriptToRun");
//                ModelState.Remove("MenuDTO.FoodName");


//                if (ModelState.IsValid)
//                {
//                    Menu _menu = mvm_post.MenuDTO.Adapt<Menu>();

//                    _menu.Menu_Status = mvm_post.MenuDTO._ExistentStatus;

//                    //_menu.AppUserID = CurrentUser.AccessibleID;
//                    //_75921573

//                    //////
//                    ///


//                    if (_menu.ID == 0)
//                    {
//                        await _imn.AddAsync(_menu);
//                        TempData["messageMenu"] = "Kategori eklendi";
//                    }
//                    else
//                    {
//                        _imn.Update(_menu);
//                        // yapılacak ödev:  Menu pasife çekilirse Foodları da pasife çekilsin!!! Update metodu içerisinde yapılabilir... ekstra metoda gerek yok

//                        /*
//                         * 
//                         Fonksiyon, belirli bir görevi gerçekleştirmek için bir dizi talimat veya prosedürdür. 

//                        Metot ise bir NSENEYLE ilişkili bir dizi talimattır. 

//                        Bir fonksiyon herhangi bir nesneye ihtiyaç duymaz ve bağımsızdır, 
//                        metot ise herhangi bir nesneyle bağlantılı bir işlevdir. 

//                        Metotlar, OOP (Nesne Yönelimli Programlama) ile ilgili bir kavram  --> _icm nesnesi İLE Update Metodu gibi

//                         Bu yuzden methodlar classlar icinde define edilir ve obje varyasyonlari ile kullanilir. Functionlarda class icinde define edilir ama o classa ait seyler icermez, objeye dependent olmaz. 

//                        Yani soyle bir sey dusunulebilir, bir dog classi, havlamak diye bir METHOD icerir, cunku sadece kopekler havlar, bu yuzden kopek objesine ihtiyac vardir.

//Fakat ayni zamanda bir human classi olsun, diyelim ki beslenmek diye bir FONKSIYON yazilacak. Cunku sart su, beslenmeyi kopek de insan da yapabilir, e bu yuzden particular bir class ihtiyaci dogurmaz. 


//                         */

//                        TempData["messageMenu"] = "Kategori güncellendi";

//                    }

//                    return RedirectToAction("MenuList");
//                }

//            }
//            else
//            {
//                _imn.Delete(await _imn.GetByIdAsync((short)mvm_post.MenuDTO.ID));

//                // Menu _menu = cdto.Adapt<Menu>();

//                // _icm.Delete(_menu);
//                TempData["messageMenu"] = "Kategori silindi";

//                TempData["Deleted"] = null;

//                return RedirectToAction("MenuList");
//            }

//            // TempData["mesaj"] = "Kategori adı ve statü giriniz..";
//            // ModelState.AddModelError("", "Ürün adı ve statü giriniz..");

//            MenuVM cVM = new MenuVM();
//            HttpContext.Session.SetObject("manipulatedData", mvm_post.MenuDTO);

//            TempData["JavascriptToRun"] = "valid";
//            TempData["HttpContext"] = "valid";

//            if (mvm_post.MenuDTO.ID != 0) //update
//            {
//                cVM.JavascriptToRun = $"ShowErrorUpdateOperationPopup({mvm_post.MenuDTO.ID})";
//                return RedirectToAction("MenuList", new { JSpopupPage = cVM.JavascriptToRun });

//            }
//            else // add // (pvm_post.FoodDTO.ID == 0) çevir...
//            {
//                // pvm.JavascriptToRun = $"ShowErrorPopup( {pvm_post.FoodDTO} )";

//                // pvm.JavascriptToRun = $"ShowErrorInsertOperationPopup()";

//                TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";
//                return RedirectToAction("MenuList", new { JSpopupPage = TempData["JSpopupPage"].ToString() });
//            }


//        }







//    }
//}
