using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Identity_Models;
using Project.ENTITIES.Models;
using Technosoft_Project.CommonTools;
using Technosoft_Project.ViewModels;
using Technosoft_Project.VMClasses;

namespace Technosoft_Project.Controllers
{
    public class CategoryofFoodController : BaseController
    {
        readonly ICategoryofFoodManager _icm;

        public CategoryofFoodController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ICategoryofFoodManager icm) : base(userManager, null, roleManager)
        {
            _icm = icm;
        }

        //[Route("CategoryofFoodIndex")]
        //public IActionResult Index()
        //{
        //    return View();
        //}


        [Route("CategoryofFoodList")]
        public async Task<IActionResult> CategoryofFoodList(string? JSpopupPage)
        {

            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null; // pop-up sıfırlanır yoksa sayfayı reflesleyince geliyor
            }

            IEnumerable<CategoryofFood> CategoryofFoodList = await _icm.GetActivesAsync();

            CategoryofFoodVM cvm = new CategoryofFoodVM
            {
                CategoryofFoodDTOs = CategoryofFoodList.Adapt<IEnumerable<CategoryofFoodDTO>>().ToList(),
                JavascriptToRun = JSpopupPage
            };

            return View(cvm);
        }

        [Route("AddCategoryofFoodAjax")]
        public PartialViewResult AddCategoryofFoodAjax()
        {
            // ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status))); => yerine                                                                 asp-items="Html.GetEnumSelectList<Technosoft_Project.Enums.Status>()" kullanıldı..

            var result = new CategoryofFoodDTO();
            CategoryofFoodDTO cDTO = new CategoryofFoodDTO();

            // HttpContext.Session.SetObject("manipulatedData", pvm_post.CategoryofFoodDTO);
            if (TempData["HttpContext"] != null)
            {
                cDTO = new CategoryofFoodDTO();
                result = HttpContext.Session.GetObject<CategoryofFoodDTO>("manipulatedData");
                cDTO = result;

            }

            CategoryofFoodVM cVM = new CategoryofFoodVM
            {
                CategoryofFoodDTO = cDTO
            };

            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(cVM.CategoryofFoodDTO._CategoryName_of_Foods))
                {
                    ModelState.AddModelError("CategoryofFoodDTO.CategoryofFoodName", "Kategori adı giriniz.");
                }

                HttpContext.Session.SetObject("manipulatedData", null);
            }

            Thread.Sleep(500); // pop-up sayfasını tekrar açmayı tetikleyince bazen gelmiyor o yüzden bu kod eklendi..

            return PartialView("_CrudCategoryofFood_Partial", cVM);
        }




        [Route("UpdateCategoryofFoodAjax")]
        public async Task<PartialViewResult> UpdateCategoryofFoodAjax(int id)
        {
            // CategoryofFood CategoryofFood_item = await _icm.GetByIdAsync(id);
            // CategoryofFoodDTO cDTO = CategoryofFood_item.Adapt<CategoryofFoodDTO>();

            // ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status))); => yerine                                                                 asp-items="Html.GetEnumSelectList<Technosoft_Project.Enums.Status>()" kullanıldı..


            CategoryofFoodDTO cDTO = new CategoryofFoodDTO();

            var result = new CategoryofFoodDTO();

            if (TempData["HttpContext"] != null)
            {
                result = HttpContext.Session.GetObject<CategoryofFoodDTO>("manipulatedData");
                cDTO = result;

                // HttpContext.Session.SetObject("manipulatedData", null);
            }
            else
            {
                CategoryofFood CategoryofFood_item = await _icm.GetByIdAsync(id);
                cDTO = CategoryofFood_item.Adapt<CategoryofFoodDTO>();
                // cdto = Food_item.Adapt<CategoryofFoodDTO>();
            }




            CategoryofFoodVM cVM = new CategoryofFoodVM
            {
                CategoryofFoodDTO = cDTO
            };



            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(cVM.CategoryofFoodDTO._CategoryName_of_Foods))
                {
                    ModelState.AddModelError("CategoryofFoodDTO.CategoryofFoodName", "Kategori adı giriniz.");
                }

                HttpContext.Session.SetObject("manipulatedData", null);
            }

            Thread.Sleep(500);

            return PartialView("_CrudCategoryofFood_Partial", cVM);
        }


        [Route("DeleteCategoryofFoodAjax")]
        public async Task<PartialViewResult> DeleteCategoryofFoodAjax(int id)
        {
            CategoryofFood CategoryofFood_item = await _icm.GetByIdAsync(id);
            CategoryofFoodDTO cDTO = CategoryofFood_item.Adapt<CategoryofFoodDTO>();

            //ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));
            ViewBag.CategoryofFoodNameDelete = cDTO._CategoryName_of_Foods;

            ViewBag.CRUD = "delete_operation";

            CategoryofFoodVM cVM = new CategoryofFoodVM
            {
                CategoryofFoodDTO = cDTO
            };

            return PartialView("_CrudCategoryofFood_Partial", cVM);
        }



        [Route("CRUDCategoryofFood")]
        [HttpPost]
        public async Task<IActionResult> CRUDCategoryofFood(CategoryofFoodVM cvm_post, IFormFile CategoryofFoodPicture)
        {

            /*
              var urlHelper = new UrlHelper(ControllerContext);
              var url = urlHelper.Action("About", "Home");
              var linkText = "Panelden yapılan değiliklik web e yansımıyor";
              
              var hyperlink = string.Format("<a href=\"{0}\">{1}</a>", url, linkText);
              
              var url2 = $"{Request.Scheme}://{Request.Host}/Home/About";
            */



            /* PasswordReset.cs'de SendGridClient --> Task Execute(string link, string emailAdress) kısmında yapılmış...*/


            if (TempData["Deleted"] == null)
            {
                ModelState.Remove("CategoryofFoodPicture");
                ModelState.Remove("CategoryofFoodDTOs");
                ModelState.Remove("JavascriptToRun");


                if (ModelState.IsValid)
                {
                    CategoryofFood ctg = cvm_post.CategoryofFoodDTO.Adapt<CategoryofFood>();

                    /* !!! !!! ctg.Status = (int)cvm_post.CategoryofFoodDTO.Status; !!! !!!*/



                    //////
                    ///
                    if (CategoryofFoodPicture != null && CategoryofFoodPicture.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(CategoryofFoodPicture.FileName); // path oluşturma

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CategoryofFoodPicture", fileName); // server'a kayıt edilecek path => wwwroot/UserPicture/fileName

                        // kayıt işlemi
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await CategoryofFoodPicture.CopyToAsync(stream); // userPicture'ı, stream'e kayıt

                            /* !!! !!! ctg.CategoryofFoodPicture = "/CategoryofFoodPicture/" + fileName;   // veritabanına kayıt (wwwroot belirtmeye gerek yok) !!! !!! */

                        }
                    }
                    else
                    {
                        CategoryofFood ctgv2 = await _icm.GetByIdAsync(cvm_post.CategoryofFoodDTO.ID);

                        if (ctgv2 != null)
                        {
                            /* !!! !!!
                             if (ctgv2.CategoryofFoodPicture != null)
                             {
                                 ctg.CategoryofFoodPicture = ctgv2.CategoryofFoodPicture;
                             }
                            !!! !!! */
                        }

                    }

                    if (ctg.ID == 0)
                    {
                        await _icm.AddAsync(ctg);
                        TempData["messageCategoryofFood"] = "Kategori eklendi";
                    }
                    else
                    {
                        _icm.Update(ctg);
                        // yapılacak ödev:  CategoryofFood pasife çekilirse Foodları da pasife çekilsin!!! Update metodu içerisinde yapılabilir... ekstra metoda gerek yok

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

                        TempData["messageCategoryofFood"] = "Kategori güncellendi";

                    }

                    return RedirectToAction("CategoryofFoodList");
                }

            }
            else
            {
                _icm.Delete(await _icm.GetByIdAsync(cvm_post.CategoryofFoodDTO.ID));

                // CategoryofFood ctg = cdto.Adapt<CategoryofFood>();

                // _icm.Delete(ctg);
                TempData["messageCategoryofFood"] = "Kategori silindi";

                TempData["Deleted"] = null;

                return RedirectToAction("CategoryofFoodList");
            }

            // TempData["mesaj"] = "Kategori adı ve statü giriniz..";
            // ModelState.AddModelError("", "Ürün adı ve statü giriniz..");

            CategoryofFoodVM cVM = new CategoryofFoodVM();
            HttpContext.Session.SetObject("manipulatedData", cvm_post.CategoryofFoodDTO);

            TempData["JavascriptToRun"] = "valid";
            TempData["HttpContext"] = "valid";

            if (cvm_post.CategoryofFoodDTO.ID != 0) //update
            {
                cVM.JavascriptToRun = $"ShowErrorUpdateOperationPopup({cvm_post.CategoryofFoodDTO.ID})";
                return RedirectToAction("CategoryofFoodList", new { JSpopupPage = cVM.JavascriptToRun });

            }
            else // add // (pvm_post.FoodDTO.ID == 0) çevir...
            {
                // pvm.JavascriptToRun = $"ShowErrorPopup( {pvm_post.FoodDTO} )";

                // pvm.JavascriptToRun = $"ShowErrorInsertOperationPopup()";

                TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";
                return RedirectToAction("CategoryofFoodList", new { JSpopupPage = TempData["JSpopupPage"].ToString() });
            }


        }
    }
}
