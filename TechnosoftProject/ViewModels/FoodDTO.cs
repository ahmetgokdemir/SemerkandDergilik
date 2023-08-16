using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Project.ENTITIES.Enums;

namespace Technosoft_Project.ViewModels
{
    public class FoodDTO
    {
        public short ID { get; set; }

        [Display(Name = "Yemek Adı")]
        [Required(ErrorMessage = "Yemek ismi gereklidir.")]
        [MaxLength(30, ErrorMessage = "Yemek en fazla 30 karakterli olmalıdır.")]
        public string Food_Name { get; set; }


        // sonra yoruma al... !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // sonra yoruma al... !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // sonra yoruma al... !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        [Required(ErrorMessage = "Yemek fiyatı giriniz.")]
        [Display(Name = "Ürün Fiyat")]
        [Range(1, 1000)]
        public decimal FoodPrice { get; set; } // FoodConfiguration.cs'de money'e çevrilmeli.. 

        
        //[Required(ErrorMessage = "Stok sayısı giriniz.")]
        //[Display(Name = "Stoktaki Ürün Adedi")]
        //[Range(1, 1000000000000)]
        // public short UnitsInStock { get; set; }


        [Display(Name = "İskonto")]
        [Range(0, 1000000000000)]
        public short? Discount { get; set; }


        //public int CategoryofFoodID { get; set; }
        public int CategoryofFoodID { get; set; }
         
        //[Display(Name = "Kategori Adı")]
        //public string CategoryofFoodName { get; set; } // Aktif, Pasif
        // [Display(Name = "Kategori DTO Adı")]
        // [Required(ErrorMessage = "Kategori ismi gereklidir...")]
        // public CategoryofFoodDTO CategoryofFood { get; set; }


        [Display(Name = "Durum")]
        [Required(ErrorMessage = "Yemek durumunu gereklidir.")]
        public ExistentStatus _ExistentStatus { get; set; } // Aktif, Pasif


        [Display(Name = "Yemek Resmi")]
        public string? FoodPicture { get; set; }


    }
}
