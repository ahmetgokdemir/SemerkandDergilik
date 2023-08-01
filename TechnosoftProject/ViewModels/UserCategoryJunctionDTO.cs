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
        public string? CategoryofFood_Picture { get; set; }  // çoklu resim

        [Display(Name = "Kategori Açıklama")]
        [MaxLength(256, ErrorMessage = "Açıklama en fazla 256 karakterli olmalıdır.")]
        public string? CategoryofFood_Description { get; set; }

        public Guid AccessibleID { get; set; } // user_id useraccessavleid
        public short CategoryofFoodID { get; set; }
        public Guid AppUserId { get; set; }

        [Required(ErrorMessage = "Kategori ismi gereklidir.")]
        [Display(Name = "Kategori Adı")]
        public string CategoryName_of_Foods { get; set; }

        /*
                             CategoryName_of_Foods = x.CategoryofFood.CategoryName_of_Foods, // include
                    CategoryofFood_Picture = x.CategoryofFood_Picture,
                    CategoryofFood_Status = x.CategoryofFood_Status,
                    AppUserId = x.AppUser.Id, // ID (IdentityUser'den gelir ve erişilemez onun yerine AppUser dan id e erişilir)
                    CategoryofFoodID = x.CategoryofFoodID
         
         
         */


    }
}
