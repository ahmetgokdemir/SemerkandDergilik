using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Project.ENTITIES.Enums;
using Project.ENTITIES.Models;

namespace Project.ENTITIES.Identity_Models
{
    public class AppUser : IdentityUser<Guid>, IEntity
    {
        //public int Primary_ID { get; set; }
        public short ID { get; set; }
        public short AccessibleID { get; set; }
        // short short short short

        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Status:
        public DataStatus DataStatus { get; set; }
        public IsConfirmedAccount IsConfirmedAccount { get; set; } = 0; // Pasif , (IsConfirmedAccount) 0 --> redundant (gereksiz)

        // public Gender Gender { get; set; }

        //////// specific properties

        public string City { get; set; }
        public string Picture { get; set; }

        // public DateTime? BirthDay { get; set; }


        public AppUser() : base()
        {
            CreatedDate = DateTime.Now;
            DataStatus = Enums.DataStatus.Inserted;
            // IsConfirmedAccount = 0; 
            AccessibleID = ID;
        }

        public AppUser(string userName) : base(userName)
        {
            CreatedDate = DateTime.Now;
            DataStatus = Enums.DataStatus.Inserted;
        }


        public virtual List<Menu> Menus { get; set; }

        // Junction Table (Çoka çok)
        // public virtual List<MenuDetail> MenuDetails { get; set; } ??? 
        public virtual List<UserCategoryJunction> UserCategoryJunctions { get; set; }
        public virtual List<UserFoodJunction> UserFoodJunctions { get; set; }

        //public virtual ICollection<AppUserToken> AppUserTokens { get; set; }        
        //public virtual ICollection<AppUserRole> AppUserRoles { get; set; }
        //public virtual ICollection<AppUserLogin> AppUserLogins { get; set; }
        //public virtual ICollection<AppUserClaim> AppUserClaims { get; set; }

    }
}
