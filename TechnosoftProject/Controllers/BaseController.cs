using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.ENTITIES.Identity_Models;
using System.Xml.Linq;

namespace Technosoft_Project.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<AppUser> userManager { get; }
        protected SignInManager<AppUser> signInManager { get; }
        protected RoleManager<AppRole> roleManager { get; } // admincontroller'da kullanılacak.. AppRole class'ı

        protected AppUser CurrentUser => userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

        
        //protected AppUser CurrentUser => userManager.FindByEmailAsync(HttpContext.User.Claims.).Result
        protected bool IsAuthenticated => HttpContext.User.Identity.IsAuthenticated;
        private List<char> deneme => HttpContext.User.Identity.AuthenticationType.ToList();

        // private List<char> deneme => HttpContext.User..ToList();


        //AppUser user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

        /*public AppUser CurrentUser   // property
        {
            get { return userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result; }   // get method
          
        }*/
        //AppUser CurrentUser = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

        public BaseController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager = null)
        {
            // roleManager = null set edilmesinin nedeni Member ve Base controller'ın constructorında roleManager olmayacak ve olmadığı için oralar patlar bunu engellemek için roleManager null'a set edildi.. Diğer bir yöntem de Member ve Base'in constructorında null parametresi göndermek olabilir ama 1.yöntem daha kısa..
            // Ama Admin controller'ın constructor'ında signManager (Admin'de kullanılmayacak) null set edildi..
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
