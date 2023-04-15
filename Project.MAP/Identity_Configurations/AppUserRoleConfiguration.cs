using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.ENTITIES.Identity_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Project.MAP.Identity_Configurations
{
    public class AppUserRoleConfiguration : BaseConfiguration<AppUserRole>
    {
        public override void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            base.Configure(builder);

            builder.Ignore(x => x.ID); // *** identity api den gelen id ile ientity deki çatmaması için


            //builder.HasOne(userRole => userRole.AppRole).WithMany(role => role.AppUserRoles).HasForeignKey(userRole => userRole.RoleId);//.OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(userRole => userRole.AppUser).WithMany(user => user.AppUserRoles).HasForeignKey(userRole => userRole.UserId);//.OnDelete(DeleteBehavior.Restrict);

            // builder.Ignore(userRole => userRole.AppUserID);
            // builder.Ignore(userRole => userRole.AppRoleID);


            builder.ToTable("UserRole");
 

        }
    }
}
