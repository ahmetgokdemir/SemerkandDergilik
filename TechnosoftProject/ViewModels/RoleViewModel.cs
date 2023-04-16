using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Technosoft_Project.ViewModels
{
    public class RoleViewModel
    {
        [Display(Name = "Role ismi")]
        [Required(ErrorMessage = "Role ismi gereklidir")]
        public string Name { get; set; }

        // role güncelleme işlemi için Id property'si tutulacak
        public string Id { get; set; }
    }
}
