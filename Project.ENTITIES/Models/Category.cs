using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Category : EntityBase, IEntity
    {
        public string CategoryName { get; set; }
        //public string Description { get; set; }
        public string? CategoryPicture { get; set; }
        public int Status { get; set; } // Aktif, Pasif

        public virtual List<Product> Products { get; set; }
    }
}
