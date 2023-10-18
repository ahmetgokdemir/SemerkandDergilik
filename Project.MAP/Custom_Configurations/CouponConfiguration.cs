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
    public class CouponConfiguration : BaseConfiguration<Coupon>
    {
        public override void Configure(EntityTypeBuilder<Coupon> builder)
        {
            base.Configure(builder);
            builder.ToTable("Kuponlar");
            builder.Property(x => x.CouponName).HasColumnName("KuponID").IsRequired();
            builder.Property(x => x.CouponExpireDay).HasColumnName("Son Kullanım Tarihi").IsRequired();

        }
    }
}
