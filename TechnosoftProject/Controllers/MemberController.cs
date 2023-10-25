using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.ENTITIES.Identity_Models;
using Mapster;
using Technosoft_Project.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security;
using System.Security.Claims;
using Technosoft_Project.VMClasses;
using Project.ENTITIES.Models;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Enums;

namespace Technosoft_Project.Controllers
{
    [Authorize]
    [Authorize(Policy = "Confirmed_Member_Policy")]
    [Route("Member")]
    public class MemberController : BaseController
    {
        // kod tekrarı önlendi..
        //public UserManager<AppUser> userManager { get; }
        //public SignInManager<AppUser> signInManager { get; }
        readonly IBlogManager _ibm;

        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IBlogManager ibm) : base(userManager, signInManager)
        {
            // kod tekrarı önlendi..
            //this.userManager = userManager;
            //this.signInManager = signInManager;
            _ibm = ibm;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {

            // HttpContext.User.Identity.Name, veritabanındaki UserName karşılığıdır.. ama Identity.Name cookie den geliyor..
            // AppUser user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
            // kod tekrarı önlenemedi..
            // AppUser user_control_2 = base.CurrentUser;
            AppUser user = CurrentUser;

            if (user == null)
            {
                return RedirectToAction("LogIn", "Home");
            }

            // fazla property'si olan class'lar için kısayol automap.. (User class'ın daki bilgileri UserViewModel'e basar)
            // mapster kütüphanesi indirilsin (dependencies --> manage nuget)

            UserViewModel userViewModel = user.Adapt<UserViewModel>(); // automap

            return View(userViewModel);
        }

        [Route("PasswordChange")]
        // PasswordChange actiom metot ->MemberLayouttan geliyor
        [AllowAnonymous]
        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("PasswordChange")]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (ModelState.IsValid)
            {
                // kod tekrarı önlenemedi..                
                // AppUser user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
                AppUser user = CurrentUser;

                // UserViewModel userViewModel = user.Adapt<UserViewModel>(); // password null geliyor userviewmodelde o yüzden remove ediyoruz model state'te


                // şifre kontrolü
                bool oldpasswordConfirm = userManager.CheckPasswordAsync(user, passwordChangeViewModel.PasswordOld).Result;

                if (oldpasswordConfirm) // şifre kontrolü başarılı
                {
                    // change işlemi eski şifreyi iptal eder.. validation
                    IdentityResult result = userManager.ChangePasswordAsync(user, passwordChangeViewModel.PasswordOld, passwordChangeViewModel.PasswordNew).Result;

                    if (result.Succeeded)
                    {
                        // 28. SecurityStamp'in işleyiş mekanizması dersine de bakılabilir..

                        await userManager.UpdateSecurityStampAsync(user); // yeni securitystamp lakin hala açık var IdentityAPI 30dk. sonra securitystamp'i kontrol edecek o zamana kadar user sayfalarda gezinebilir bu bir açıtır.. o yüzden cookie (içerisinde eski securitystamp var) SignOutAsync ile sıfırlanmalı.. 

                        /*             
                        Identity API 30 dk da bir cookie bilgisi ile veritabanındaki securitystamp bilgisini
                        kontrol ediyor.. eğer eşleşmezler iseler logout olur sistem.. ki eşleşmiyecekler zira
                        UpdateSecurityStampAsync edildi..

                        SecurityStamp: veritabanındaki bir alan.. yeni bir SecurityStamp oluşturulacak (user ile
                        ilgili bir önemli bilgi değiştirildiği zaman update edilmesi gerekir)
                        

                        backend tarafında kod ile user çıkış ve giriş yaptırılmaz ise veritabanında yeni
                        securitystamp olmasına rağmen cookie içerisinde hala eski securitystamp bulunacak ve 30 dk
                        sonra user logout olur ayrıca 30 dk boyunca eski cookie bilgisi ile dolaşmasıda açık yaratır..

                        30 dk bir olması nedeni veritabanını bu kontrolü yapması ile yormaması için..
                        */

                        await signInManager.SignOutAsync(); // çıkış yapıldı .. cookie bilgisi (içerisinde eski securitystamp var) silinir.. yoksa 30 dk. kadar gezinip eski SecurityStamp ile sayfalarda genizir 30 dk sonra logout olur bu da o zamana kadar güvenlik açığı yaratır..


                        // user otomatik olarak yeni şifre ile login oldu.. PasswordSignInAsync metodu HomeController da LogIn action da kullanıldı..  yeni cookie içerisinde artık güncel securitystamp var)
                        await signInManager.PasswordSignInAsync(user, passwordChangeViewModel.PasswordNew, true, false);

                        /*
                          artık yeni cookie oluşturuldu artık sistem logout olmaz.. 
                         */

                        //AppUser user2 = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

                        ViewBag.success = "true";
                    }
                    else
                    {
                        // kod tekrarı önlendi..
                        /*foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }*/
                        AddModelError(result);

                    }
                }
                else // şifre kontrolü başarısız
                {
                    ModelState.AddModelError("", "Eski şifreniz yanlış");
                }
            }

            return View(passwordChangeViewModel); // şifre değişilince gene PasswordChange sayfasında kalır
        }

        // Üye bilgilerini güncelleme 
        [Route("UserEdit")]
        public IActionResult UserEdit()
        {
            // kod tekrarı önlendi..
            // AppUser user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
            
            AppUser user = CurrentUser;

            if (user == null)
            {
                return RedirectToAction("LogIn", "Home");
            }

            UserViewModel userViewModel = user.Adapt<UserViewModel>(); // automap

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender))); // 1.yöntem 
            // Html.GetEnumSelectList<Gender>() => 2.yöntem html'de kullanılır..

            // GetNames: gender enum'ın isimlerini alır

            /*
            AppUser userv2 = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
            userViewModel.Gender = (Gender)userv2.Gender;
            user.Adapt<UserViewModel>(); bu kod cast işlemini yapıyor dolayısıyla ilk iki koda gerek kalmıyor..
            */

            return View(userViewModel); // güncellenecek bilgiler view'e gönderildi..
        }

        [HttpPost]
        [Route("UserEdit")]
        public async Task<IActionResult> UserEdit(UserViewModel userViewModel, IFormFile userPicture)
        {
            ModelState.Remove("Password");
            //[Bind(Include = "UserName,Em..")] attribute'da Password null gelir ve ModelState.IsValid kısmında hata verir..

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender))); // post işlemden sonra viewbag sıfırlandığı için tekrar yüklenmeli.. 

            //if (userPicture==null || userViewModel.Picture == null)
            //{
                ModelState.Remove("Picture");
                ModelState.Remove("userPicture");
                //ModelState.Remove("Gender");
                //ModelState.Remove("BirthDay");
                //ModelState.Remove("City");

            //}


            if (ModelState.IsValid)
            {
                // kod tekrarı önlendi..
                // AppUser user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
                AppUser user = CurrentUser;

                
                string phone = userManager.GetPhoneNumberAsync(user).Result;

                // girdiği no (userViewModel.PhoneNumber), kendi numarasından(phone) farklı ise
                if (phone != userViewModel.PhoneNumber)
                {
                    // girdiği no daha önceden başka user tarafından kaydedimiş mi öyle ise hata alır..
                    if (userManager.Users.Any(u => u.PhoneNumber == userViewModel.PhoneNumber))
                    {
                        ModelState.AddModelError("", "Bu telefon numarası başka üye tarafından kullanılmaktadır.");
                        return View(userViewModel);
                    }
                    else // girdiği no daha önceden başka user tarafından kaydedimemiş ise
                    {
                        user.PhoneNumber = userViewModel.PhoneNumber;
                    }
                }
                else // girdiği no (userViewModel.PhoneNumber), kendi numarasından(phone) aynı ise
                {
                    user.PhoneNumber = phone;
                }

                // girdiği no (userViewModel.PhoneNumber), kendi numarasıyla(phone) aynı ise bir işlem yapılmaz sadece user.PhoneNumber = userViewModel.PhoneNumber; yapılır..

                if (userPicture != null && userPicture.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userPicture.FileName); // path oluşturma

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserPicture", fileName); // server'a kayıt edilecek path => wwwroot/UserPicture/fileName

                    // kayıt işlemi
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await userPicture.CopyToAsync(stream); // userPicture'ı, stream'e kayıt

                        user.Picture = "/UserPicture/" + fileName;   // veritabanına kayıt (wwwroot belirtmeye gerek yok)
                        
                    }
                }
                

                // güncelleme işlemi
                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                // user.PhoneNumber = userViewModel.PhoneNumber;
                user.City = userViewModel.City;                
                // user.BirthDay = userViewModel.BirthDay;                
                // user.Gender = userViewModel.Gender;

                userViewModel.Picture = user.Picture;

                //  IdentityResult sayesinde backend validation'ları (Program.cs ve CustomValidation kısımlarında) çalışır
                IdentityResult result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user); // kritik bilgi değişti için veritabanında securitystamp güncellendi.. ama cookie bilgisi eski..


                    await signInManager.SignOutAsync();
                     await signInManager.SignInAsync(user, true, null); // password ile giriş yapılmayacak 1.yol
                    //artık cookie yenilendi.. 30 dk sonra sistemden atılmayacak user

                    /* 2.yol -> test edilmedi..
                        UserViewModel userViewModel_2 = user.Adapt<UserViewModel>(); // automap
                        await signInManager.PasswordSignInAsync(user, userViewModel_2.Password, true, false);
                    */




                    // AppUser user2 = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

                    ViewBag.success = "true";
                }
                else
                {
                    // kod tekrarı önlendi..
                    /*
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }*/

                    AddModelError(result);

                }
            }

            return View(userViewModel); // aynı sayfaya bilgiler dolu gider.. ViewBag.success = "true"; ise o kısım gelmez
            // return RedirectToAction("UserEdit", "Member");
        }

        /*
        // Logout
        public ActionResult LogOut()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        */

        [AllowAnonymous]
        public void LogOut()
        {
            signInManager.SignOutAsync(); // opts.LogoutPath = new PathString("/Member/LogOut"); çalışır..
            // return RedirectToAction("Index","Home"); klasik yöntem ve public ActionResult LogOut() olacak..
        }

        /*       
            Eğer kullanıcın yetkisi yoksa --> opts.AccessDeniedPath = new PathString("/Member/AccessDenied"); devreye girer ve yetki hatası verir         
         */
        // // // erişim yetkisi olmayan kullanıcıyı sayfadan Access Denied etme 
        [Route("AccessDenied")]
        [AllowAnonymous]
        public IActionResult AccessDenied(string ReturnUrl)
        {
            string value = ReturnUrl.ToLower();

            if (value.Contains("violencepage"))
            {
                ViewBag.message = "Erişmeye çalıştığınız sayfa şiddet videoları içerdiğinden dolayı 15 yaşında büyük olmanız gerekmektedir";
            }
            else if (value.Contains("ıstanbulpage")) // istanbul olmaz.. ıstanbulpage
            {
                ViewBag.message = "Bu sayfaya sadece şehir alanı ankara olan kullanıcılar erişebilir";
            }
            else if (value.Contains("exchange"))
            {
                ViewBag.message = "30 günlük ücretsiz deneme hakkınız sona ermiştir.";
            }
            else if (value.Contains("member"))
            {
                ViewBag.message = "Uygulamaya giriş için Yönetici onayı gerekmektedir.";
            }
            else
            {
                ViewBag.message = "Bu sayfaya erişim izniniz yoktur. Erişim izni almak için site yöneticisiyle görüşünüz";
            }

            return View();
        }


        // // // rollerin ulaşabileceği sayfalar, case sensitive  
        [Authorize(Roles = "Manager,Admin")]
        [Route("Manager")]
        public IActionResult Manager()
        {
            return View();
        }


        // // // rollerin ulaşabileceği sayfalar, case sensitive  
        [Authorize(Roles = "Editor,Admin")]
        [Route("Editor")]
        public IActionResult Editor()
        {
            return View();
        }

        // customize claim , yetki yoksa Access Denied olur..
        [Authorize(Policy = "IstanbulPolicy")]
        [Route("IstanbulPage")]
        public IActionResult IstanbulPage()
        {
            return View();
        }

        // 2. claim senoryosu  AspNetUserClaims tablosu kullanılmayacak
        [Authorize(Policy = "ViolencePolicy")]
        [Route("ViolencePage")]
        public IActionResult ViolencePage()
        {
            return View();
        }

        // <a class="btn btn-danger btn-block" asp-action="ExchangeRedirect" asp-controller="Member">Borsa bilgileri</a>
        // user ilk kez girdiği anda giriş tarihi AspNetUserClaims tablosunda tutacak..
        [Route("ExchangeRedirect")]
        public async Task<IActionResult> ExchangeRedirect()
        {
            // ClaimsPrincipal principal = new ClaimsPrincipal();
            // bool result = principal.HasClaim(x => x.Type == "ExpireDateExchange");

            // Claim'in var mı yok mu yoksa aşağıda eklenir..
            bool result = User.HasClaim(x => x.Type == "ExpireDateExchange");

            // first enter..
            if (!result)
            {
                // use view until DateTime.Now.AddDays(30)
                // value must be string due to database
                
                Claim ExpireDateExchange = new Claim("ExpireDateExchange", DateTime.Now.AddDays(30).Date.ToShortDateString(), ClaimValueTypes.String, "Internal");

                // CurrentUser olmadı from BaseController
                // AppUser user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
                AppUser user = CurrentUser;


                await userManager.AddClaimAsync(/*CurrentUser*/ user, ExpireDateExchange); // AspNetUserClaims tablosuna eklendi..

                // cookie güncellendi..
                await signInManager.SignOutAsync();
                await signInManager.SignInAsync(/*CurrentUser*/ user, true);
            }

            return RedirectToAction("Exchange"); //**
        }

        // 3. claim senoryosu (30 günlük ücretsiz kullanım hakkı) yetkili ise girebilir.. AspNetUserClaims tablosu kullanılacak
        [Authorize(Policy = "ExchangePolicy")] // program.cs
        [Route("Exchange")]
        public IActionResult Exchange()
        {
            return View();
        }

        [Route("Blog")]
        public IActionResult Blog()
        {
            ViewBag.BlogStatus = new SelectList(Enum.GetNames(typeof(BlogStatus)));

            //if (ViewBag.ErrorMessageBlog == true)
            //{
            //    if (string.IsNullOrEmpty(pvm.FoodDTO.FoodName))
            //    {
            //        ModelState.AddModelError("FoodDTO.FoodName", "Ürün adı giriniz.");
            //    }
            //}

            return View();
        }

        [HttpPost]
        [Route("Blog")]
        public async Task<IActionResult> Blog(BlogVM bvm_post)
        {
            ModelState.Remove("BlogDTOs");
            ModelState.Remove("JavascriptToRun");

            if (ModelState.IsValid)
            {
                Blog blg = bvm_post.BlogDTO.Adapt<Blog>(); // Mapster

                await _ibm.AddAsync(blg);
                TempData["messageBlog"] = "Blog eklendi";

                return RedirectToAction("Blog");

            }

            //ModelState.AddModelError("", "Başlık ve Durum boş geçilemez!");

            ViewBag.ErrorMesssege = true;

            ViewBag.BlogStatus = new SelectList(Enum.GetNames(typeof(BlogStatus)));

            // return RedirectToAction("Blog");
            return View(bvm_post);


        }

    }
}
