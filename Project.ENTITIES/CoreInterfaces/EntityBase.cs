using Project.ENTITIES.Enums;
using System.ComponentModel.DataAnnotations;

namespace Project.ENTITIES.CoreInterfaces
{
    public abstract class EntityBase : IEntity
    {
        //[Key]
        //public int Primary_ID { get; set; }
        public int ID { get; set; }

        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual DateTime? DeletedDate { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual DataStatus? DataStatus { get; set; }
        // public int Status { get; set; }
        public EntityBase()
        {
            CreatedDate = DateTime.Now;
            DataStatus = Enums.DataStatus.Inserted;
        }
    }
}
