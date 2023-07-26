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
    public class UserFoodJunctionConfiguration : BaseConfiguration<UserFoodJunction>
    {
        public override void Configure(EntityTypeBuilder<UserFoodJunction> builder)
        {
            base.Configure(builder);

            builder.ToTable("Restoran Yemek Detayı");

            builder.Ignore(x => x.ID);
            builder.Ignore(x => x.AppUserID);

            builder.HasKey(x => new { x.AppUser.Email, x.FoodID });

            builder.Property(x => x.Food_Price).HasColumnName("Yemek Fiyat").IsRequired();
            builder.Property(x => x.Food_Status).HasColumnName("Yemek Durum").IsRequired();
            builder.Property(x => x.Food_Description).HasColumnName("Yemek Açıklama").IsRequired();
            builder.Property(x => x.Food_Picture).HasColumnName("Yemek Resim").IsRequired();


            // builder.Property(x => x.FoodPrice).HasColumnName("Yemek Fiyati").HasColumnType("money"); //**

        }
    }
}
