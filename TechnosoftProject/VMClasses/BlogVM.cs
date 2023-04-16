using Technosoft_Project.ViewModels;

namespace Technosoft_Project.VMClasses
{
    public class BlogVM
    {
        public BlogDTO BlogDTO { get; set; }
        public List<BlogDTO> BlogDTOs { get; set; }
        public string JavascriptToRun { get; set; } // henüz kullanılmadı!

    }
}
