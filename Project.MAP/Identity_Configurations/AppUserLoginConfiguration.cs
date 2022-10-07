using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.ENTITIES.Identity_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Identity_Configurations
{
    public class AppUserLoginConfiguration : BaseConfiguration<AppUserLogin>
    {
        public override void Configure(EntityTypeBuilder<AppUserLogin> builder)
        {
            base.Configure(builder);

            //  builder.HasOne(userLogin => userLogin.AppUser).WithMany(user => user.AppUserLogins).HasForeignKey(userLogin => userLogin.UserId);//.OnDelete(DeleteBehavior.Restrict);

            // builder.Ignore(userLogin => userLogin.AppUserID);

            // builder.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });

            builder.Property(ul => ul.LoginProvider).HasMaxLength(128);
            builder.Property(ul => ul.ProviderKey).HasMaxLength(128);

            builder.ToTable("UserLogin");
 
        }
    }
}
