using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Identity_Models;
using Project.ENTITIES.Models;
using Technosoft_Project.CommonTools;
using Technosoft_Project.ViewModels;
using Technosoft_Project.VMClasses;

namespace Technosoft_Project.Controllers
{

    [Authorize]
    [Authorize(Policy ="Confirmed_Member_Policy")]
    public class MenuController : BaseController
    {
        [Route("MenuIndex")]
        public IActionResult Index()
        {
            return View();
        }

        readonly IMenuManager _imn;
        readonly IMenuDetailManager _imdm;
        
        public MenuController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMenuManager imn, IMenuDetailManager imdm) : base(userManager, null, roleManager)
        {
            _imn = imn;
            _imdm = imdm;  
        }


        [Route("MenuList_forMember")]
        public async Task<IActionResult> MenuList_forMember(string? JSpopupPage, string? onlyOnce)
        {
            if(HttpContext.Session.GetObject<string>("hold_new_valid_menu_name") != null)
            {
                HttpContext.Session.SetObject("hold_new_valid_menu_name", null);
            }

            TempData["onlyOnce"] = onlyOnce;

            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null;
            }

            // IEnumerable<Menu> MenuList = await _imn.GetActivesAsync(); CurrentUser
            IEnumerable<object> MenuList = await _imn.Get_ByUserID_Async(CurrentUser.Id);
            MenuVM mvm = new MenuVM { 
            
                MenuDTOs = MenuList.Adapt<IEnumerable<MenuDTO>>().ToList(),
                JavascriptToRun = JSpopupPage
            };

            return View("MenuListforMember", mvm);

        }









    }
}
