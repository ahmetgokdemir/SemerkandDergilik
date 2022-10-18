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

        [Route("CategoryIndex")]
        public IActionResult Index()
        {
            return View();
        }


        [Route("CategoryList")]
        public async Task<IActionResult> CategoryList()
        {
            IEnumerable<Category> categoryList = await _icm.GetActivesAsync();

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

            return PartialView("_CrudCategoryPartial");
        }




        [Route("UpdateCategoryAjax")]
        public async Task<PartialViewResult> UpdateCategoryAjax(int id)
        {
            Category category_item = await _icm.GetByIdAsync(id);
            CategoryDTO cDTO = category_item.Adapt<CategoryDTO>();

            ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));


            return PartialView("_CrudCategoryPartial", cDTO);
        }


        [Route("DeleteCategoryAjax")]
        public async Task<PartialViewResult> DeleteCategoryAjax(int id)
        {
            Category category_item = await _icm.GetByIdAsync(id);
            CategoryDTO cDTO = category_item.Adapt<CategoryDTO>();

            //ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));
            ViewBag.CategoryNameDelete = cDTO.CategoryName;

            ViewBag.CRUD = "delete_operation";

            return PartialView("_CrudCategoryPartial", cDTO);
        }



        [Route("CRUDCategory")]
        [HttpPost]
        public async Task<IActionResult> CRUDCategory(CategoryDTO cdto, IFormFile categoryPicture)
        {
            if (TempData["Deleted"] == null)
            {
                ModelState.Remove("CategoryPicture");

                if (ModelState.IsValid)
                {
                    Category ctg = cdto.Adapt<Category>();

                    ctg.Status = (int)cdto.Status;



                    //////
                    ///
                    if (categoryPicture != null && categoryPicture.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(categoryPicture.FileName); // path oluşturma

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CategoryPicture", fileName); // server'a kayıt edilecek path => wwwroot/UserPicture/fileName

                        // kayıt işlemi
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await categoryPicture.CopyToAsync(stream); // userPicture'ı, stream'e kayıt

                            ctg.CategoryPicture = "/CategoryPicture/" + fileName;   // veritabanına kayıt (wwwroot belirtmeye gerek yok)

                        }
                    }
                    else
                    {
                        Category ctgv2 =  await _icm.GetByIdAsync(cdto.ID);

                        if (ctgv2 != null)
                        {
                            if (ctgv2.CategoryPicture != null)
                            {
                                ctg.CategoryPicture = ctgv2.CategoryPicture;
                            }
                        }

                    }

                    if (ctg.ID == 0)
                    {
                        await _icm.AddAsync(ctg);
                        TempData["mesaj"] = "Kategori eklendi";
                    }
                    else
                    {
                        _icm.Update(ctg);
                        TempData["mesaj"] = "Kategori güncellendi";

                    }

                    return RedirectToAction("CategoryList");
                }

            }
            else
            {
                _icm.Delete(await _icm.GetByIdAsync(cdto.ID));

                // Category ctg = cdto.Adapt<Category>();

                // _icm.Delete(ctg);
                TempData["mesaj"] = "Kategori silindi..";

                TempData["Deleted"] = null;

                return RedirectToAction("CategoryList");
            }

            TempData["mesaj"] = "Ürün adı ve statü giriniz..";
            //ModelState.AddModelError("", "Ürün adı ve statü giriniz..");
            return RedirectToAction("CategoryList");

        }




    }
}
