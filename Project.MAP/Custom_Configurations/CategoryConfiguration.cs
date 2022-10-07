using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.ENTITIES.Models;
using Project.MAP.Identity_Configurations;
using Microsoft.EntityFrameworkCore;
using Project.ENTITIES.Identity_Models;

namespace Project.MAP.Custom_Configurations
{
    public class CategoryConfiguration : BaseConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.CategoryName).HasColumnName("Kategori İsmi").IsRequired();

            //builder.HasMany<Product>().WithOne().HasForeignKey(p => p.CategoryID).IsRequired();
            builder.ToTable("Kategoriler");
        }
    }
}
