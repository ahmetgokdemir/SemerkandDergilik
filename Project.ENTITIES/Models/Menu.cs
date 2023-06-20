using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Menu : EntityBase, IEntity
    {
        public string Menu_Name { get; set; }
        public virtual List<MenuDetail> MenuDetails { get; set; }

        // int kategory_id olabilir
        //public Dictionary<int, Food> _foodItems { get; set; }
        //public decimal? Menu_Price
        //{
        //    get
        //    {
        //        return _foodItems.Sum(x => x.Value.UnitPrice); 

        //    }

        //    // list = dsfd for 
        //}

    }
}
