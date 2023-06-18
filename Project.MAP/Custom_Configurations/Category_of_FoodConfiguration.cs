using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.ENTITIES.Models;
using Project.MAP.Identity_Configurations;
using Microsoft.EntityFrameworkCore;
using Project.ENTITIES.Identity_Models;

namespace Project.MAP.Custom_Configurations
{
    public class Category_of_FoodConfiguration : BaseConfiguration<Category_of_Food>
    {
        public override void Configure(EntityTypeBuilder<Category_of_Food> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Category_of_FoodName).HasColumnName("Kategori İsmi").IsRequired();

            //builder.HasMany<Food>().WithOne().HasForeignKey(p => p.Category_of_FoodID).IsRequired();
            // builder.ToTable("Kategoriler");
        }
    }
}
