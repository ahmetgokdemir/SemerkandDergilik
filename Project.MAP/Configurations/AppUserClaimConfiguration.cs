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
    public class AppUserClaimConfiguration : BaseConfiguration<AppUserClaim>
    {
        public override void Configure(EntityTypeBuilder<AppUserClaim> builder)
        {
            base.Configure(builder);

           // builder.HasOne(userClaim => userClaim.AppUser).WithMany(user => user.AppUserClaims).HasForeignKey(userClaim => userClaim.UserId);//.OnDelete(DeleteBehavior.Restrict);

            //builder.HasIndex(userClaim => userClaim.Id).IsUnique();
            //builder.Property(userClaim => userClaim.Id).HasConversion<Guid>();

           // builder.Ignore(userClaim => userClaim.AppUserID);

            // builder.HasKey(userClaim => userClaim.Id);

            builder.ToTable("UserClaims");

        }
    }
}
