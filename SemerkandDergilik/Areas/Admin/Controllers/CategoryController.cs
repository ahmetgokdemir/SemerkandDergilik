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
        public async Task<IActionResult> CategoryList()
        {
            IEnumerable<Category> categoryList = await _icm.GetAllAsync();

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

        [Route("AddCategory")]
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDTO cdto)
        {          

            if (ModelState.IsValid)
            {
                Category ctg = cdto.Adapt<Category>();

                ctg.Status = (int)cdto.Status;

                if (ctg.ID == null)
                {
                    await _icm.AddAsync(ctg);
                }
                else
                {
                    _icm.Update(ctg);

                }
               
                return RedirectToAction("CategoryList");
            }

            TempData["mesaj"] = "Ürün adı ve statü giriniz..";
            //ModelState.AddModelError("", "Ürün adı ve statü giriniz..");
            return RedirectToAction("CategoryList");

        }


        [Route("UpdateCategoryAjax")]
        public async Task<IActionResult> UpdateCategoryAjax(int id)
        {
            Category category_item = await _icm.GetByIdAsync(id);
            CategoryDTO cDTO = category_item.Adapt<CategoryDTO>();

            ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));
                               

            return PartialView("_AddCategoryPartial", cDTO);
        }

        [Route("CategoryIndex")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
