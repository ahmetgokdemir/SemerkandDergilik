using Microsoft.AspNetCore.Authorization;

namespace Semerkand_Dergilik.ClaimProvider
{
    public class ExpireDateExchangeRequirement : IAuthorizationRequirement
    {
    }

    public class ExpireDateExchangeHandler : AuthorizationHandler<ExpireDateExchangeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExpireDateExchangeRequirement requirement)
        {
            if (context.User != null & context.User.Identity != null)
            {
                var claim = context.User.Claims.Where(x => x.Type == "ExpireDateExchange" && x.Value != null).FirstOrDefault();

                //compare
                if (claim != null)
                {
                    if (DateTime.Now < Convert.ToDateTime(claim.Value))

                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        // if ödeme yapıldı ise succeed
                        // else {context.Fail();}
                        context.Fail(); // access denied
                    }
                }
            }
            return Task.CompletedTask;//**********
            // program.cs de devam
        }
    }
}
