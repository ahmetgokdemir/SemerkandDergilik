using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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


            builder.ToTable("User");

            builder.HasMany<AppUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            builder.HasMany<AppUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
            builder.HasMany<AppUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

            builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

            builder.Property(user => user.UserName).HasColumnName("Kullanıcı Adı");

            //builder.HasOne(x => x.Profile).WithOne(x => x.AppUser).HasForeignKey<AppUserProfile>(x => x.ID); //birebir ilişki ayarımız icin talimat
            //builder.Ignore(x => x.ID); // ***  burada ID'imiz C#'ta kalsa da biz onu (kendi ID'imizi) Sql'e göndermedik

        }
    }
}
