﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Configurations
{
    public  class AppRoleConfiguration : BaseConfiguration<AppRole>
    {
        public override void Configure(EntityTypeBuilder<AppRole> builder)
        {
            base.Configure(builder);
           
            builder.ToTable("Roles");
           

        }
    }
}
