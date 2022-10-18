using Semerkand_Dergilik.ViewModels;

namespace Semerkand_Dergilik.VMClasses
{
    public class ProductVM
    {
        public ProductDTO Product { get; set; }
        public List<ProductDTO> Products { get; set; }

        public string JavascriptToRun { get; set; }
    }
}
