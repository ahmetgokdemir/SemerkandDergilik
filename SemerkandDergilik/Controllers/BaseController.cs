using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.ENTITIES.Models;
using System.Xml.Linq;

namespace Semerkand_Dergilik.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<AppUser> userManager { get; }
        protected SignInManager<AppUser> signInManager { get; }
        protected RoleManager<AppRole> roleManager { get; } // admincontroller'da kullanılacak.. AppRole class'ı

        protected AppUser CurrentUser => userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
        //AppUser user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

        /*public AppUser CurrentUser   // property
        {
            get { return userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result; }   // get method
          
        }*/
        //AppUser CurrentUser = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

        public BaseController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager = null)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;

        }

        [Route("AddModelError")]
        public void AddModelError(IdentityResult result)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
        }
    }
}
