using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Identity_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class ImageofFood : EntityBase, IEntity
    {
        public string Food_Image { get; set; }
        public bool IsProfile { get; set; }

        // UserFoodJunction.cs ile ilişkili olacak (public string? Food_Picture { get; set; } // çoklu resim)

        // public Guid UserFoodJunctionAccessibleID { get; set; }
        // public short UserFoodJunctionFoodID { get; set; }
        
        public short UserFoodJunctionID { get; set; }
        public virtual UserFoodJunction UserFoodJunction { get; set; }

    }
}
