using Microsoft.AspNetCore.Identity;
using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Identity_Models
{
    public class AppRoleClaim : IdentityRoleClaim<Guid>, IEntity
    {

        //public override int Id { get; set; }

        //public int Primary_ID { get; set; }
        public int ID { get; set; }  // identiyden gelen id olduğu için ignore edilecek configuration'da 

        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DataStatus? DataStatus { get; set; }

        //public int AppRoleID { get; set; }
        //public virtual AppRole AppRole { get; set; }

    }
}
