using Microsoft.AspNetCore.Identity; // IPasswordValidator kullanımını sağlar
using Project.ENTITIES.Models;

namespace Semerkand_Dergilik.CustomValidation
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser> // AppUser üzerinde işlemleri yapacak...
    {
        // IPasswordValidator'ın, ValidateAsync metodu bu class içerisinde implement'e edilir .. ValidateAsync içerisindeki parametreler kullanılacak..
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            // IdentityResult dönmemi istiyor fonksiyon o yüzden List<IdentityError> errors oluşturulur ve return edilir..
            List<IdentityError> errors = new List<IdentityError>();

            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                if (!(password.ToLower().Contains(user.Email.ToLower()))) // hem username için hem de email için hata vermemek için..
                {
                    // Code Id'i temsil eder.. 
                    errors.Add(new IdentityError() { Code = "PasswordContainsUserName", Description = "şifre alanı kullanıcı adı içeremez" });
                }
                // if (!user.Email.Contains(user.UserName)) iptal
                // yerine if (!(password.ToLower().Contains(user.Email.ToLower()))) 
            }

            // else if denmemeli hepsi ayrı ayrı kontrol edilmeli..
            if (password.ToLower().Contains("1234"))
            {
                errors.Add(new IdentityError() { Code = "PasswordContains1234", Description = "şifre alanı ardışık sayı içeremez" });
            }

            if (password.ToLower().Contains(user.Email.ToLower()))
            {
                errors.Add(new IdentityError() { Code = "PasswordContainsEmail", Description = "şifre alanı email adresiniz içeremez" });
            }

            // hata yoksa .. Task<IdentityResult> döner..
            if (errors.Count == 0)
            {
                return Task.FromResult(IdentityResult.Success);
            } // hata varsa
            else
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray())); // ToArray => from List to Array
            }

            // bu class yapısını Default validation'a eklemek... StartUp.cs


        }
    }
}
