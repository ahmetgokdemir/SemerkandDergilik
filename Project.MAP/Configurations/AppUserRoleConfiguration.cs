﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Project.MAP.Configurations
{
    public class AppUserRoleConfiguration : BaseConfiguration<AppUserRole>
    {
        public override void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            base.Configure(builder);
 
            builder.HasOne(userRole => userRole.AppRole).WithMany(role => role.AppUserRoles).HasForeignKey(userRole => userRole.RoleId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(userRole => userRole.AppUser).WithMany(user => user.AppUserRoles).HasForeignKey(userRole => userRole.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("UserRole");
 

        }
    }
}
