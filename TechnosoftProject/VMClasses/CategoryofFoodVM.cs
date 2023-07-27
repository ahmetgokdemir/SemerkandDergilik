using Technosoft_Project.ViewModels;

namespace Technosoft_Project.VMClasses
{
    public class CategoryofFoodVM
    {
        public CategoryofFoodDTO CategoryofFoodDTO { get; set; }
        public List<CategoryofFoodDTO> CategoryofFoodDTOs { get; set; }
        public UserCategoryJunctionDTO UserCategoryJunctionDTO { get; set; }
        public List<UserCategoryJunctionDTO> UserCategoryJunctionDTOs { get; set; }

        public string JavascriptToRun { get; set; }
    }
}
