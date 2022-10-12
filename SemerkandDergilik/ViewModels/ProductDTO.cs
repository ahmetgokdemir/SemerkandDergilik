using Semerkand_Dergilik.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Semerkand_Dergilik.ViewModels
{
    public class ProductDTO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Ürün ismi gereklidir.")]
        [Display(Name = "Ürün Adı")]
        public string ProductName { get; set; }

        [Display(Name = "Ürün Resmi")]
        public string? ProductPicture { get; set; }

        [Display(Name = "Durum")]
        [Required(ErrorMessage = "Ürün durumunu giriniz.")]
        public Status Status { get; set; } // Aktif, Pasif

        [Display(Name = "Kategori Adı")]
        public string CategoryName { get; set; } // Aktif, Pasif
    }
}
