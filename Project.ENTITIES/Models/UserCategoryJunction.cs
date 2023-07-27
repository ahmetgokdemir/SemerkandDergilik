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
    public class UserCategoryJunction : EntityBase, IEntity
    {
        public ExistentStatus CategoryofFood_Status { get; set; } = (ExistentStatus) 1; // Aktif, Pasif (Kategori Durumu)
        public string? CategoryofFood_Description { get; set; }
        public string? CategoryofFood_Picture { get; set; } // çoklu resim


        public Guid AccessibleID { get; set; }
        public virtual AppUser AppUser { get; set; }
        public short CategoryofFoodID { get; set; }
        public virtual CategoryofFood CategoryofFood { get; set; }

    }
}
