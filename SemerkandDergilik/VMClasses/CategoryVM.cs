using Project.ENTITIES.Models;
using Semerkand_Dergilik.ViewModels;

namespace Semerkand_Dergilik.VMClasses
{
    public class CategoryVM
    {
        public CategoryDTO CategoryDTO { get; set; }
        public List<CategoryDTO> CategoryDTOs { get; set; }
        public string JavascriptToRun { get; set; }
    }
}
