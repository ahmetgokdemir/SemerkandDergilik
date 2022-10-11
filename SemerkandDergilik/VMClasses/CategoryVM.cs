using Project.ENTITIES.Models;
using Semerkand_Dergilik.ViewModels;

namespace Semerkand_Dergilik.VMClasses
{
    public class CategoryVM
    {
        public CategoryDTO Category { get; set; }
        public List<CategoryDTO> Categories { get; set; }
    }
}
