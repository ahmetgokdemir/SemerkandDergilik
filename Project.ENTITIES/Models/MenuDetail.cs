using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Identity_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class MenuDetail : EntityBase, IEntity
    {
        public string CategoryName_of_Foods { get; set; } // join ile uğraşmamak için CategoryofFood.cs'deki ismi CategoryName_of_Foods

        // public decimal FoodPrice { get; set; } // UserFoodJunction.cs --> UnitPrice; Configuration.cs'de money'e çevrilmeli.. 

        //public ExistentStatus Status_MenuDetail { get; set; } // Aktif, Pasif --> UserFoodJunction.cs deki ExistentStatus kullaılabilir bunun yerine

        // Menu.cs'de AccessableUserID olduğu için burada tekrarlanmadı...


        public short MenuID { get; set; }
        public virtual Menu Menu { get; set; }
        public short UserFoodJunctionID { get; set; } //***
        public virtual UserFoodJunction UserFoodJunction { get; set; }

        // ?
        // public int AppUserID { get; set; }
        // public virtual AppUser AppUser { get; set; } // AppUserID olayı (ID) AppUser (class isminden mi) AppUser (class'a verilen isimden mi idi?) hangisinden idi 
    }
}
