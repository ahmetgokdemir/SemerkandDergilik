using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class CategoryofFood : EntityBase, IEntity
    {
        public string CategoryName_of_Foods { get; set; } // CategoryName_of_Food

        // Çoka çok ilişki
        public virtual List<UserCategoryJunction> UserCategoryJunctions { get; set; }

        // ** public string Description { get; set; }
        // ** public string? CategoryofFoodPicture { get; set; }
        // ** public int Status { get; set; } // Aktif, Pasif

        // public virtual List<Food> Foods { get; set; }        
        // public virtual List<CategoryFoodJunction> CategoryFoodJunctions { get; set; }
    }
}