using Project.ENTITIES.Models;
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





        public List<FoodDTO>? Foods_of_Categeriesvvv { get; set; }

        public List<FoodDTO> AllFoods_ { get; set; }


        
        public List<string> FoodNames { get; set; }



        public Category_of_FoodDTO SelectedCategory { get; set; }

    }
}
