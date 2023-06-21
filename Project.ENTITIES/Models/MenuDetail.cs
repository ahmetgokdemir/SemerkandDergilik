using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class MenuDetail : EntityBase, IEntity
    {
        public string CategoryName_of_Food { get; set; } // join ile uğraşmamak için Category_of_Food.cs'deki ismi Category_of_FoodName

        public decimal FoodPrice { get; set; } // Food.cs --> UnitPrice; Configuration.cs'de money'e çevrilmeli.. 

        public int MenuID { get; set; }
        public int FoodID { get; set; }
        public virtual Menu Menu { get; set; }
        public virtual Food Food { get; set; }
    }
}
