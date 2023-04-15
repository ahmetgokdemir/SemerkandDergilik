using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Project.ENTITIES.Enums;

namespace Project.ENTITIES.Identity_Models
{
    public class AppUser : IdentityUser<Guid>, IEntity
    {
        //public int Primary_ID { get; set; }
        public int ID { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DataStatus? DataStatus { get; set; }

        ////////

        public string City { get; set; }
        public string Picture { get; set; }
        public DateTime? BirthDay { get; set; }
        public int Gender { get; set; }

        public AppUser() : base()
        {
            CreatedDate = DateTime.Now;
            DataStatus = Enums.DataStatus.Inserted;
        }

        public AppUser(string userName) : base(userName)
        {
            CreatedDate = DateTime.Now;
            DataStatus = Enums.DataStatus.Inserted;
        }

        //public virtual ICollection<AppUserToken> AppUserTokens { get; set; }        
        //public virtual ICollection<AppUserRole> AppUserRoles { get; set; }
        //public virtual ICollection<AppUserLogin> AppUserLogins { get; set; }
        //public virtual ICollection<AppUserClaim> AppUserClaims { get; set; }

    }
}
