using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Product : EntityBase, IEntity
    {
        public string ProductName { get; set; }
        public short UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; } // ProductConfiguration.cs'de money'e çevrilmeli.. 
        public string? ProductPicture { get; set; }
        public int Status { get; set; } // Aktif, Pasif... EntityBase'de ki gibi DataStatus verilebilir idi int yerine... ve casting işlemlerine gerek kalmaz idi..
        public short? Discount { get; set; } 


        public int CategoryID { get; set; }
        //Relational Properties
        public virtual Category Category { get; set; }
    }
}
