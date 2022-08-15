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
        public SignInManager<AppUser> signInManager { get; }


        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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

        // LogIn.cshtml sayfasından LoginViewModel post edilecek o yüzden parametre olarak LoginViewModel aldı..
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel userlogin)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(userlogin.Email); // böyle Kullanıcı mevcut mu bunun kontrolü

                if (user != null)
                {

                    await signInManager.SignOutAsync(); // login işleminden önce çıkış yapılıdı amaç sistemdeki eski cookie'i silmek..

                    // PasswordSignInAsync ile Login işlemi gerçekleştirilir..
                    // opts.ExpireTimeSpan = System.TimeSpan.FromDays(60); (StartUp.cs'de) bunu aktif hale getirmek için userlogin.RememberMe true olması gerek
                    // lockoutFailure: false lockoutFailure özelliği ile kullanıcı başarısız girişlerde kullanıcıyı kilitleyip kilitlememe durumu..
                    // SignInResult ile hata verir bir Identity'den gelen SignInResult bir de ASPNET.Core.MVC den gelen var bu iki durum çakışıyor bunu engellemek için başına Microsoft.AspNetCore.Identity namespace'i eklendi..
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, userlogin.Password, false, false);

                    if (result.Succeeded)
                    {
                        //await userManager.ResetAccessFailedCountAsync(user);

                        if (TempData["ReturnUrl"] != null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }

                        return RedirectToAction("Index", "Member"); // başarılı ise Member sayfasına yönlendirilir.. (sadece üyeler görebilir)

                    }
                    else
                    {

                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bu email adresine kayıtlı kullanıcı bulunamamıştır.");
                }
            }

            return View(userlogin); // ModelState başarısızsa geri döner aynı sayfaya
        }



    }
}