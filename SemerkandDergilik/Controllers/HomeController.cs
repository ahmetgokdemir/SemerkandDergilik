using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using Project.ENTITIES.Identity_Models;
using Semerkand_Dergilik.Enums;
using Semerkand_Dergilik.Helper;
using Semerkand_Dergilik.Models;
using Semerkand_Dergilik.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace Semerkand_Dergilik.Controllers
{
    public class HomeController : BaseController
    {
        //private readonly ILogger<HomeController> _logger;

        /*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        /*
         * tekrar eden kodlar
        public UserManager<AppUser> userManager { get; }
        public SignInManager<AppUser> signInManager { get; }


        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        */

        private readonly EmailConfirmation _emailConfirmation; // addscope unutma program.cs
        private readonly PasswordReset _passwordReset;


        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, EmailConfirmation emailConfirmation, PasswordReset passwordReset) : base(userManager, signInManager)
        {
            _emailConfirmation = emailConfirmation; // addscope unutma program.cs
            _passwordReset = passwordReset;
            //this.userManager = userManager;
            //this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            /*
            // eğer önceden  giriş yapılmışsa o zaman tekrar Home/Index sayfasına gitmek yerine Member/Index sayfasına yönlendirsin
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Member");
            }
            */

            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            // userViewModel.City = "Istanbul";
            //userViewModel.BirthDay = DateTime.Now;
            // userViewModel.Picture = null;
            //userViewModel.Gender = Gender.Bay;

            if (ModelState.IsValid) // startup kısmında validationlar AppUser için ayarlandı -backend taraflı- 
            {
                // *** userManager.User, Any => bool
                if (userManager.Users.Any(u => u.PhoneNumber == userViewModel.PhoneNumber))
                {
                    ModelState.AddModelError("", "Bu telefon numarası kayıtlıdır.");
                    return View(userViewModel);
                }


                AppUser user = new AppUser();
                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;
                user.City = userViewModel.City;
                //user.BirthDay = userViewModel.BirthDay;
                user.Picture = userViewModel.Picture;
                user.Gender = (int)userViewModel.Gender;

                // validation işlemi yapılır
                IdentityResult result = await userManager.CreateAsync(user, userViewModel.Password);

                if (result.Succeeded)
                {
                    // create token
                    string confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    // create link, ResetPasswordConfirm action metot'da querystring olarak kullanılacak
                    string link = Url.Action("ConfirmEmail", "Home", new
                    {
                        userId = user.Id,
                        token = confirmationToken
                    }, protocol: HttpContext.Request.Scheme

                    ); // Action bir aşağıda

                    // link in SendEmail
                    //Helper.EmailConfirmation.SendEmail(link, user.Email);
                    _emailConfirmation.Send(link, user.Email);

                    TempData["EmailConfirmMessage"] = "Giriş yapabilmek için Email'inize gelen linki tıklayınız.";

                    return RedirectToAction("LogIn");
                }
                else
                {
                    // kod tekrarı önlendi..
                    AddModelError(result);
                    /*foreach (IdentityError item in result.Errors)
                    {
                        ModelState.AddModelError("",item.Description);
                    }*/
                }
            }

            return View(userViewModel); // aynı sayfaya yönlendirir..  AddModelError(result) kısmında hatalar eklenip kullanıcıya model(userViewModel) tekrar gönderilir..

        }

        // ConfirmEmail      
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);

            IdentityResult result = await userManager.ConfirmEmailAsync(user, token);//**

            if (result.Succeeded)
            {
                ViewBag.status = "Email adresiniz onaylanmıştır. Login ekranından giriş yapabilirsiniz.";
            }
            else
            {
                ViewBag.status = "Bir hata meydana geldi. lütfen daha sonra tekrar deneyiniz.";
            }
            return View();
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

                    
                    // user email'ini doğrularsa, ConfirmEmail action çalışır, ve sonra user, login olunca buradan geçebilir..
                    if (userManager.IsEmailConfirmedAsync(user).Result == false)//**
                    {
                        ModelState.AddModelError("", "Email adresiniz onaylanmamıştır. Lütfen  epostanızı kontrol ediniz.");
                        return View(userlogin);
                    }
                    

                    await signInManager.SignOutAsync(); // login işleminden önce çıkış yapılıdı amaç sistemdeki eski cookie'i silmek..

                    // PasswordSignInAsync ile Login işlemi gerçekleştirilir..
                    // opts.ExpireTimeSpan = System.TimeSpan.FromDays(60); (StartUp.cs'de) bunu aktif hale getirmek için userlogin.RememberMe true olması gerek
                    // lockoutFailure: false lockoutFailure özelliği ile kullanıcı başarısız girişlerde kullanıcıyı kilitleyip kilitlememe durumu.. veritabanında LockoutEnabled kısmı etkiler..
                    // SignInResult ile hata verir bir Identity'den gelen SignInResult bir de ASPNET.Core.MVC den gelen var bu iki durum çakışıyor bunu engellemek için başına Microsoft.AspNetCore.Identity namespace'i eklendi..
                    // validation işlemi yapılır
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, userlogin.Password, userlogin.RememberMe, false);

                    /*
                      Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.CheckPasswordSignInAsync(user, userlogin.Password, false);                 
                     */

                    // şifre doğru mu değil mi => SignInResult result 
                    if (result.Succeeded)
                    {
                        // doğru giriş durumunda veritabanındaki AccessFailedCount sıfırlanır..
                        await userManager.ResetAccessFailedCountAsync(user);

                        // LogIn get metodundan gelir.. önce giriş yapmadan üye sayfasına (ReturnUrl) girmeye çalışırsa login sayfasına düşer ve mekanizma string ReturnUrl üretir ama sonra login olursa o zaman önceden girmeye çalıştığı üye sayfasına (TempData["ReturnUrl"]) direkt gider.. "Index", "Member" sayfasına değil.. 
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
                            // kullanıcıya hata bilgisi verirken specifik olarak hatanın nerede olduğu belirtilmemeli
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
                _passwordReset.Send(passwordResetLink, user.Email);

                //ViewBag.email = passwordResetLink;


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
                   // İstenmeyen property'ler için diğer bir yöntem.. [Bind("PasswordNew")]
                   //[Bind(Include = "Password")] attribute'da Email null gelir ve ModelState.IsValid olsa idi o kısımda hata verirdi..
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
                //** ResetPasswordAsync: şifre sıfırlanacak.. validation işlemi yapılır..
                IdentityResult result = await userManager.ResetPasswordAsync(user, token, passwordResetViewModel.PasswordNew);

                if (result.Succeeded)
                {
                    // SecurityStamp: veritabanındaki bir alan yeni bir SecurityStamp oluşturulacak (user ile ilgili bir bilgi değiştirildiği zaman yapılması gerekir)
                    // nedeni: cookie içerisinde stamp bilgisi var ve user şifresini değiştirince artık yeni şifre ile login olmalı bunun içinde cookie/stamp değiştirilmeli, zira cookie içerisindeki stamp de ile veritabanındaki stamp karşılaştırılır ve farklı olduklarından login sayfasına atar.. o yüzden cookie/stamp değiştirilmelidir..
                    await userManager.UpdateSecurityStampAsync(user);

                    /*
                     
                    await signInManager.SignOutAsync();
                    await signInManager.SignInAsync(user, true); // password ile giriş yapılmayacak
                                                                 //artık cookie yenilendi.. 30 dk sonra sistemden atılmayacak user
                     
                     */

                    ViewBag.status = "success"; // ResetPasswordConfirm.cshtml'de kullanılacak..
                }
                else
                {
                    // kod tekrarı önlendi..
                    AddModelError(result);
                    /*foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);                       
                        
                    }*/

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



        // https://console.cloud.google.com/projectselector2/apis/dashboard?supportedpurview=project
        // Your Client ID 882283884932-4rcl90b3m4lgqb6a9766dh1qp649sfkj.apps.googleusercontent.com
        // Your Client Secret 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
        public IActionResult GoogleLogin(string ReturnUrl)

        {
            if (TempData["ReturnUrl"] != null)
            {
                ReturnUrl = TempData["ReturnUrl"].ToString(); // ben ekledim!!
            }

            string RedirectUrl = Url.Action("ExternalResponse", "Home", new { ReturnUrl = ReturnUrl });

            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", RedirectUrl);

            return new ChallengeResult("Google", properties);
        }


        // https://apps.dev.microsoft.com/#/appList ahmetgokdemirtc@hotmail.com
        public IActionResult MicrosoftLogin(string ReturnUrl)

        {
            if (TempData["ReturnUrl"] != null)
            {
                ReturnUrl = TempData["ReturnUrl"].ToString(); // ben ekledim!!
            }

            string RedirectUrl = Url.Action("ExternalResponse", "Home", new { ReturnUrl = ReturnUrl });

            var properties = signInManager.ConfigureExternalAuthenticationProperties("Microsoft", RedirectUrl);

            return new ChallengeResult("Microsoft", properties);
        }



        // App ID: 588387496267633 & App secret: 73c40dd7d1e6b50927a369da27d83cf4
        public IActionResult FacebookLogin(string ReturnUrl)

        {
            if (TempData["ReturnUrl"] != null)
            {
                ReturnUrl = TempData["ReturnUrl"].ToString(); // ben ekledim!!
            }

            string RedirectUrl = Url.Action("ExternalResponse", "Home", new { ReturnUrl = ReturnUrl }); // Facebook ile login sonrası gideceği sayfa

            var properties = signInManager.ConfigureExternalAuthenticationProperties("Facebook", RedirectUrl); // Facebook ile bağlanmaya çalıştığını belirtir.. RedirectUrl ile facebook sayfasında giriş yaptıktan sonra gideceği sayfa belirtilir.

            return new ChallengeResult("Facebook", properties);
        }

        // 
        public async Task<IActionResult> ExternalResponse(string ReturnUrl = "/")
        {
            // string ReturnUrl = "/" => demek https://localhost:7112/Home/Index veya https://localhost:7112/Home demek 

            ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync(); // User'ın facebook bilgileri..
            // info.LoginProvider: provider's name (example: facebook)
            // info.ProviderKey: UserId in provider (example: facebook UserId)

            // facebook bilgileri girilmemiş demek
            if (info == null)
            {
                return RedirectToAction("LogIn");
            }
            else
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true); // true: ExpireTimeSpan 30 days.. AspNetUserLogins tablosu kullanılacak..

                if (result.Succeeded) // user önceden kayıt olmuş demektir (AspNetUserLogins tablosuna)
                {
                    return Redirect(ReturnUrl);
                    //**   https://localhost:7112/Home/Index veya https://localhost:7112/Home gider.. orada da return RedirectToAction("Index", "Member"); sayfasına gider..
                }
                else // ilk kez 3.party authentication olursa (AspNetUserLogins tablosunda kayıtlı değil)
                {
                    AppUser user = new AppUser();

                    user.Email = info.Principal.FindFirst(ClaimTypes.Email).Value; // facebookdan gelen claim içerisindeki email (Ideniity API faceden alır ve claim'e çevirir)
                    string ExternalUserId = info.Principal.FindFirst(ClaimTypes.NameIdentifier).Value; // facebookdan gelen claim içerisindeki UserId, bu value userName'de kulllanılacak

                    if (info.Principal.HasClaim(x => x.Type == ClaimTypes.Name))
                    {
                        string userName = info.Principal.FindFirst(ClaimTypes.Name).Value;  // facebookdan gelen claim içerisindeki userName

                        userName = userName.Replace(' ', '-').ToLower() + ExternalUserId.Substring(0, 5).ToString();
                        // + ExternalUserId.Substring(0, 5): Guid eklenecek ... aynı kullanıcı isimleri çakışmasın diye
                        user.UserName = userName;
                    } // ClaimTypes.Name yoksa
                    else
                    {
                        user.UserName = info.Principal.FindFirst(ClaimTypes.Email).Value;
                    }

                    user.City = "Istanbul";
                    user.BirthDay = DateTime.Now;
                    user.Picture = "/UserPicture/user.webp";
                    user.Gender = (int)Gender.Bay;

                    AppUser user2 = await userManager.FindByEmailAsync(user.Email);

                    if (user2 == null) // böyle bir kullanıcı yoksa AspNetUsers tablosuna kayıt işlemi yapılmalı..
                    {
                        IdentityResult createResult = await userManager.CreateAsync(user); // AspNetUsers tablosuna kayıt işlemi..

                        if (createResult.Succeeded) // AspNetUsers tablosuna kayıt işlemi başarılı
                        {
                            IdentityResult loginResult = await userManager.AddLoginAsync(user, info); // otomatik olarak login işleminden önce AspNetUserLogins tablosu doldurulacak

                            if (loginResult.Succeeded)
                            {
                                // await signInManager.SignInAsync(user, true); // otomatik olarak login işlemi gerçekleşecek

                                await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
                                // kullanıcının 3.party authentication ile login olduğu anlaşılır zira SignInAsync işlemi ile Identity API tarafından, bu bilgi cookie'ye işlenir..

                                /*
                                Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
                                yukarıda olduğu gibi bu kodda da user'ın Identity API tarafından, 3.party authentication ile login olduğu cookie'ye işlenir..

                                 */

                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                AddModelError(loginResult);
                            }
                        }
                        else
                        {
                            AddModelError(createResult);
                        }
                    }
                    else // böyle bir kullanıcı varsa AspNetUsers tablosuna kayıt işlemi yapılmaz.. ama AspNetUserLogins kaydedilmeli zira başka providerdan (face, google, microsoft vs... ) giriş yapıyor..
                    {
                        IdentityResult loginResult = await userManager.AddLoginAsync(user2, info); // AspNetUserLogins tablosu doldurulacak

                        await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true); // otomatik olarak login işlemi gerçekleşecek

                        return Redirect(ReturnUrl);
                    }
                }
            }

            List<string> errors = ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList();

            return View("Error", errors);

            //return RedirectToAction("Error");

        }

        // 
        public ActionResult Error()
        {
            return View();
        }
    }
}