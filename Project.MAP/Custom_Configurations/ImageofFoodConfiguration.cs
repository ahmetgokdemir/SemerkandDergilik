using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.ENTITIES.Models;
using Project.MAP.Identity_Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Custom_Configurations
{
    public class ImageofFoodConfiguration : BaseConfiguration<ImageofFood>
    {
        public override void Configure(EntityTypeBuilder<ImageofFood> builder)
        {
            base.Configure(builder);

            builder.ToTable("Yemek Resimleri");

            // builder.Ignore(x => x.ID);

            // builder.Ignore(x => x.UserFoodJunctionID); // ignore etmezsek çalışır mı? zira UserFoodJunction'in ID'si ignore edilmişti

            // builder.Ignore(x => x.UserFoodJunction);
            // builder.Ignore(x => x.UserFoodJunction.AccessibleID);
            // builder.Ignore(x => x.UserFoodJunction.FoodID);


            // builder.HasKey(x => new { x.UserFoodJunction.AccessibleID, x.UserFoodJunction.FoodID });
            // builder.HasKey(x => new { x.UserFoodJunctionAccessibleID, x.UserFoodJunctionFoodID });


            //bool IsProfile
            builder.Property(x => x.Food_Image).HasColumnName("Yemek Resim").IsRequired();
  
            // builder.Property(x => x.Food_Image).HasMaxLength(256);

        }
    }
}
