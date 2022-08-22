using Microsoft.AspNetCore.Identity;
using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class AppUserRole: IdentityUserRole<Guid>, IEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DataStatus? Status { get; set; }

        //public int AppUserID { get; set; }
        //public int AppRoleID { get; set; }

        //public virtual AppUser AppUser { get; set; }
        //public virtual AppRole AppRole { get; set; }
    }
}
