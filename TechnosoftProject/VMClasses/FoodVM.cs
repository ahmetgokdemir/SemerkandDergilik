using Technosoft_Project.ViewModels;

namespace Technosoft_Project.VMClasses
{
    public class FoodVM
    {
        public FoodDTO FoodDTO { get; set; }
        public List<FoodDTO> FoodDTOs { get; set; }
        public Category_of_FoodDTO Category_of_FoodDTO { get; set; }
        public string JavascriptToRun { get; set; }
    }
}
