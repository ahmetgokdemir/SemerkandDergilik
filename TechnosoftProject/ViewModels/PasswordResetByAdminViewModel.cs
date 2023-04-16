using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Technosoft_Project.ViewModels
{
    public class PasswordResetByAdminViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Display(Name = "Email Adresi")]
        public string Email { get; set; }

        [Display(Name = "Yeni şifre")]
        [Required(ErrorMessage = "Şifre alanı gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "şifreniz en az 8 karakterli olmalıdır.")]
        public string NewPassword { get; set; }
    }
}
