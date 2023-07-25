using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class CategoryFoodJunction : EntityBase, IEntity
    {
        public int FoodID { get; set; }
        public virtual Food Food { get; set; }

        // public int Category_of_Food_ID { get; set; }  _ID dan dolayı hata...
        public int Category_of_FoodID { get; set; }
        public virtual Category_of_Food Category_of_Food { get; set; }

    }
}
