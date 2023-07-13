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
    public class MenuDetailConfiguration : BaseConfiguration<MenuDetail>
    {
        public override void Configure(EntityTypeBuilder<MenuDetail> builder)
        {
            base.Configure(builder);

            builder.ToTable("Menu Detayi");

            builder.Ignore(x => x.ID); 
            builder.HasKey(x => new { x.MenuID, x.FoodID });

            builder.Property(x => x.CategoryName_of_Food).HasColumnName("Kategori Adi").IsRequired();         
            // builder.Property(x => x.FoodPrice).HasColumnName("Yemek Fiyati").HasColumnType("money"); //**

        }
    }
}
