using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Models;
using Technosoft_Project.ViewModels;
using Technosoft_Project.VMClasses;

namespace Technosoft_Project.Controllers
{

    [Authorize]
    [Authorize(Policy ="Confirmed_Member_Policy")]
    public class MenuController : Controller
    {
        [Route("MenuIndex")]
        public IActionResult Index()
        {
            return View();
        }

        readonly IMenuManager _imn;
        readonly IMenuDetailManager _imdm;
        
        public MenuController(IMenuManager imn, IMenuDetailManager imdm, ICategoryofFoodManager icm,  IFoodManager ifm)
        {
            _imn = imn;
            _imdm = imdm;  
        }


        [Route("MenuList")]
        public async Task<IActionResult> MenuList(string? JSpopupPage)
        {
            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null;
            }

            IEnumerable<Menu> MenuList = await _imn.GetActivesAsync();

            MenuVM mvm = new MenuVM { 
            
                MenuDTOs = MenuList.Adapt<IEnumerable<MenuDTO>>().ToList(),
                JavascriptToRun = JSpopupPage
            };

            return View(mvm);


        }









    }
}
