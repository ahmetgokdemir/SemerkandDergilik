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
    public class AppRole:IdentityRole<Guid>,IEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DataStatus? Status { get; set; }

        public AppRole() : base()
        {
            CreatedDate = DateTime.Now;
            Status = DataStatus.Inserted;
        }

        public AppRole(string roleName) : base(roleName)
        {
            CreatedDate = DateTime.Now;
            Status = DataStatus.Inserted;
        }

        public virtual ICollection<AppUserRole> AppUserRoles { get; set; }
        public virtual ICollection<AppRoleClaim> AppRoleClaims { get; set; }
    }
}
