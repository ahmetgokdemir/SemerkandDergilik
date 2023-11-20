using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Project.ENTITIES.Identity_Models;
using System.Security.Claims;

namespace Technosoft_Project.ClaimProvider
{
    public class ClaimProvider : IClaimsTransformation
    {
        public UserManager<AppUser> userManager { get; set; }

        // dependency injection
        public ClaimProvider(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        // Identity API, cookie den gelen değerlerden claim oluştururken biz de claim ekleyeceğiz dinamik olarak...
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            //ClaimsPrincipal principal --> User
            //ClaimsIdentity identity --> principal.Identity(HttpContext.User.Identity)

            // üye mi kontrolü..  
            // TransformAsync methodu üye olsun olmasın her user için her zaman çalışacak o yüzden user bir member mı kontrol edilmeli
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = principal.Identity as ClaimsIdentity; // HttpContext.User.Identity

                AppUser user = await userManager.FindByNameAsync(identity.Name); // find user
                // HttpContext.User.Identity.Name => identity.Name

                if (user != null)
                {
                    //if (user.BirthDay != null)
                    //{
                    //    if (!principal.HasClaim(c => c.Type == "violence")) // not exits then create it.. ben ekledim!!
                    //    {
                    //        var tdy = DateTime.Today;
                    //        var age = tdy.Year - user.BirthDay?.Year;

                    //        if (age > 15)
                    //        {
                                    // 15'den büyükse, true.ToString() olduğundan Claim (yani Confirmed_Member_Policy) Controller/Action'larda çalışır.
                    //            Claim ViolenceClaim = new Claim("violence", true.ToString(), ClaimValueTypes.String, "Internal");

                    //            identity.AddClaim(ViolenceClaim);
                    //        }
                    //    }
                    //}

                    if (user.City != null)
                    {
                        // control that existing of city claim in User(principal)
                        if (!principal.HasClaim(c => c.Type == "city")) // not exits then create it..
                        {
                            // type, value, value type, issuer(dağıtıcı):Internal(iç mekanizma)
                            // user.City --> "İstanbul" ise Claim (yani Confirmed_Member_Policy) Controller/Action'larda çalışır.
                            Claim CityClaim = new Claim("city", user.City, ClaimValueTypes.String, "Internal");

                            identity.AddClaim(CityClaim);
                        }
                    }

                    if (user.IsConfirmedAccount != null)
                    {
                        if (!principal.HasClaim(c => c.Type == "member")) // not exits then create it..
                        {
                            // user.IsConfirmedAccount.ToString() --> "Aktif" ise Claim (yani Confirmed_Member_Policy) Controller/Action'larda çalışır.
                            // policy.RequireClaim("member","Aktif"); -->  
                            Claim ConfirmedAccountClaim = new Claim("member", user.IsConfirmedAccount.ToString(), ClaimValueTypes.String, "Internal");

                            identity.AddClaim(ConfirmedAccountClaim);
                        }
                    }
                }
            }

            return principal; // Program.cs de devam..
        }
    }
}
