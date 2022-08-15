using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.ENTITIES.Models;
using Semerkand_Dergilik.Models;
using Semerkand_Dergilik.ViewModels;
using System.Diagnostics;

namespace Semerkand_Dergilik.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        /*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        public UserManager<AppUser> userManager { get; }

        public HomeController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            if (ModelState.IsValid) // startup kısmında validationlar AppUser için ayarlandı -backend taraflı- 
            {
                AppUser user = new AppUser();
                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;

                IdentityResult result = await userManager.CreateAsync(user, userViewModel.Password);

                if (result.Succeeded)
                {
                    //string confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                               
                    return RedirectToAction("LogIn");
                }
                else
                {
                    foreach (IdentityError item in result.Errors)
                    {
                        ModelState.AddModelError("",item.Description);
                    }
                }
            }

            return View(userViewModel); // aynı sayfaya yönlendirir..  AddModelError(result) kısmında hatalar eklenip kullanıcıya model(userViewModel) tekrar gönderilir..
            
        }

        public IActionResult LogIn()
        {
            return View();

        }


    }
}