﻿using Microsoft.EntityFrameworkCore;
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
    public class FoodConfiguration : BaseConfiguration<Food>
    {
        public override void Configure(EntityTypeBuilder<Food> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.UnitPrice).HasColumnType("money");

            builder.Property(x => x.FoodName).HasColumnName("Ürün Adı").IsRequired();
        }
    }
}
