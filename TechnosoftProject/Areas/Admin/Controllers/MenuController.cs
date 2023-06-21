using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Models;
using System.Data;
using Technosoft_Project.CommonTools;
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

        public MenuController(IMenuManager imm) // services.AddRepManServices(); 
        {
            _imm = imm;
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

            // HttpContext.Session.SetObject("manipulatedData", pvm_post.Category_of_FoodDTO);
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
                    ModelState.AddModelError("Category_of_FoodDTO.Category_of_FoodName", "Kategori adı giriniz.");
                }

                HttpContext.Session.SetObject("manipulatedData", null);
            }

            Thread.Sleep(500); // pop-up sayfasını tekrar açmayı tetikleyince bazen gelmiyor o yüzden bu kod eklendi..

            return PartialView("_CrudMenuPartial", mVM);
        }




    }
}
