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
    public class CategoryFoodJunctionConfiguration : BaseConfiguration<CategoryFoodJunction>
    {
        public override void Configure(EntityTypeBuilder<CategoryFoodJunction> builder)
        {
            base.Configure(builder);

            builder.ToTable("Category and Foods Details");

            builder.Ignore(x => x.ID);
            builder.HasKey(x => new { x.FoodID, x.Category_of_FoodID });
           
        }
    }
}
