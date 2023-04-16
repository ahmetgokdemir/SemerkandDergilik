using Technosoft_Project.Enums;
using System.ComponentModel.DataAnnotations;

namespace Technosoft_Project.ViewModels
{
    public class BlogDTO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Başlık gereklidir.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; }

        [Display(Name = "Alt Başlık")]
        public string? SubTitle { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Durum")]
        [Required(ErrorMessage = "Durum giriniz.")]
        // [Required(ErrorMessage = "Ürün durumunu giriniz.")]
        public BlogStatus Status { get; set; } // a,b,c
    }
}
