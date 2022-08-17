using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.ENTITIES.Models;
using Mapster;
using Semerkand_Dergilik.ViewModels;

namespace Semerkand_Dergilik.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        public UserManager<AppUser> userManager { get; }
        public SignInManager<AppUser> signInManager { get; }


        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            // HttpContext.User.Identity.Name, veritabanındaki UserName'dir
            AppUser user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;            

            // fazla property'si olan class'lar için kısayol automap.. (User class'ın daki bilgileri UserViewModel'e basar)
            // mapster kütüphanesi indirilsin (dependencies --> manage nuget)

            UserViewModel userViewModel = user.Adapt<UserViewModel>(); // automap

            return View(userViewModel);
        }
    }
}
