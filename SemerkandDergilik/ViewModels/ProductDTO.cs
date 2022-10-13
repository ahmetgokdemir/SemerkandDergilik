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

        //public int CategoryID { get; set; }

        //[Display(Name = "Kategori Adı")]
        //public string CategoryName { get; set; } // Aktif, Pasif

        public CategoryDTO Category { get; set; }

        [Display(Name = "Ürün Resmi")]
        public string? ProductPicture { get; set; }

        [Required(ErrorMessage = "Stok sayısı giriniz.")]
        [Display(Name = "Stoktaki Ürün Adedi")]
        public short UnitsInStock { get; set; }

        [Required(ErrorMessage = "Ürün fiyatı giriniz.")]
        [Display(Name = "Ürün Fiyat")]
        public decimal UnitPrice { get; set; } // ProductConfiguration.cs'de money'e çevrilmeli.. 

        [Display(Name = "İskonto")]
        public short? Discount { get; set; }

        [Display(Name = "Durum")]
        [Required(ErrorMessage = "Ürün durumunu giriniz.")]
        public Status Status { get; set; } // Aktif, Pasif


    }
}
