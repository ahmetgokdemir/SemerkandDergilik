using System.ComponentModel.DataAnnotations;
using Technosoft_Project.Enums;

namespace Technosoft_Project.ViewModels
{
    public class MenuDetailDTO
    {
        public int ID { get; set; }

        //[Required(ErrorMessage = "Menü ismi girmelisiniz.")]
        //[Display(Name = "Menü Adı")]
        //public string Menu_Name { get; set; }

        // MenuDetail Tablosundan gelecekler: 
        public int MenuID { get; set; }

        public int FoodID { get; set; }

        [Required(ErrorMessage = "Kategori seçmelisiniz.")]
        [Display(Name = "Kategori Adı")]
        public string CategoryName_of_Food { get; set; }


        // Food Tablsundan geleçekler: 
        [Required(ErrorMessage = "Yemek seçmelisiniz.")]
        [Display(Name = "Yemek Adı")]
        public string FoodName { get; set; }
 
        [Display(Name = "Yemek Fiyatı")]
        public decimal FoodPrice { get; set; }

        [Display(Name = "Yemek Resmi")]
        public string? FoodPicture { get; set; }

        [Display(Name = "Durum")]
        public Status Status { get; set; } // Aktif, Pasif 

    }
}
