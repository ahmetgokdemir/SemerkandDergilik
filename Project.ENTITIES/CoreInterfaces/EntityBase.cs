using Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.CoreInterfaces
{
    public abstract class EntityBase : IEntity
    {
        public int ID { get; set; }
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual DateTime? DeletedDate { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual DataStatus? Status { get; set; }

        public EntityBase()
        {
            CreatedDate = DateTime.Now;
            Status = DataStatus.Inserted;
        }
    }
}
