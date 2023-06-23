using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Models;
using System.Data;
using Technosoft_Project.CommonTools;
using Technosoft_Project.Enums;
using Technosoft_Project.ViewModels;
using Technosoft_Project.VMClasses;

namespace Technosoft_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Menu")]
    [Authorize(Roles = "Admin")] // case sensitive  
    public class MenuController : Controller
    {
        [Route("MenuIndex")]
        public IActionResult Index()
        {
            return View();
        }

        readonly IMenuManager _imm;
        readonly IMenuDetailManager _imdm;

        public MenuController(IMenuManager imm, IMenuDetailManager imdm) // services.AddRepManServices(); 
        {
            _imm = imm;
            _imdm = imdm;
        }

        [Route("MenuDetailList")]
        public async Task<IActionResult> MenuDetailList(int id, string? JSpopupPage)
        {

            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null; // pop-up sıfırlanır yoksa sayfayı reflesleyince geliyor
            }

            int Menu_ID = id;

            IEnumerable<object> FoodsofMenu = await _imdm.Get_FoodsofMenu_Async(Menu_ID);

            IEnumerable<object> CategoriessofMenu = await _imdm.Get_CategoriesofMenu_Async(Menu_ID);

            MenuDetailVM mvm = new MenuDetailVM
            {
                MenuDetailDTOs = FoodsofMenu.Adapt<IEnumerable<MenuDetailDTO>>().ToList(),
                //JavascriptToRun = JSpopupPage
                Category_of_FoodDTOs = CategoriessofMenu.Adapt<IEnumerable<Category_of_FoodDTO>>().ToList() //  List<Category_of_FoodDTO> 
            };
 


            return View(mvm);
        }


        [Route("MenuList")]
        public async Task<IActionResult> MenuList(string? JSpopupPage)
        {

            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null; // pop-up sıfırlanır yoksa sayfayı reflesleyince geliyor
            }

            IEnumerable<Menu> MenuList = await _imm.GetActivesAsync();

            MenuVM mvm = new MenuVM
            {
                MenuDTOs = MenuList.Adapt<IEnumerable<MenuDTO>>().ToList(),
                JavascriptToRun = JSpopupPage

            };
            
            return View(mvm);  
        }

        [Route("AddMenuAjax")]
        public PartialViewResult AddMenuAjax()
        {
            // ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status))); => yerine                                                                 asp-items="Html.GetEnumSelectList<Technosoft_Project.Enums.Status>()" kullanıldı..

            var result = new MenuDTO();
            MenuDTO mDTO = new MenuDTO();

            // HttpContext.Session.SetObject("manipulatedData", pvm_post.MenuDTO);
            if (TempData["HttpContext"] != null)
            {
                //mDTO = new MenuDTO();
                result = HttpContext.Session.GetObject<MenuDTO>("manipulatedData");
                mDTO = result;

            }

            MenuVM mVM = new MenuVM
            {
                MenuDTO = mDTO
            };

            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(mVM.MenuDTO.Menu_Name))
                {
                    ModelState.AddModelError("MenuDTO.MenuName", "Menü adı giriniz.");
                }

                HttpContext.Session.SetObject("manipulatedData", null);
            }

            Thread.Sleep(500); // pop-up sayfasını tekrar açmayı tetikleyince bazen gelmiyor o yüzden bu kod eklendi..

            return PartialView("_CrudMenuPartial", mVM);
        }

        [Route("UpdateMenuAjax")]
        public async Task<PartialViewResult> UpdateMenuAjax(int id)
        {
            // Menu Menu_item = await _icm.GetByIdAsync(id);
            // MenuDTO cDTO = Menu_item.Adapt<MenuDTO>();

            // ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status))); => yerine                                                                 asp-items="Html.GetEnumSelectList<Technosoft_Project.Enums.Status>()" kullanıldı..


            MenuDTO cDTO = new MenuDTO();

            var result = new MenuDTO();

            if (TempData["HttpContext"] != null)
            {
                result = HttpContext.Session.GetObject<MenuDTO>("manipulatedData");
                cDTO = result;

                // HttpContext.Session.SetObject("manipulatedData", null);
            }
            else
            {
                Menu Menu_item = await _imm.GetByIdAsync(id);
                cDTO = Menu_item.Adapt<MenuDTO>();
                // cdto = Food_item.Adapt<MenuDTO>();
            }


            MenuVM cVM = new MenuVM
            {
                MenuDTO = cDTO
            };

            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(cVM.MenuDTO.Menu_Name))
                {
                    ModelState.AddModelError("MenuDTO.MenuName", "Menü adı giriniz.");
                }

                HttpContext.Session.SetObject("manipulatedData", null);
            }

            Thread.Sleep(500);

            return PartialView("_CrudMenuPartial", cVM);
        }


        [Route("DeleteMenuAjax")]
        public async Task<PartialViewResult> DeleteMenuAjax(int id)
        {
            Menu Menu_item = await _imm.GetByIdAsync(id);
            MenuDTO cDTO = Menu_item.Adapt<MenuDTO>();

            //ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));
            ViewBag.MenuNameDelete = cDTO.Menu_Name;

            ViewBag.CRUD = "delete_operation";

            MenuVM cVM = new MenuVM
            {
                MenuDTO = cDTO
            };

            return PartialView("_CrudMenuPartial", cVM);
        }

        [Route("CRUDMenu")]
        [HttpPost]
        public async Task<IActionResult> CRUDMenu(MenuVM mvm_post)
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
                ModelState.Remove("MenuDTOs");
                ModelState.Remove("JavascriptToRun");
                ModelState.Remove("MenuDTO.FoodName");
                

                if (ModelState.IsValid)
                {
                    Menu ctg = mvm_post.MenuDTO.Adapt<Menu>();

                    ctg.Status = (int)mvm_post.MenuDTO.Status;



                    //////
                    ///


                    if (ctg.ID == 0)
                    {
                        await _imm.AddAsync(ctg);
                        TempData["messageMenu"] = "Kategori eklendi";
                    }
                    else
                    {
                        _imm.Update(ctg);
                        // yapılacak ödev:  Menu pasife çekilirse Foodları da pasife çekilsin!!! Update metodu içerisinde yapılabilir... ekstra metoda gerek yok

                        /*
                         * 
                         Fonksiyon, belirli bir görevi gerçekleştirmek için bir dizi talimat veya prosedürdür. 

                        Metot ise bir NSENEYLE ilişkili bir dizi talimattır. 

                        Bir fonksiyon herhangi bir nesneye ihtiyaç duymaz ve bağımsızdır, 
                        metot ise herhangi bir nesneyle bağlantılı bir işlevdir. 

                        Metotlar, OOP (Nesne Yönelimli Programlama) ile ilgili bir kavram  --> _icm nesnesi İLE Update Metodu gibi

                         Bu yuzden methodlar classlar icinde define edilir ve obje varyasyonlari ile kullanilir. Functionlarda class icinde define edilir ama o classa ait seyler icermez, objeye dependent olmaz. 

                        Yani soyle bir sey dusunulebilir, bir dog classi, havlamak diye bir METHOD icerir, cunku sadece kopekler havlar, bu yuzden kopek objesine ihtiyac vardir.

Fakat ayni zamanda bir human classi olsun, diyelim ki beslenmek diye bir FONKSIYON yazilacak. Cunku sart su, beslenmeyi kopek de insan da yapabilir, e bu yuzden particular bir class ihtiyaci dogurmaz. 


                         */

                        TempData["messageMenu"] = "Kategori güncellendi";

                    }

                    return RedirectToAction("MenuList");
                }

            }
            else
            {
                _imm.Delete(await _imm.GetByIdAsync(mvm_post.MenuDTO.ID));

                // Menu ctg = cdto.Adapt<Menu>();

                // _icm.Delete(ctg);
                TempData["messageMenu"] = "Kategori silindi";

                TempData["Deleted"] = null;

                return RedirectToAction("MenuList");
            }

            // TempData["mesaj"] = "Kategori adı ve statü giriniz..";
            // ModelState.AddModelError("", "Ürün adı ve statü giriniz..");

            MenuVM cVM = new MenuVM();
            HttpContext.Session.SetObject("manipulatedData", mvm_post.MenuDTO);

            TempData["JavascriptToRun"] = "valid";
            TempData["HttpContext"] = "valid";

            if (mvm_post.MenuDTO.ID != 0) //update
            {
                cVM.JavascriptToRun = $"ShowErrorUpdateOperationPopup({mvm_post.MenuDTO.ID})";
                return RedirectToAction("MenuList", new { JSpopupPage = cVM.JavascriptToRun });

            }
            else // add // (pvm_post.FoodDTO.ID == 0) çevir...
            {
                // pvm.JavascriptToRun = $"ShowErrorPopup( {pvm_post.FoodDTO} )";

                // pvm.JavascriptToRun = $"ShowErrorInsertOperationPopup()";

                TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";
                return RedirectToAction("MenuList", new { JSpopupPage = TempData["JSpopupPage"].ToString() });
            }


        }


    }
}
