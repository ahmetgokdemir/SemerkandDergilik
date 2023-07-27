using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.ENTITIES.Models;
using Project.MAP.Identity_Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Project.MAP.Custom_Configurations
{
    public class MenuConfiguration : BaseConfiguration<Menu>
    {
        public override void Configure(EntityTypeBuilder<Menu> builder)
        {
            base.Configure(builder);

            builder.Ignore(x => x.AppUserID);

            builder.Property(x => x.Menu_Name).HasColumnName("Menu Ad").IsRequired();
            builder.Property(x => x.Menu_Status).HasColumnName("Menu Durum").IsRequired();

            builder.Property(x => x.Menu_Status).HasColumnType("smallint");

            builder.Property(x => x.Menu_Name).HasMaxLength(128);

        }
    }
}
