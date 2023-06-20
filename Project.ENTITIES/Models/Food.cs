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
        public string FoodName { get; set; }
        
        // public short UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; } // FoodConfiguration.cs'de money'e çevrilmeli.. 
        public string? FoodPicture { get; set; }
        public int Status { get; set; } // Aktif, Pasif... EntityBase'de ki gibi DataStatus verilebilir idi int yerine... ve casting işlemlerine gerek kalmaz idi..
        public short? Discount { get; set; } 


        public int Category_of_FoodID { get; set; }
        //Relational Properties
        public virtual Category_of_Food Category_of_Food { get; set; }
        public virtual List<MenuDetail> MenuDetails { get; set; }

    }
}
