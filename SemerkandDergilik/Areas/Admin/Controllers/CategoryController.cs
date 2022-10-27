using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Identity_Models;
using Project.ENTITIES.Models;
using Semerkand_Dergilik.CommonTools;
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
        public async Task<IActionResult> CategoryList(string? JSpopupPage)
        {
            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null; // pop-up sıfırlanır yoksa sayfayı reflesleyince geliyor
            }

            IEnumerable<Category> categoryList = await _icm.GetActivesAsync();

            CategoryVM cvm = new CategoryVM
            {
                CategoryDTOs = categoryList.Adapt<IEnumerable<CategoryDTO>>().ToList(),
                JavascriptToRun = JSpopupPage

            };

            JSpopupPage = null;
            return View(cvm);
        }

        [Route("AddCategoryAjax")]
        public PartialViewResult AddCategoryAjax()
        {
            // ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status))); => yerine                                                                 asp-items="Html.GetEnumSelectList<Semerkand_Dergilik.Enums.Status>()" kullanıldı..

            var result = new CategoryDTO();
            CategoryDTO cDTO = new CategoryDTO();

            // HttpContext.Session.SetObject("manipulatedData", pvm_post.ProductDTO);
            if (TempData["HttpContext"] != null)
            {
                cDTO = new CategoryDTO();
                result = HttpContext.Session.GetObject<CategoryDTO>("manipulatedData");
                cDTO = result;

            }

            CategoryVM cVM = new CategoryVM
            {
                CategoryDTO = cDTO
            };

            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(cVM.CategoryDTO.CategoryName))
                {
                    ModelState.AddModelError("CategoryDTO.CategoryName", "Kategori adı giriniz.");
                }

                HttpContext.Session.SetObject("manipulatedData", null);
            }

            Thread.Sleep(500); // pop-up sayfasını tekrar açmayı tetikleyince bazen gelmiyor o yüzden bu kod eklendi..

            return PartialView("_CrudCategoryPartial", cVM);
        }




        [Route("UpdateCategoryAjax")]
        public async Task<PartialViewResult> UpdateCategoryAjax(int id)
        {
            // Category category_item = await _icm.GetByIdAsync(id);
            // CategoryDTO cDTO = category_item.Adapt<CategoryDTO>();

            // ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status))); => yerine                                                                 asp-items="Html.GetEnumSelectList<Semerkand_Dergilik.Enums.Status>()" kullanıldı..


            CategoryDTO cDTO = new CategoryDTO();

            var result = new CategoryDTO();

            if (TempData["HttpContext"] != null)
            {
                result = HttpContext.Session.GetObject<CategoryDTO>("manipulatedData");
                cDTO = result;

                // HttpContext.Session.SetObject("manipulatedData", null);
            }
            else
            {
                Category category_item = await _icm.GetByIdAsync(id);
                cDTO = category_item.Adapt<CategoryDTO>();
                // cdto = product_item.Adapt<CategoryDTO>();
            }




            CategoryVM cVM = new CategoryVM
            {
                CategoryDTO = cDTO
            };



            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(cVM.CategoryDTO.CategoryName))
                {
                    ModelState.AddModelError("CategoryDTO.CategoryName", "Kategori adı giriniz.");
                }

                HttpContext.Session.SetObject("manipulatedData", null);
            }

            Thread.Sleep(500);

            return PartialView("_CrudCategoryPartial", cVM);
        }


        [Route("DeleteCategoryAjax")]
        public async Task<PartialViewResult> DeleteCategoryAjax(int id)
        {
            Category category_item = await _icm.GetByIdAsync(id);
            CategoryDTO cDTO = category_item.Adapt<CategoryDTO>();

            //ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));
            ViewBag.CategoryNameDelete = cDTO.CategoryName;

            ViewBag.CRUD = "delete_operation";

            CategoryVM cVM = new CategoryVM
            {
                CategoryDTO = cDTO
            };

            return PartialView("_CrudCategoryPartial", cVM);
        }



        [Route("CRUDCategory")]
        [HttpPost]
        public async Task<IActionResult> CRUDCategory(CategoryVM cvm_post, IFormFile categoryPicture)
        {
            if (TempData["Deleted"] == null)
            {
                ModelState.Remove("CategoryPicture");
                ModelState.Remove("CategoryDTOs");
                ModelState.Remove("JavascriptToRun");


                if (ModelState.IsValid)
                {
                    Category ctg = cvm_post.CategoryDTO.Adapt<Category>();

                    ctg.Status = (int)cvm_post.CategoryDTO.Status;



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
                        Category ctgv2 = await _icm.GetByIdAsync(cvm_post.CategoryDTO.ID);

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
                _icm.Delete(await _icm.GetByIdAsync(cvm_post.CategoryDTO.ID));

                // Category ctg = cdto.Adapt<Category>();

                // _icm.Delete(ctg);
                TempData["mesaj"] = "Kategori silindi";

                TempData["Deleted"] = null;

                return RedirectToAction("CategoryList");
            }

            // TempData["mesaj"] = "Kategori adı ve statü giriniz..";
            // ModelState.AddModelError("", "Ürün adı ve statü giriniz..");

            CategoryVM cVM = new CategoryVM();
            HttpContext.Session.SetObject("manipulatedData", cvm_post.CategoryDTO);

            TempData["JavascriptToRun"] = "valid";
            TempData["HttpContext"] = "valid";

            if (cvm_post.CategoryDTO.ID != 0) //update
            {
                cVM.JavascriptToRun = $"ShowErrorUpdateOperationPopup({cvm_post.CategoryDTO.ID})";
                return RedirectToAction("CategoryList", new { JSpopupPage = cVM.JavascriptToRun });

            }
            else // add // (pvm_post.ProductDTO.ID == 0) çevir...
            {
                // pvm.JavascriptToRun = $"ShowErrorPopup( {pvm_post.ProductDTO} )";

                // pvm.JavascriptToRun = $"ShowErrorInsertOperationPopup()";

                TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";
                return RedirectToAction("CategoryList", new { JSpopupPage = TempData["JSpopupPage"].ToString() });
            }


        }




    }
}
