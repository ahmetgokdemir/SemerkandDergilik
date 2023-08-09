using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Identity_Models;
using Technosoft_Project.ViewModels;
using Technosoft_Project.VMClasses;

namespace Technosoft_Project.Controllers
{
    public class FoodController : BaseController
    {

        readonly IFoodManager _ifm;
        readonly IUserFoodJunctionManager _iufjm;

        public FoodController(UserManager<AppUser> userManager, RoleManager<AppRole>    roleManager, IFoodManager ifm, IUserFoodJunctionManager iufjm) : base  (userManager, null, roleManager)
        {
            _ifm = ifm;
            _iufjm = iufjm;
        }

        [Route("FoodList_forMember")]
        public async Task<IActionResult> FoodList_forMember(string? JSpopupPage, string? onlyOnce)
        {
            TempData["onlyOnce"] = onlyOnce;

            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null; // pop-up sıfırlanır yoksa sayfayı reflesleyince geliyor
            }

            string msj;
            if (TempData["messageCategoryofFood_InOtherUsersList"] != null)
            {
                msj = TempData["messageCategoryofFood_InOtherUsersList"].ToString();
                TempData["messageCategoryofFood_InOtherUsersList"] = msj;
            }

            // IEnumerable<CategoryofFood> CategoryofFoodList = await _icm.GetActivesAsync();

            IEnumerable<object> UserFoodJunctionList = await _iufjm.Get_ByUserID_Async(CurrentUser.Id); // IdentityUser'dan gelen Id (Guid tipli)


            FoodVM cvm = new FoodVM
            {
                UserFoodJunctionDTOs = UserFoodJunctionList.Adapt<IEnumerable<UserFoodJunctionDTO>>().ToList(),
                // CategoryofFoodDTOs = CategoryofFoodList.Adapt<IEnumerable<CategoryofFoodDTO>>().ToList(),
                JavascriptToRun = JSpopupPage
            };

            return View("FoodListforMember", cvm);
        }


    }
}
