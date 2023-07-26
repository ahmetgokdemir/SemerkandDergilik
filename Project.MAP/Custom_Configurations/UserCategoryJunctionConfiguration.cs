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

namespace Project.MAP.Custom_Configurations
{
    public class UserCategoryJunctionConfiguration : BaseConfiguration<UserCategoryJunction>
    {
        public override void Configure(EntityTypeBuilder<UserCategoryJunction> builder)
        {
            base.Configure(builder);

            builder.ToTable("Restoran Kategori Detayı");

            builder.Ignore(x => x.ID);
            builder.Ignore(x => x.AppUserID);


            builder.HasKey(x => new { x.AppUser.Email, x.CategoryofFoodID });

            builder.Property(x => x.CategoryofFood_Status).HasColumnName("Kategori Durum").IsRequired();
            builder.Property(x => x.CategoryofFood_Description).HasColumnName("Kategori Açıklama").IsRequired();
            builder.Property(x => x.CategoryofFood_Picture).HasColumnName("Kategori Resim").IsRequired();

            // builder.Property(x => x.FoodPrice).HasColumnName("Yemek Fiyati").HasColumnType("money"); //**

        }
    }
}
