using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Food : EntityBase, IEntity
    {
        public string Food_Name { get; set; }
        public virtual List<UserFoodJunction> UserFoodJunctions { get; set; }
        
        // public virtual List<MenuDetail> MenuDetails { get; set; }

        // ** public decimal UnitPrice { get; set; } // FoodConfiguration.cs'de money'e çevrilmeli.. 
        // ** public string? FoodPicture { get; set; }
        // ** public int Status { get; set; } // Aktif, Pasif... EntityBase'de ki gibi DataStatus verilebilir idi int yerine... ve casting işlemlerine gerek kalmaz idi..

        // public short? Discount { get; set; }
        // public short UnitsInStock { get; set; }

        //Relational Properties
        // public int CategoryofFoodID { get; set; }
        // public virtual CategoryofFood CategoryofFood { get; set; }
        // public virtual List<CategoryFoodJunction> CategoryFoodJunctions { get; set; }

    }
}
