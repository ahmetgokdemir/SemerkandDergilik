using Project.ENTITIES.Models;
using System.ComponentModel.DataAnnotations;
using Technosoft_Project.ViewModels;

namespace Technosoft_Project.VMClasses
{
    public class MenuDetailVM
    {
        public MenuDetailDTO MenuDetailDTO { get; set; }
        public List<MenuDetailDTO> MenuDetailDTOs { get; set; }
        public string JavascriptToRun { get; set; }
        public List<Category_of_FoodDTO> Categories_of_Menu_DTOs { get; set; }
        public List<Category_of_FoodDTO> Categories_of_AllFoods_DTOs { get; set; }


        [Required(ErrorMessage = "Yemek seçiniz.")]
        public List<int> _foodList_ID { get; set; }

        [Required(ErrorMessage = "Kategori seçiniz.")]
        public List<string> _categoryList { get; set; } 

        public int menu_id { get; set; }

    }
}
