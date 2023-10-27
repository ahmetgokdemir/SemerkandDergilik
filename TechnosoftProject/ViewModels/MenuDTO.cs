using Project.ENTITIES.Enums;
using System.ComponentModel.DataAnnotations;

namespace Technosoft_Project.ViewModels
{
    public class MenuDTO
    {
        public short ID { get; set; }

        [Required(ErrorMessage = "Menü ismi girmelisiniz.")]
        [Display(Name = "Menü Adı")]
        public string Menu_Name { get; set; }

        [Display(Name = "Durum")]
        [Required(ErrorMessage = "Menü durumunu giriniz.")]
        public ExistentStatus _ExistentStatus { get; set; } // Aktif, Pasif

        [Required(ErrorMessage = "Kişi Kimliği gereklidir.")]
        public Guid AccessibleID { get; set; } // user_id useraccessavleid
    }
}
