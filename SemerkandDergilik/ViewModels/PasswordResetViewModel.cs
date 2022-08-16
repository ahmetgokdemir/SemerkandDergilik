using System.ComponentModel.DataAnnotations;

namespace Semerkand_Dergilik.ViewModels
{
    public class PasswordResetViewModel
    {
        [Display(Name = "Email adresiniz")]
        [Required(ErrorMessage = "Email alanı gereklidir")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Yeni şifreniz")]
        [Required(ErrorMessage = "Şifre alanı gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "şifreniz en az 8 karakterli olmalıdır.")]
        public string PasswordNew { get; set; }
    }
}
