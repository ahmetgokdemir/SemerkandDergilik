using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Category_of_Food : EntityBase, IEntity
    {
        public string Category_of_FoodName { get; set; } // CategoryName_of_Food
        //public string Description { get; set; }
        public string? Category_of_FoodPicture { get; set; }
        public int Status { get; set; } // Aktif, Pasif

        // public virtual List<Food> Foods { get; set; }
        public virtual List<CategoryFoodJunction> CategoryFoodJunctions { get; set; }
    }
}