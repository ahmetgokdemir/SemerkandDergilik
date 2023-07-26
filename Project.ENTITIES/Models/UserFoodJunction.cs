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
    public class UserFoodJunction : EntityBase, IEntity
    {
        public decimal Food_Price { get; set; } // UserFoodJunctionConfiguration.cs'de money'e çevrilmeli.. 
        public ExistentStatus Food_Status { get; set; } = (ExistentStatus)1; // Aktif, Pasif (Yemek Durumu)  
        public string? Food_Description { get; set; }
        public string? Food_Picture { get; set; } // çoklu resim
        
        // public int Status { get; set; } // Aktif, Pasif... EntityBase'de ki gibi DataStatus verilebilir idi int yerine... ve casting işlemlerine gerek kalmaz idi..


        public int AppUserID { get; set; }
        public virtual AppUser AppUser { get; set; }
        public int FoodID { get; set; }
        public virtual Food Food { get; set; }
    }
}
