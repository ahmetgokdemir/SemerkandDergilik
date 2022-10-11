using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Identity_Models;
using Project.ENTITIES.Models;
using Semerkand_Dergilik.Enums;
using Semerkand_Dergilik.ViewModels;
using Semerkand_Dergilik.VMClasses;
using System.Data;

namespace Semerkand_Dergilik.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Authorize(Roles = "Admin")] // case sensitive  
    public class CategoryController : Controller
    {
        readonly ICategoryManager _icm;

        public CategoryController(ICategoryManager icm) // services.AddRepManServices(); 
        {
            _icm = icm;
        }

        [Route("CategoryList")]
        public IActionResult CategoryList()
        {
            IEnumerable<Category> categoryList = _icm.GetAllAsync().Result;

            CategoryVM cvm = new CategoryVM
            {
                Categories = categoryList.Adapt<IEnumerable<CategoryDTO>>().ToList(),
                
            };

            return View(cvm);
        }

        [Route("AddCategoryAjax")]
        public PartialViewResult AddCategoryAjax()
        {
            ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));

            return PartialView("_AddCategoryPartial");
        }


        [Route("CategoryIndex")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
