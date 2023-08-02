using Project.ENTITIES.Enums;
using System.ComponentModel.DataAnnotations;

namespace Technosoft_Project.ViewModels
{
    public class MenuDetailDTO
    {
        public int ID { get; set; }

        //[Required(ErrorMessage = "Menü ismi girmelisiniz.")]
        //[Display(Name = "Menü Adı")]
        //public string Menu_Name { get; set; }

        // MenuDetail Tablosundan gelecekler: 
        public short MenuID { get; set; }

        public short FoodID { get; set; }

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
        public ExistentStatus _ExistentStatus { get; set; } // Aktif, Pasif 

    }
}
