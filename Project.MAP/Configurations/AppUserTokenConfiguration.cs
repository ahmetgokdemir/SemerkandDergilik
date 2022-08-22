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
    public class AppUserTokenConfiguration : BaseConfiguration<AppUserToken>
    {
        public override void Configure(EntityTypeBuilder<AppUserToken> builder)
        {
            base.Configure(builder);

           // builder.HasOne(userToken => userToken.AppUser).WithMany(user => user.AppUserTokens).HasForeignKey(userToken => userToken.UserId);//.OnDelete(DeleteBehavior.Restrict);

           // builder.Ignore(userToken => userToken.AppUserID);

            //builder.HasKey(userToken => userToken.Id);

            builder.ToTable("UserToken"); 

        }
    }
}
