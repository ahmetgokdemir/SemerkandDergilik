using Project.ENTITIES.Models;
using Technosoft_Project.ViewModels;

namespace Technosoft_Project.VMClasses
{
    public class CategoryVM
    {
        public CategoryDTO CategoryDTO { get; set; }
        public List<CategoryDTO> CategoryDTOs { get; set; }
        public string JavascriptToRun { get; set; }
    }
}
