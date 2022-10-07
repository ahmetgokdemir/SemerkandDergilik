using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Project.ENTITIES.Identity_Models;
using Microsoft.EntityFrameworkCore;
using Project.ENTITIES.Models;
using Project.MAP.Identity_Configurations;
using Project.MAP.Custom_Configurations;

namespace Project.DAL.Context
{
    public class SemerkandDergilikContext : IdentityDbContext<AppUser, AppRole, Guid, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        public SemerkandDergilikContext(DbContextOptions<SemerkandDergilikContext> options) : base(options)
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<MyContext>());
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AppUserConfiguration());
            builder.ApplyConfiguration(new AppRoleConfiguration());
            builder.ApplyConfiguration(new AppRoleClaimConfiguration());
            builder.ApplyConfiguration(new AppUserClaimConfiguration());
            builder.ApplyConfiguration(new AppUserLoginConfiguration());
            builder.ApplyConfiguration(new AppUserRoleConfiguration());
            builder.ApplyConfiguration(new AppUserTokenConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new CouponConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());



            //builder.Entity<AppUserRole>(builder =>
            //{
            //    //builder.HasOne(userRole => userRole.AppRole).WithMany(role => role.AppUserRoles).HasForeignKey(userRole => userRole.RoleId).OnDelete(DeleteBehavior.Restrict);


            //    //builder.HasOne(userRole => userRole.AppUser).WithMany(user => user.AppUserRoles).HasForeignKey(userRole => userRole.UserId);//.OnDelete(DeleteBehavior.Restrict);

            //    //builder.Ignore(userRole => userRole.AppUserID);
            //    //builder.Ignore(userRole => userRole.AppRoleID);

            //    builder.ToTable("UserRole");

            //    //builder.Property(roleClaim => roleClaim.RoleId).HasColumnName("Vendor Name");
            //});






            /*

            builder.Entity<AppRoleClaim>(builder =>
            {
                builder.HasOne(roleClaim => roleClaim.AppRole).WithMany(role => role.AppRoleClaims).HasForeignKey(roleClaim => roleClaim.RoleId).OnDelete(DeleteBehavior.Restrict);
                builder.ToTable("RoleClaim");
                //builder.Property(roleClaim => roleClaim.RoleId).HasColumnName("Vendor Name");
            });

            builder.Entity<AppRoleClaim>().ToTable("RoleClaim");

            builder.Entity<AppUserClaim>(builder =>
            {
                builder.HasOne(userClaim => userClaim.AppUser).WithMany(user => user.AppUserClaims).HasForeignKey(userClaim => userClaim.UserId).OnDelete(DeleteBehavior.Restrict);
                builder.ToTable("UserClaim");
            });

            builder.Entity<AppUserLogin>(builder =>
            {
                builder.HasOne(userLogin => userLogin.AppUser).WithMany(user => user.AppUserLogins).HasForeignKey(userLogin => userLogin.UserId).OnDelete(DeleteBehavior.Restrict);
                builder.ToTable("UserLogin");
            });

            builder.Entity<AppUserRole>(builder =>
            {
                builder.HasOne(userRole => userRole.AppRole).WithMany(role => role.AppUserRoles).HasForeignKey(userRole => userRole.RoleId).OnDelete(DeleteBehavior.Restrict);
                builder.HasOne(userRole => userRole.AppUser).WithMany(user => user.AppUserRoles).HasForeignKey(userRole => userRole.UserId).OnDelete(DeleteBehavior.Restrict);
                builder.ToTable("UserRole");
            });

            builder.Entity<AppUserToken>(builder =>
            {
                builder.HasOne(userToken => userToken.AppUser).WithMany(user => user.AppUserTokens).HasForeignKey(userToken => userToken.UserId).OnDelete(DeleteBehavior.Restrict);
                builder.ToTable("UserToken");
            });

            builder.Entity<AppUserToken>(builder =>
            {
                builder.Property(x => x.CreatedDate).HasColumnName("Oluşturulma Tarihi");
            });

            */

            base.OnModelCreating(builder);
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppRoleClaim> AppRoleClaims { get; set; }
        public DbSet<AppUserClaim> AppUserClaims { get; set; }
        public DbSet<AppUserLogin> AppUserLogins { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }
        public DbSet<AppUserToken> AppUserTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Product> Products { get; set; }




    }
}

