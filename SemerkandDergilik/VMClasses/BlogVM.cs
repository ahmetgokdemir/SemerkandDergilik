using Semerkand_Dergilik.ViewModels;

namespace Semerkand_Dergilik.VMClasses
{
    public class BlogVM
    {
        public BlogDTO BlogDTO { get; set; }
        public List<BlogDTO> BlogDTOs { get; set; }
        public string JavascriptToRun { get; set; } // henüz kullanılmadı!

    }
}
