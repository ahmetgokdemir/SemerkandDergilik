using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Configurations
{
    public class AppUserConfiguration : BaseConfiguration<AppUser>
    {
        public override void Configure(EntityTypeBuilder<AppUser> builder)
        {
            base.Configure(builder);


            builder.ToTable("User");
            

            //builder.HasOne(x => x.Profile).WithOne(x => x.AppUser).HasForeignKey<AppUserProfile>(x => x.ID); //birebir ilişki ayarımız icin talimat
            //builder.Ignore(x => x.ID); // ***  burada ID'imiz C#'ta kalsa da biz onu (kendi ID'imizi) Sql'e göndermedik

        }
    }
}
