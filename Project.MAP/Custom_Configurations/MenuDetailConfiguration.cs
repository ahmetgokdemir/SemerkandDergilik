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

            builder.ToTable("Kullanici_Menu_Detayi");

            builder.Ignore(x => x.ID);

            builder.HasKey(x => new { x.MenuID, x.UserFoodJunctionID });

            builder.Property(x => x.CategoryName_of_Foods).HasColumnName("Kategori Ad").IsRequired();
            builder.Property(x => x.CategoryName_of_Foods).HasMaxLength(128);

        }
    }
}
