using System.ComponentModel.DataAnnotations;

namespace Technosoft_Project.ViewModels
{
    public class LoginViewModel
    {        
        // client-validation'lar için kurulucak kütüphaneler.. clide-side library --> jqueryvalidation, jqueryvalidationobtrusive

        [Display(Name = "Email adresiniz")]
        [Required(ErrorMessage = "Email alanı gereklidir")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Şifreniz")]
        [Required(ErrorMessage = "Şifre alanı gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "şifreniz en az 8 karakterli olmalıdır.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

