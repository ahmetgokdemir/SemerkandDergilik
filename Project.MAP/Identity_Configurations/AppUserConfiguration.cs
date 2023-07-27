using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.ENTITIES.Enums;
using Project.ENTITIES.Identity_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Identity_Configurations
{
    public class AppUserConfiguration : BaseConfiguration<AppUser>
    {
        public override void Configure(EntityTypeBuilder<AppUser> builder)
        {
            base.Configure(builder);

            builder.Ignore(x => x.ID); // *** identity api den gelen id ile ientity deki çatmaması için ***  burada ID'imiz C#'ta kalsa da biz onu (kendi ID'imizi) Sql'e göndermedik


            builder.ToTable("Üyeler");

            //builder.HasMany<AppUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            //builder.HasMany<AppUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
            //builder.HasMany<AppUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            //
            //builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            //builder.HasOne(x => x.Profile).WithOne(x => x.AppUser).HasForeignKey<AppUserProfile>(x => x.ID); //birebir ilişki ayarımız icin talimat

            builder.Property(user => user.UserName).HasColumnName("Kullanıcı Adı").IsRequired();
            // builder.Property(x => x.Gender).HasColumnName("Cinsiyet").HasColumnType("Cinsiyet").IsRequired();
            builder.Property(x => x.IsConfirmedAccount).HasColumnName("Onay Durumu").IsRequired();
            builder.Property(user => user.City).HasColumnName("Şehir").IsRequired();
            builder.Property(user => user.Picture).HasColumnName("Kullanıcı Resim");


            // builder.Property(x => x.Gender).HasColumnType("smallint");
            builder.Property(x => x.IsConfirmedAccount).HasColumnType("smallint");

            builder.Property(x => x.UserName).HasMaxLength(128);
            builder.Property(x => x.City).HasMaxLength(128);
            // builder.Property(x => x.Picture).HasMaxLength(128);

        }
    }
}
