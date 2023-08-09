using Project.ENTITIES.Enums;
using System.ComponentModel.DataAnnotations;

namespace Technosoft_Project.ViewModels
{
    public class UserFoodJunctionDTO
    {
        [Display(Name = "Yemek Fiyat")]
        [Required(ErrorMessage = "Yemek fiyat giriniz.")]
        [Range(1, 1000000000000)]
        public float Food_Price { get; set; }


        [Display(Name = "Yemek Mevcudiyet Durum")]
        [Required(ErrorMessage = "Yemek durum giriniz.")]
        public ExistentStatus Food_Status { get; set; } // Aktif, Pasif , Adapt, _CategoryofFood_Status şeklinde ('_') kabul etmiyor 


        [Display(Name = "Yemek Resim")]
        public string? Food_Picture { get; set; }  // çoklu resim


        [Display(Name = "Yemek Açıklama")]
        [MaxLength(256, ErrorMessage = "Yemek en fazla 256 karakterli olmalıdır.")]
        public string? Food_Description { get; set; }


        public Guid AccessibleID { get; set; } // user_id useraccessavleid
        public short FoodID { get; set; }
        public Guid AppUserId { get; set; }


        [Required(ErrorMessage = "Yemek ismi giriniz.")]
        [Display(Name = "Yemek Adı")]
        public string Food_Name { get; set; }
    }
}
