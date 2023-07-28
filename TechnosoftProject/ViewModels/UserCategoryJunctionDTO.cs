using Project.ENTITIES.Enums;
using Project.ENTITIES.Identity_Models;
using Project.ENTITIES.Models;
using System.ComponentModel.DataAnnotations;

namespace Technosoft_Project.ViewModels
{
    public class UserCategoryJunctionDTO
    {


        [Display(Name = "Kategori Mevcudiyet Durum")]
        [Required(ErrorMessage = "Kategori durum giriniz.")]
        public ExistentStatus CategoryofFood_Status { get; set; } // Aktif, Pasif , Adapt, _CategoryofFood_Status şeklinde ('_') kabul etmiyor 

        [Display(Name = "Kategori Resim")]
        public string? CategoryofFoodPicture { get; set; }  // çoklu resim

        [Display(Name = "Kategori Açıklama")]
        [MaxLength(256, ErrorMessage = "Açıklama en fazla 256 karakterli olmalıdır.")]
        public string? CategoryofFood_Description { get; set; }

        public Guid AccessibleID { get; set; } // user_id useraccessavleid
        public short CategoryofFoodID { get; set; }


    }
}
