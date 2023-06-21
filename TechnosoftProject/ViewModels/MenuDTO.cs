using System.ComponentModel.DataAnnotations;
using Technosoft_Project.Enums;

namespace Technosoft_Project.ViewModels
{
    public class MenuDTO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Menü ismi girmelisiniz.")]
        [Display(Name = "Menü Adı")]
        public string Menu_Name { get; set; }

        [Display(Name = "Durum")]
        [Required(ErrorMessage = "Menü durumunu giriniz.")]
        public Status Status { get; set; } // Aktif, Pasif
    }
}
