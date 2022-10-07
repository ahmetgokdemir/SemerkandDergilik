using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project.DAL.Context;
using Project.ENTITIES.Identity_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// using Semerkand_Dergilik; referans verilmeli ki using'i kullanılabilsin

namespace Project.DAL.Strategy
{
    public static class SeedRoles
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            using (UserManager<AppUser> _userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>())
            {

                List<AppUser> userlist = new List<AppUser>()
                {
                    new AppUser(){ UserName="ahmetgokdemir", PasswordHash= "ctz*9913", Email = "ahmetgokdemirtc@gmail.com", EmailConfirmed = true, Gender = 1, Picture = "/UserPicture/user.webp", City="Istanbul" }
                    // new AppUser(){UserName ="David@hotmial.com", Password="Abc12345!"}
                };

                /*
                var userlistv2 = new AppUser()
                {
                    UserName = "ahmetgokdemir",
                    PasswordHash = "ctz*9913",
                    Email = "ahmetgokdemirtc@gmail.com",
                    EmailConfirmed = true,
                    Gender = 1,
                    Picture = "/UserPicture/user.webp"

                };
                */

                using (RoleManager<AppRole> _roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>())
                {
                    List<AppRole> rolelist = new List<AppRole>()
                    {
                        new AppRole(){ Name="Admin" },
                        new AppRole(){ Name="Manager" },
                        new AppRole(){ Name="Editor" }

                    };

                    foreach (AppRole role in rolelist)
                    {
                        if (!_roleManager.Roles.Any(r => r.Name == role.Name))
                        {
                            IdentityResult result = _roleManager.CreateAsync(role).Result;
                        }
                    }
                }




                foreach (AppUser user in userlist)
                {
                    if (!_userManager.Users.Any(r => r.UserName == user.UserName))
                    {
                        //var newuser = new IdentityUser { UserName = user.UserName, Email = user.UserName };
                        IdentityResult result = await _userManager.CreateAsync(user, user.PasswordHash);
                        bool b = result.Succeeded;
                        await _userManager.AddToRoleAsync(user, "Admin");
                        bool a = result.Succeeded;
                    }

                    


                }


            }
        }

    }
}

