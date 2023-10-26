using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Enums;
using Project.ENTITIES.Identity_Models;
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
        public ExistentStatus Menu_Status { get; set; } = (ExistentStatus)1;

        //Relational Properties
        public Guid AccessibleID { get; set; }
        public virtual AppUser AppUser { get; set; } // AppUserID olayı (ID) AppUser (class isminden mi) AppUser (class'a verilen isimden mi idi?) hangisinden idi 
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
