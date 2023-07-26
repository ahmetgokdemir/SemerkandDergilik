using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.ENTITIES.Models;
using Project.MAP.Identity_Configurations;
using Microsoft.EntityFrameworkCore;
using Project.ENTITIES.Identity_Models;

namespace Project.MAP.Custom_Configurations
{
    public class CategoryofFoodConfiguration : BaseConfiguration<CategoryofFood>
    {
        public override void Configure(EntityTypeBuilder<CategoryofFood> builder)
        {
            base.Configure(builder);

            builder.ToTable("Kategoriler");

            builder.Property(x => x.CategoryName_of_Foods).HasColumnName("Kategori Ad").IsRequired();

            //builder.HasMany<Food>().WithOne().HasForeignKey(p => p.CategoryofFoodID).IsRequired();

        }
    }
}
