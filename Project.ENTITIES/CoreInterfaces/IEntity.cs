using Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.CoreInterfaces
{
    public interface IEntity
    {
        // public int ID { get; set; } gerek yok Identity Sınıflarının Inheratancelarından zaten Guid tipinde ID geliyor.. (AppRole:IdentityRole<Guid> gibi) 

        //public int Primary_ID { get; set; }
        public int ID { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DataStatus? DataStatus { get; set; }
        // public int Status { get; set; }
    }
}
