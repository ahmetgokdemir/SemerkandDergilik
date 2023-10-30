using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Project.ENTITIES.Identity_Models;
using System.Drawing;
using System.Text.Encodings.Web;

namespace Technosoft_Project.CustomTagHelpers
{
    //  TagHelper'ın yakalayacağı şey
    [HtmlTargetElement("td", Attributes = "user-roles")]
    public class UserRolesName : TagHelper
    {
        public UserManager<AppUser> UserManager { get; set; }

        public UserRolesName(UserManager<AppUser> userManager)
        {
            this.UserManager = userManager;
        }

        // bind işlemi => user-roles="@user.Id" , user.Id alındı ve UserId'ye set edildi..
        [HtmlAttributeName("user-roles")]
        public string UserId { get; set; }

        // fill to taghelper
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            AppUser user = await UserManager.FindByIdAsync(UserId); // get user

            IList<string> roles = await UserManager.GetRolesAsync(user); // get user's roles

            // html code tarafı
            string html = string.Empty;     // *** string.Empty

            var sb = new System.Text.StringBuilder();
            // roles to html
            // x : herbir rol
            roles.ToList().ForEach(x =>
            {
                html += $"<span style='color:black' class='badge bg-warning'> {x} </span> ";  // {x} kullanabilmek için $ eklendi..
                // html += $"<span style='color:white' class='badge badge-dark'>  {x}  </span>";

                // class='boldCell'
            });

            //var colorStyle = $"color:red"; 
            //output.Attributes.SetAttribute("style", colorStyle);

            output.Content.SetHtmlContent(html); //  td'nin içini output ile dolduracağız

            // _ViewImports.cshtml kısmına namespace eklenmeli : @addTagHelper Technosoft_Project.CustomTagHelpers.*,TechnosoftProject

        }
    }
}
