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

            builder.ToTable("Kullanici_Yemek_Detayi");

            // builder.Ignore(x => x.ID);
            
            // builder.Ignore(x => x.AppUser.Id);

            // builder.HasKey(x => new { x.AppUser.Id, x.FoodID }); // ... is not a valid member access expression. The expression should represent a simple property or field acces HATASI VERİYOR
            
            // builder.HasKey(x => new { x.AccessibleID, x.FoodID });

 
            builder.Property(x => x.Food_Price).HasColumnName("Yemek Fiyat").IsRequired(); 
            builder.Property(x => x.Food_Status).HasColumnName("Yemek Mevcudiyet Durum").IsRequired();
            builder.Property(x => x.Food_Description).HasColumnName("Yemek Açıklama");
            builder.Property(x => x.Food_Picture).HasColumnName("Yemek Resim");

            builder.Property(x => x.Food_Price).HasColumnType("smallmoney");
            builder.Property(x => x.Food_Status).HasColumnType("smallint");

            builder.Property(x => x.Food_Description).HasMaxLength(256);
            // builder.Property(x => x.Food_Picture).HasMaxLength(256);

            builder.Property(x => x.AccessibleID).HasColumnName("Kullanici_ID").IsRequired();
            builder.Property(x => x.FoodID).HasColumnName("Yemek_ID").IsRequired();
        }
    }
}
