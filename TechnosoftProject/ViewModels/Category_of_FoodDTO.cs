using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Project.ENTITIES.Enums;

namespace Technosoft_Project.ViewModels
{
    public class CategoryofFoodDTO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Kategori ismi gereklidir.")]
        [Display(Name = "Kategori Adı")]
        public string CategoryofFoodName { get; set; } // CategoryName_of_Food olacak

        [Display(Name = "Kategori Resmi")]
        public string? CategoryofFoodPicture { get; set; }

        [Display(Name = "Durum")]
        [Required(ErrorMessage = "Kategori durumunu giriniz.")]
        public ExistentStatus _ExistentStatus { get; set; } // Aktif, Pasif
    }
}
