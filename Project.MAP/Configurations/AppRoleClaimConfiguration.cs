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
    public class AppRoleClaimConfiguration : BaseConfiguration<AppRoleClaim>
    {
        public override void Configure(EntityTypeBuilder<AppRoleClaim> builder)
        {
            base.Configure(builder);

           // builder.HasOne(roleClaim => roleClaim.AppRole).WithMany(role => role.AppRoleClaims).HasForeignKey(roleClaim => roleClaim.RoleId);//OnDelete(DeleteBehavior.Restrict);

            //builder.Property(roleClaim => roleClaim.Id).HasConversion<Guid>();
            //builder.HasIndex(roleClaim => roleClaim.Id).IsUnique();
            builder.HasKey(roleClaim => roleClaim.Id);

            //builder.Ignore(roleClaim => roleClaim.AppRoleID);

            //builder.Entity<AppRoleClaim>().ToTable("RoleClaim");
            builder.ToTable("RoleClaim");
        }
    }
}
