using Technosoft_Project.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Technosoft_Project.ViewModels
{
    public class FoodDTO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Ürün ismi gereklidir.")]
        [Display(Name = "Ürün Adı")]
        public string FoodName { get; set; }


        [Required(ErrorMessage = "Ürün fiyatı giriniz.")]
        [Display(Name = "Ürün Fiyat")]
        [Range(1, 1000000000000)]
        public decimal UnitPrice { get; set; } // FoodConfiguration.cs'de money'e çevrilmeli.. 

        
        //[Required(ErrorMessage = "Stok sayısı giriniz.")]
        //[Display(Name = "Stoktaki Ürün Adedi")]
        //[Range(1, 1000000000000)]
        // public short UnitsInStock { get; set; }


        [Display(Name = "İskonto")]
        [Range(0, 1000000000000)]
        public short? Discount { get; set; }


        //public int Category_of_FoodID { get; set; }
        public int Category_of_FoodID { get; set; }
         
        //[Display(Name = "Kategori Adı")]
        //public string Category_of_FoodName { get; set; } // Aktif, Pasif
        // [Display(Name = "Kategori DTO Adı")]
        // [Required(ErrorMessage = "Kategori ismi gereklidir...")]
        // public Category_of_FoodDTO Category_of_Food { get; set; }


        [Display(Name = "Durum")]
        [Required(ErrorMessage = "Ürün durumunu giriniz.")]
        public Status Status { get; set; } // Aktif, Pasif


        [Display(Name = "Ürün Resmi")]
        public string? FoodPicture { get; set; }


    }
}
