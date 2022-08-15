using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Configurations
{
    public class AppUserLoginConfiguration : BaseConfiguration<AppUserLogin>
    {
        public override void Configure(EntityTypeBuilder<AppUserLogin> builder)
        {
            base.Configure(builder);
 
            builder.HasOne(userLogin => userLogin.AppUser).WithMany(user => user.AppUserLogins).HasForeignKey(userLogin => userLogin.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("UserLogin");
 
        }
    }
}
