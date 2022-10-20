using Semerkand_Dergilik.ViewModels;

namespace Semerkand_Dergilik.VMClasses
{
    public class ProductVM
    {
        public ProductDTO ProductDTO { get; set; }
        public List<ProductDTO> ProductDTOs { get; set; }
        public CategoryDTO CategoryDTO { get; set; }
        public string JavascriptToRun { get; set; }
    }
}
