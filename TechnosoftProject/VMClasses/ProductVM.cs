using Technosoft_Project.ViewModels;

namespace Technosoft_Project.VMClasses
{
    public class ProductVM
    {
        public ProductDTO ProductDTO { get; set; }
        public List<ProductDTO> ProductDTOs { get; set; }
        public Category_of_FoodDTO Category_of_FoodDTO { get; set; }
        public string JavascriptToRun { get; set; }
    }
}
