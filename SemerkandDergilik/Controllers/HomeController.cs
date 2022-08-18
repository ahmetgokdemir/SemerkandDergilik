using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Common;
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
            // eğer önceden  giriş yapılmışsa o zaman tekrar Home/Index sayfasına gitmek yerine Member/Index sayfasına yönlendirsin
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Member");
            }

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
                    //AddModelError(result);
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
        public IActionResult ResetPassword(PasswordResetViewModel passwordResetViewModel)
        {

                // böyle bir kullanıcı var mı yok mu bakılmalı
                // AppUser user = await userManager.FindByEmailAsync(passwordResetViewModel.Email);
                 AppUser user = userManager.FindByEmailAsync(passwordResetViewModel.Email).Result;

                if (user != null)

                {
                    // create token
                    // AddDefaultTokenProviders sayesinde aşağıdaki kod çalışır..
                    string passwordResetToken = userManager.GeneratePasswordResetTokenAsync(user).Result;

                    // create link 
                    string passwordResetLink = Url.Action("ResetPasswordConfirm", "Home", new
                    {
                        // ResetPasswordConfirm action metot'da querystring olarak kullanılacak
                        userId = user.Id,
                        token = passwordResetToken,
                        email = user.Email
                    }, HttpContext.Request.Scheme);

                    //  link görünümü: www.....com/Home/ResetPasswordConfirm?userId=sdjfsjf&token=dfjkdjfdjf

                    //******Helper.PasswordReset.PasswordResetSendEmail(passwordResetLink, user.Email);

                ViewBag.email = passwordResetLink;
                  

                ViewBag.status = "success";
                    //TempData["durum"] = true.ToString();
                }
                else
                {
                    ModelState.AddModelError("", "Sistemde kayıtlı email adresi bulunamamıştır.");
                }
                return View(passwordResetViewModel);
            


            // StartUp.cs kısmında AddDefaultTokenProviders servisi eklenmeli..
        }

        // Url.Action("ResetPasswordConfirm", .. TempData'lar yukarıdaki koddan geliyor..
        public IActionResult ResetPasswordConfirm(string userId, string token, string email)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            TempData["email"] = email;

            return View();
        }


        [HttpPost] // PasswordResetViewModel, ResetPasswordConfirm.cshtml'den gelir.. PasswordResetViewModel'den Email property'si de geliyor bunu istemiyoruz bunun için Bind Attribute kullanılır..
        // İstenmeyen property'ler için diğer bir yöntem.. [Bind(exclusive..
        public async Task<IActionResult> ResetPasswordConfirm(PasswordResetViewModel passwordResetViewModel)
        {
            string userId;
            string token;
            AppUser user;

            if (TempData["userId"] == null || TempData["token"] == null)
            {
                user = userManager.FindByEmailAsync(passwordResetViewModel.Email).Result;
                //userId = user.Id.ToString();
                token = userManager.GeneratePasswordResetTokenAsync(user).Result;
            }
            else
            {
                userId = TempData["userId"].ToString();
                token = TempData["token"].ToString();
                // öncekilerinin aksine bu sefer email yerine userid ile kontrol edilecek böyle bir user mevcut mu
                user = await userManager.FindByIdAsync(userId);

            }      
             
            if (user != null)
            {
                //** ResetPasswordAsync: şifre sıfırlanacak
                IdentityResult result = await userManager.ResetPasswordAsync(user, token, passwordResetViewModel.PasswordNew);

                if (result.Succeeded)
                {
                    // SecurityStamp: veritabanındaki bir alan yeni bir SecurityStamp oluşturulacak (user ile ilgili bir bilgi değiştirildiği zaman yapılması gerekir)
                    // nedeni: cookie içerisinde stamp bilgisi var ve user şifresini değiştirince artık yeni şifre ile login olmalı bu yüzden cookie/stamp değiştirilmeli
                    await userManager.UpdateSecurityStampAsync(user);

                    ViewBag.status = "success"; // ResetPasswordConfirm.cshtml'de kullanılacak..
                }
                else
                {
                    //AddModelError(result);
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);                       
                        
                    }
                    // hata durumunda tempdata güncellenir..
                    TempData["email"] = passwordResetViewModel.Email;
                }
            }
            else
            {
                ModelState.AddModelError("", "hata meydana gelmiştir. Lütfen daha sonra tekrar deneyiniz.");
            }
           

            return View(passwordResetViewModel);
        }




    }
}