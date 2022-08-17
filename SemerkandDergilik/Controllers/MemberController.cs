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
            // HttpContext.User.Identity.Name, veritabanındaki UserName karşılığıdır.. ama Identity.Name cookie den geliyor..
            AppUser user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;            

            // fazla property'si olan class'lar için kısayol automap.. (User class'ın daki bilgileri UserViewModel'e basar)
            // mapster kütüphanesi indirilsin (dependencies --> manage nuget)

            UserViewModel userViewModel = user.Adapt<UserViewModel>(); // automap

            return View(userViewModel);
        }

        // PasswordChange actiom metot ->MemberLayouttan geliyor
        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (ModelState.IsValid)
            {
                //AppUser user = CurrentUser;
                AppUser user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

                // şifre kontrolü
                bool exist = userManager.CheckPasswordAsync(user, passwordChangeViewModel.PasswordOld).Result;

                if (exist) // şifre kontrolü başarılı
                {
                    // change işlemi
                    IdentityResult result = userManager.ChangePasswordAsync(user, passwordChangeViewModel.PasswordOld, passwordChangeViewModel.PasswordNew).Result;

                    if (result.Succeeded)
                    {
                        await userManager.UpdateSecurityStampAsync(user); // eski şifreyi iptal eder.. cookie bilgisi silinir

                        /*
                         eski şifreyi iptal eder.. cookie bilgisi silinir..

                        Identity API 30 dk da bir cookie bilgisi ile securitystamp bilgisini kontrol ediyor.. eğer eşleşmezler iseler logout olur sistem..

                                            // SecurityStamp: veritabanındaki bir alan yeni bir SecurityStamp oluşturulacak (user ile ilgili bir bilgi değiştirildiği zaman yapılması gerekir)
                    // nedeni: cookie içerisinde stamp bilgisi var ve user şifresini değiştirince artık yeni şifre ile login olmalı bu yüzden cookie/stamp değiştirilmeli
                         
                         */
                        await signInManager.SignOutAsync(); // çıkış yapıldı 


                        // user otomatik olarak yeni şifre ile login oldu.. PasswordSignInAsync metodu HomeController da LogIn action da kullanıldı 
                        await signInManager.PasswordSignInAsync(user, passwordChangeViewModel.PasswordNew, true, false);

                        ViewBag.success = "true";
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                        //AddModelError(result);

                    }
                }
                else // şifre kontrolü başarısız
                {
                    ModelState.AddModelError("", "Eski şifreniz yanlış");
                }
            }

            return View(passwordChangeViewModel); // şifre değişilince gene PasswordChange sayfasında kalır
        }

    }
}
