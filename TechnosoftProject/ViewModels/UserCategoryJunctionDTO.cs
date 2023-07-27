using Project.ENTITIES.Enums;
using Project.ENTITIES.Identity_Models;
using Project.ENTITIES.Models;
using System.ComponentModel.DataAnnotations;

namespace Technosoft_Project.ViewModels
{
    public class UserCategoryJunctionDTO
    {
        [Required(ErrorMessage = "Kategori ismi gereklidir.")]
        [Display(Name = "Kategori Adı")]
        public string _CategoryName_of_Foods { get; set; } // CategoryName_of_Food olacak

        [Display(Name = "Kategori Mevcudiyet Durum")]
        [Required(ErrorMessage = "Kategori durum giriniz.")]
        public ExistentStatus _CategoryofFood_Status { get; set; } // Aktif, Pasif

        [Display(Name = "Kategori Resim")]
        public string? _CategoryofFoodPicture { get; set; }

        [Display(Name = "Kategori Açıklama")]
        [MaxLength(256, ErrorMessage = "Açıklama en fazla 256 karakterli olmalıdır.")]
        public string? _CategoryofFood_Description { get; set; }

        public Guid _AccessibleID { get; set; } // user_id
        public short _CategoryofFoodID { get; set; }

     }
}
