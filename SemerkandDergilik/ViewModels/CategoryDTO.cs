using Semerkand_Dergilik.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Semerkand_Dergilik.ViewModels
{
    public class CategoryDTO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Kategori ismi gereklidir.")]
        [Display(Name = "Kategori Adı")]
        public string CategoryName { get; set; }

        [Display(Name = "Kategori Resmi")]
        public string? CategoryPicture { get; set; }

        [Display(Name = "Durum")]
        [Required(ErrorMessage = "Kategori durumunu giriniz.")]
        public Status Status { get; set; } // Aktif, Pasif
    }
}
