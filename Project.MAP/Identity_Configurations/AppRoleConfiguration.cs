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
    public  class AppRoleConfiguration : BaseConfiguration<AppRole>
    {
        public override void Configure(EntityTypeBuilder<AppRole> builder)
        {
            base.Configure(builder);
           
            builder.ToTable("Roles");

            //builder.HasKey(r => r.Id);

            builder.HasIndex(r => r.NormalizedName).IsUnique();
            builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
            builder.HasMany<AppRoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

            builder.Property(role => role.Name).HasColumnName("Rol Adı");
        }
    }
}
