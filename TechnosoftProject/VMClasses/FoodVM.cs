using Technosoft_Project.ViewModels;

namespace Technosoft_Project.VMClasses
{
    public class FoodVM
    {
        public FoodDTO? FoodDTO { get; set; }
        public List<FoodDTO>? FoodDTOs { get; set; }
        public UserFoodJunctionDTO? UserFoodJunctionDTO { get; set; }
        public List<UserFoodJunctionDTO>? UserFoodJunctionDTOs { get; set; }
        public CategoryofFoodDTO? CategoryofFoodDTO { get; set; }
        public string? JavascriptToRun { get; set; }
    }
}
