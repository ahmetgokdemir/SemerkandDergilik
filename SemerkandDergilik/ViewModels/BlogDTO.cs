using Semerkand_Dergilik.Enums;
using System.ComponentModel.DataAnnotations;

namespace Semerkand_Dergilik.ViewModels
{
    public class BlogDTO
    {
        public int Primary_ID { get; set; }

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
