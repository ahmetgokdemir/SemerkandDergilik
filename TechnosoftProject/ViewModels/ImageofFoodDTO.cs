namespace Technosoft_Project.ViewModels
{
    public class ImageofFoodDTO
    {
        public string Food_Image { get; set; }
        public bool IsProfile { get; set; }

        // UserFoodJunction.cs ile ilişkili olacak (public string? Food_Picture { get; set; } // çoklu resim)
        // public short UserFoodJunctionID { get; set; }
        public Guid UserFoodJunctionAccessibleID { get; set; }
        public short UserFoodJunctionFoodID { get; set; }
    }
}
