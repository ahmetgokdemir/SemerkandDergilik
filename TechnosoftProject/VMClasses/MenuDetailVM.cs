using Technosoft_Project.ViewModels;

namespace Technosoft_Project.VMClasses
{
    public class MenuDetailVM
    {
        public MenuDetailDTO MenuDetailDTO { get; set; }
        public List<MenuDetailDTO> MenuDetailDTOs { get; set; }
        public string JavascriptToRun { get; set; }
        public List<Category_of_FoodDTO> Category_of_FoodDTOs { get; set; }
    }
}
