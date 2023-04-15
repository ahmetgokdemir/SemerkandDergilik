using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Identity_Configurations
{
    public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            // builder.HasKey(x => x.Primary_ID);
            builder.HasKey(x => x.ID); // //[Key] yerine bu best practice (FATİH ÇAKIROĞLU)

            builder.Property(x => x.CreatedDate).HasColumnName("Oluşturulma Tarihi");
            builder.Property(x => x.DeletedDate).HasColumnName("Silinme Tarihi");
            builder.Property(x => x.ModifiedDate).HasColumnName("Güncelleme Tarihi");
        }
    }
}
