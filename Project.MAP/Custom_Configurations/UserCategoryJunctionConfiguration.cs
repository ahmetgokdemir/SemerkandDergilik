using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.ENTITIES.Models;
using Project.MAP.Identity_Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Project.ENTITIES.Enums;

namespace Project.MAP.Custom_Configurations
{
    public class UserCategoryJunctionConfiguration : BaseConfiguration<UserCategoryJunction>
    {
        public override void Configure(EntityTypeBuilder<UserCategoryJunction> builder)
        {
            base.Configure(builder);

            builder.ToTable("Restoran Kategori Detay");

            builder.Ignore(x => x.ID);
            // builder.Ignore(x => x.AppUser.Id);

            //IdentityUser iu = new IdentityUser();
            // x.AccessibleID

            // builder.HasKey(x => new { x.AppUser.AccessibleID, x.CategoryofFoodID });
            // ... is not a valid member access expression. The expression should represent a simple property or field acces HATASI VERİYOR

            builder.HasKey(x => new { x.AccessibleID, x.CategoryofFoodID });

            builder.Property(x => x.CategoryofFood_Status).HasColumnName("Kategori Mevcudiyet Durum").IsRequired();
            builder.Property(x => x.CategoryofFood_Description).HasColumnName("Kategori Açıklama");
            builder.Property(x => x.CategoryofFood_Picture).HasColumnName("Kategori Resim");

            builder.Property(x => x.CategoryofFood_Status).HasColumnType("smallint");

            builder.Property(x => x.CategoryofFood_Description).HasMaxLength(256);
            // builder.Property(x => x.CategoryofFood_Picture).HasMaxLength(512);

        }
    }
}
