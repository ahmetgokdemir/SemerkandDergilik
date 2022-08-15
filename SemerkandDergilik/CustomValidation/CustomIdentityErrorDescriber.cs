using Microsoft.AspNetCore.Identity;

namespace Semerkand_Dergilik.CustomValidation
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber // IdentityErrorDescriber: Doğrulama mesajlarının türkçeleştirilmesi sağlayan sınıf..
    {
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "InvalidUserName",
                Description = $" Bu {userName} geçersizdir."
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "DuplicateUserName",
                Description = $" Bu Kulllanıcı adı({userName}) zaten kullanılmaktadır."
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = "DuplicateEmail",
                Description = $" Bu {email} kullanılmaktadır."
                //Bu f@outlook.com kullanılmaktadır.
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            /*
                int length nereden gelir?
                StartUp.cs'de opts.Password.RequiredLength = 8; kısmından             
            */

            return new IdentityError()
            {
                Code = "PasswordTooShort",
                Description = $"Şifreniz en az {length} karakterli olmalıdır."
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresDigit",
                Description = $"Şifreniz en az bir rakam içermelidir."
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = $"Şifreniz en az bir alfanümerik olmayan karakter içermelidir."
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresLower",
                Description = $"Şifreniz en az bir küçük harf içermelidir."
            };
        }

    

        // StartUp.cs kısmında ayarlamalar...
    }
}
