using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Identity_Models;
using Project.ENTITIES.Models;
using Technosoft_Project.CommonTools;
using Technosoft_Project.Enums;
using Technosoft_Project.ViewModels;
using Technosoft_Project.VMClasses;
using System.Data;
using Microsoft.AspNetCore.Mvc.Routing;
using Castle.Core.Smtp;
using Technosoft_Project.Helper;

namespace Technosoft_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Category_of_Food")]
    [Authorize(Roles = "Admin")] // case sensitive  
    public class Category_of_FoodController : Controller
    {
        readonly ICategory_of_FoodManager _icm;

        public Category_of_FoodController(ICategory_of_FoodManager icm) // services.AddRepManServices(); 
        {
            _icm = icm;
        }

        [Route("Category_of_FoodIndex")]
        public IActionResult Index()
        {
            return View();
        }


        [Route("Category_of_FoodList")]
        public async Task<IActionResult> Category_of_FoodList(string? JSpopupPage)
        {

            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null; // pop-up sıfırlanır yoksa sayfayı reflesleyince geliyor
            }

            IEnumerable<Category_of_Food> Category_of_FoodList = await _icm.GetActivesAsync();

            Category_of_FoodVM cvm = new Category_of_FoodVM
            {
                Category_of_FoodDTOs = Category_of_FoodList.Adapt<IEnumerable<Category_of_FoodDTO>>().ToList(),
                JavascriptToRun = JSpopupPage

            };

            return View(cvm);
        }

        [Route("AddCategory_of_FoodAjax")]
        public PartialViewResult AddCategory_of_FoodAjax()
        {
            // ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status))); => yerine                                                                 asp-items="Html.GetEnumSelectList<Technosoft_Project.Enums.Status>()" kullanıldı..

            var result = new Category_of_FoodDTO();
            Category_of_FoodDTO cDTO = new Category_of_FoodDTO();

            // HttpContext.Session.SetObject("manipulatedData", pvm_post.Category_of_FoodDTO);
            if (TempData["HttpContext"] != null)
            {
                cDTO = new Category_of_FoodDTO();
                result = HttpContext.Session.GetObject<Category_of_FoodDTO>("manipulatedData");
                cDTO = result;

            }

            Category_of_FoodVM cVM = new Category_of_FoodVM
            {
                Category_of_FoodDTO = cDTO
            };

            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(cVM.Category_of_FoodDTO.Category_of_FoodName))
                {
                    ModelState.AddModelError("Category_of_FoodDTO.Category_of_FoodName", "Kategori adı giriniz.");
                }

                HttpContext.Session.SetObject("manipulatedData", null);
            }

            Thread.Sleep(500); // pop-up sayfasını tekrar açmayı tetikleyince bazen gelmiyor o yüzden bu kod eklendi..

            return PartialView("_CrudCategory_of_FoodPartial", cVM);
        }




        [Route("UpdateCategory_of_FoodAjax")]
        public async Task<PartialViewResult> UpdateCategory_of_FoodAjax(int id)
        {
            // Category_of_Food Category_of_Food_item = await _icm.GetByIdAsync(id);
            // Category_of_FoodDTO cDTO = Category_of_Food_item.Adapt<Category_of_FoodDTO>();

            // ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status))); => yerine                                                                 asp-items="Html.GetEnumSelectList<Technosoft_Project.Enums.Status>()" kullanıldı..


            Category_of_FoodDTO cDTO = new Category_of_FoodDTO();

            var result = new Category_of_FoodDTO();

            if (TempData["HttpContext"] != null)
            {
                result = HttpContext.Session.GetObject<Category_of_FoodDTO>("manipulatedData");
                cDTO = result;

                // HttpContext.Session.SetObject("manipulatedData", null);
            }
            else
            {
                Category_of_Food Category_of_Food_item = await _icm.GetByIdAsync(id);
                cDTO = Category_of_Food_item.Adapt<Category_of_FoodDTO>();
                // cdto = product_item.Adapt<Category_of_FoodDTO>();
            }




            Category_of_FoodVM cVM = new Category_of_FoodVM
            {
                Category_of_FoodDTO = cDTO
            };



            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(cVM.Category_of_FoodDTO.Category_of_FoodName))
                {
                    ModelState.AddModelError("Category_of_FoodDTO.Category_of_FoodName", "Kategori adı giriniz.");
                }

                HttpContext.Session.SetObject("manipulatedData", null);
            }

            Thread.Sleep(500);

            return PartialView("_CrudCategory_of_FoodPartial", cVM);
        }


        [Route("DeleteCategory_of_FoodAjax")]
        public async Task<PartialViewResult> DeleteCategory_of_FoodAjax(int id)
        {
            Category_of_Food Category_of_Food_item = await _icm.GetByIdAsync(id);
            Category_of_FoodDTO cDTO = Category_of_Food_item.Adapt<Category_of_FoodDTO>();

            //ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));
            ViewBag.Category_of_FoodNameDelete = cDTO.Category_of_FoodName;

            ViewBag.CRUD = "delete_operation";

            Category_of_FoodVM cVM = new Category_of_FoodVM
            {
                Category_of_FoodDTO = cDTO
            };

            return PartialView("_CrudCategory_of_FoodPartial", cVM);
        }



        [Route("CRUDCategory_of_Food")]
        [HttpPost]
        public async Task<IActionResult> CRUDCategory_of_Food(Category_of_FoodVM cvm_post, IFormFile Category_of_FoodPicture)
        {
            var urlHelper = new UrlHelper(ControllerContext);
            var url = urlHelper.Action("About", "Home");
            var linkText = "Panelden yapılan değiliklik web e yansımıyor";

            var hyperlink = string.Format("<a href=\"{0}\">{1}</a>", url, linkText);

            var url2 = $"{Request.Scheme}://{Request.Host}/Home/About";




            /* PasswordReset.cs'de SendGridClient --> Task Execute(string link, string emailAdress) kısmında yapılmış...*/


            if (TempData["Deleted"] == null)
             {
                 ModelState.Remove("Category_of_FoodPicture");
                 ModelState.Remove("Category_of_FoodDTOs");
                 ModelState.Remove("JavascriptToRun");


                 if (ModelState.IsValid)
                 {
                     Category_of_Food ctg = cvm_post.Category_of_FoodDTO.Adapt<Category_of_Food>();

                     ctg.Status = (int)cvm_post.Category_of_FoodDTO.Status;



                     //////
                     ///
                     if (Category_of_FoodPicture != null && Category_of_FoodPicture.Length > 0)
                     {
                         var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Category_of_FoodPicture.FileName); // path oluşturma

                         var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Category_of_FoodPicture", fileName); // server'a kayıt edilecek path => wwwroot/UserPicture/fileName

                         // kayıt işlemi
                         using (var stream = new FileStream(path, FileMode.Create))
                         {
                             await Category_of_FoodPicture.CopyToAsync(stream); // userPicture'ı, stream'e kayıt

                             ctg.Category_of_FoodPicture = "/Category_of_FoodPicture/" + fileName;   // veritabanına kayıt (wwwroot belirtmeye gerek yok)

                         }
                     }
                     else
                     {
                         Category_of_Food ctgv2 = await _icm.GetByIdAsync(cvm_post.Category_of_FoodDTO.ID);

                         if (ctgv2 != null)
                         {
                             if (ctgv2.Category_of_FoodPicture != null)
                             {
                                 ctg.Category_of_FoodPicture = ctgv2.Category_of_FoodPicture;
                             }
                         }

                     }

                     if (ctg.ID == 0)
                     {
                         await _icm.AddAsync(ctg);
                         TempData["messageCategory_of_Food"] = "Kategori eklendi";
                     }
                     else
                     {
                         _icm.Update(ctg);
                         // yapılacak ödev:  Category_of_Food pasife çekilirse productları da pasife çekilsin!!! Update metodu içerisinde yapılabilir... ekstra metoda gerek yok

                         /*
                          * 
                          Fonksiyon, belirli bir görevi gerçekleştirmek için bir dizi talimat veya prosedürdür. 

                         Metot ise bir NSENEYLE ilişkili bir dizi talimattır. 

                         Bir fonksiyon herhangi bir nesneye ihtiyaç duymaz ve bağımsızdır, 
                         metot ise herhangi bir nesneyle bağlantılı bir işlevdir. 

                         Metotlar, OOP (Nesne Yönelimli Programlama) ile ilgili bir kavram  --> _icm nesnesi İLE Update Metodu gibi

                          Bu yuzden methodlar classlar icinde define edilir ve obje varyasyonlari ile kullanilir. Functionlarda class icinde define edilir ama o classa ait seyler icermez, objeye dependent olmaz. 

                         Yani soyle bir sey dusunulebilir, bir dog classi, havlamak diye bir METHOD icerir, cunku sadece kopekler havlar, bu yuzden kopek objesine ihtiyac vardir.

 Fakat ayni zamanda bir human classi olsun, diyelim ki beslenmek diye bir FONKSIYON yazilacak. Cunku sart su, beslenmeyi kopek de insan da yapabilir, e bu yuzden particular bir class ihtiyaci dogurmaz. 


                          */

            TempData["messageCategory_of_Food"] = "Kategori güncellendi";

                    }

                    return RedirectToAction("Category_of_FoodList");
                }

            }
            else
            {
                _icm.Delete(await _icm.GetByIdAsync(cvm_post.Category_of_FoodDTO.ID));

                // Category_of_Food ctg = cdto.Adapt<Category_of_Food>();

                // _icm.Delete(ctg);
                TempData["messageCategory_of_Food"] = "Kategori silindi";

                TempData["Deleted"] = null;

                return RedirectToAction("Category_of_FoodList");
            }

            // TempData["mesaj"] = "Kategori adı ve statü giriniz..";
            // ModelState.AddModelError("", "Ürün adı ve statü giriniz..");

            Category_of_FoodVM cVM = new Category_of_FoodVM();
            HttpContext.Session.SetObject("manipulatedData", cvm_post.Category_of_FoodDTO);

            TempData["JavascriptToRun"] = "valid";
            TempData["HttpContext"] = "valid";

            if (cvm_post.Category_of_FoodDTO.ID != 0) //update
            {
                cVM.JavascriptToRun = $"ShowErrorUpdateOperationPopup({cvm_post.Category_of_FoodDTO.ID})";
                return RedirectToAction("Category_of_FoodList", new { JSpopupPage = cVM.JavascriptToRun });

            }
            else // add // (pvm_post.ProductDTO.ID == 0) çevir...
            {
                // pvm.JavascriptToRun = $"ShowErrorPopup( {pvm_post.ProductDTO} )";

                // pvm.JavascriptToRun = $"ShowErrorInsertOperationPopup()";

                TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";
                return RedirectToAction("Category_of_FoodList", new { JSpopupPage = TempData["JSpopupPage"].ToString() });
            }


        }




    }
}
