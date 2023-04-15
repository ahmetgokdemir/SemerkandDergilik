using Microsoft.AspNetCore.Identity;
using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Identity_Models
{
    public class AppUserToken : IdentityUserToken<Guid>,IEntity
    {
        //public int Primary_ID { get; set; }
        public int ID { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DataStatus? DataStatus { get; set; }

        //public int AppUserID { get; set; }
        //public virtual AppUser AppUser { get; set; }
    }
}
