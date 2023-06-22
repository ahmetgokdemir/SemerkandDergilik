using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Blog : EntityBase, IEntity
    {
        // public int ID { get; set; }
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Description { get; set; }

        // public int Status { get; set; }


    }
}
