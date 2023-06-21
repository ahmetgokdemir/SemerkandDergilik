using Technosoft_Project.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Technosoft_Project.ViewModels
{
    public class Category_of_FoodDTO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Kategori ismi gereklidir.")]
        [Display(Name = "Kategori Adı")]
        public string Category_of_FoodName { get; set; } // CategoryName_of_Food olacak

        [Display(Name = "Kategori Resmi")]
        public string? Category_of_FoodPicture { get; set; }

        [Display(Name = "Durum")]
        [Required(ErrorMessage = "Kategori durumunu giriniz.")]
        public Status Status { get; set; } // Aktif, Pasif
    }
}
