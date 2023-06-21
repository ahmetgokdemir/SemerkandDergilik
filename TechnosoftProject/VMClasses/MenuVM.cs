using Technosoft_Project.ViewModels;

namespace Technosoft_Project.VMClasses
{
    public class MenuVM
    {
        public MenuDTO MenuDTO { get; set; }
        public List<MenuDTO> MenuDTOs { get; set; }
        public string JavascriptToRun { get; set; }
    }
}
