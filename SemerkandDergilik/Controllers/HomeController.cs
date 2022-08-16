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

        public IActionResult LogIn(string ReturnUrl)
        {
            // doğrudan member sayfasına giderse url'de returnurl çıkar ...
            TempData["ReturnUrl"] = ReturnUrl;

            return View();
        }

        // LogIn.cshtml sayfasından LoginViewModel post edilecek o yüzden parametre olarak LoginViewModel aldı..
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel userlogin)
        {
            if (ModelState.IsValid)
            {
                // kısmında aslında parolayı kontrol etmiyor sadece kullanıcı adresini kontrol ediyor  
                AppUser user = await userManager.FindByEmailAsync(userlogin.Email); // böyle Kullanıcı mevcut mu bunun kontrolü

               

                if (user != null)
                {
                    // Kullanıcı Kilitli mi değil mi (veritabanında LockoutEnd tarihi kontrol edilir)
                    if (await userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "Hesabınız bir süreliğine kilitlenmiştir. Lütfen daha sonra tekrar deneyiniz.");

                        return View(userlogin);
                    }



                    await signInManager.SignOutAsync(); // login işleminden önce çıkış yapılıdı amaç sistemdeki eski cookie'i silmek..

                    // PasswordSignInAsync ile Login işlemi gerçekleştirilir..
                    // opts.ExpireTimeSpan = System.TimeSpan.FromDays(60); (StartUp.cs'de) bunu aktif hale getirmek için userlogin.RememberMe true olması gerek
                    // lockoutFailure: false lockoutFailure özelliği ile kullanıcı başarısız girişlerde kullanıcıyı kilitleyip kilitlememe durumu.. veritabanında LockoutEnabled kısmı etkiler..
                    // SignInResult ile hata verir bir Identity'den gelen SignInResult bir de ASPNET.Core.MVC den gelen var bu iki durum çakışıyor bunu engellemek için başına Microsoft.AspNetCore.Identity namespace'i eklendi..
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, userlogin.Password, userlogin.RememberMe, false);

                    // şifre doğru mu değil mi => SignInResult result 
                    if (result.Succeeded)
                    {
                        // doğru giriş durumunda veritabanındaki AccessFailedCount sıfırlanır..
                        await userManager.ResetAccessFailedCountAsync(user);

                        if (TempData["ReturnUrl"] != null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }

                        return RedirectToAction("Index", "Member"); // başarılı ise Member sayfasına yönlendirilir.. (sadece üyeler görebilir)

                    }
                    else // hatalı giriş durumunda
                    {
                        // hatalı giriş durumunda veritabanındaki AccessFailedCount kısmı bir artar..
                        await userManager.AccessFailedAsync(user);

                        // başarısız giriş sayısını çekmek.. veritabanındaki AccessFailedCount kısmına denk gelir
                        int fail = await userManager.GetAccessFailedCountAsync(user);
                        ModelState.AddModelError("", $" {fail} kez başarısız giriş.");
                        if (fail == 3)
                        {
                            // ne kadar sürelik kitleme süresi belirlenir..veritabanında lockoutend kısmı..
                            await userManager.SetLockoutEndDateAsync(user, new System.DateTimeOffset(DateTime.Now.AddMinutes(20)));

                            ModelState.AddModelError("", "Hesabınız 3 başarısız girişten dolayı 20 dakika süreyle kitlenmiştir. Lütfen daha sonra tekrar deneyiniz.");
                        }
                        else // başarısız girişimi 3 değilse
                        {
                            ModelState.AddModelError("", "Email adresiniz veya şifreniz yanlış."); // email hatası SignUp.cshtml'de sadece summary kısmında validation error verir..
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bu email adresine kayıtlı kullanıcı bulunamamıştır.");
                    // bu else kısmında aslında parolayı kontrol etmiyor sadece kullanıcı adresini kontrol ediyor bak:                            await userManager.FindByEmailAsync(userlogin.Email);
                }
            }

            return View(userlogin); // ModelState başarısızsa kullanıcının bilgileri ile (userlogin) geri döner aynı sayfaya

 

        }

        // Şifremi unuttum mekanizması
        public IActionResult ResetPassword()
        {
            // TempData["durum"] = null;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(PasswordResetViewModel passwordResetViewModel)
        {
            if (TempData["durum"] == null)
            {
                // böyle bir kullanıcı var mı yok mu bakılmalı
                AppUser user = await userManager.FindByEmailAsync(passwordResetViewModel.Email);
                // AppUser user = await userManager.FindByEmailAsync(userlogin.Email).Result;

                if (user != null)

                {
                    // create token
                    string passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(user);

                    /*string passwordResetLink = Url.Action("ResetPasswordConfirm", "Home", new
                    {
                        userId = user.Id,
                        token = passwordResetToken
                    }, HttpContext.Request.Scheme);

                    //  www.bıdıbıdı.com/Home/ResetPasswordConfirm?userId=sdjfsjf&token=dfjkdjfdjf

                    Helper.PasswordReset.PasswordResetSendEmail(passwordResetLink, user.Email);

                    ViewBag.status = "success";
                    TempData["durum"] = true.ToString();*/
                }
                else
                {
                    ModelState.AddModelError("", "Sistemde kayıtlı email adresi bulunamamıştır.");
                }
                return View(passwordResetViewModel);
            }
            else
            {
                return RedirectToAction("ResetPassword");
            }
        }



    }
}