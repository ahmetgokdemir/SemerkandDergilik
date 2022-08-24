using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Project.ENTITIES.Models;
using System.Security.Claims;

namespace Semerkand_Dergilik.ClaimProvider
{
    public class ClaimProvider : IClaimsTransformation
    {
        public UserManager<AppUser> userManager { get; set; }

        // dependency injection
        public ClaimProvider(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        // Identity API, cookie den gelen değerlerden claim oluştururken biz de claim ekleyeceğiz dinamik olarak
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // üye mi kontrolü
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = principal.Identity as ClaimsIdentity; // HttpContext.User.Identity

                AppUser user = await userManager.FindByNameAsync(identity.Name); // find user
                // HttpContext.User.Identity.Name => identity.Name

                if (user != null)
                {
                    if (user.BirthDay != null)
                    {
                        if (!principal.HasClaim(c => c.Type == "violence")) // not exits then create it.. ben ekledim!!
                        {
                            var tdy = DateTime.Today;
                            var age = tdy.Year - user.BirthDay?.Year;

                            if (age > 15)
                            {
                                Claim ViolenceClaim = new Claim("violence", true.ToString(), ClaimValueTypes.String, "Internal");

                                identity.AddClaim(ViolenceClaim);
                            }
                        }
                    }

                    if (user.City != null)
                    {
                        // control that existing of city claim in User(principal)
                        if (!principal.HasClaim(c => c.Type == "city")) // not exits then create it..
                        {
                            // type, value, value type, issuer(dağıtıcı):Internal(iç mekanizma)
                            Claim CityClaim = new Claim("city", user.City, ClaimValueTypes.String, "Internal");

                            identity.AddClaim(CityClaim);
                        }
                    }
                }
            }

            return principal; // Program.cs de devam..
        }
    }
}
