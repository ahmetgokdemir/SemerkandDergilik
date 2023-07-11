﻿using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Models;
using System.Data;
using Technosoft_Project.CommonTools;
using Technosoft_Project.Enums;
using Technosoft_Project.ViewModels;
using Technosoft_Project.VMClasses;
using static Project.DAL.Repositories.Concretes.FoodRepository;

namespace Technosoft_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Menu")]
    [Authorize(Roles = "Admin")] // case sensitive  
    public class MenuController : Controller
    {
        [Route("MenuIndex")]
        public IActionResult Index()
        {
            return View();
        }

        readonly IMenuManager _imm;
        readonly IMenuDetailManager _imdm;
        readonly ICategory_of_FoodManager _icm;
        readonly IFoodManager _ifm;


        public MenuController(IMenuManager imm, IMenuDetailManager imdm, ICategory_of_FoodManager icm, IFoodManager ifm) // services.AddRepManServices(); 
        {
            _imm = imm;
            _imdm = imdm;
            _icm = icm;
            _ifm = ifm;
        }

        [Route("MenuDetailList")]
        public async Task<IActionResult> MenuDetailList(int id, string menuName/*, int? categoryid*/)
        {
            
            TempData["Menu_ID"] = id; // menuid
            TempData["Menu_Name"] = menuName;
            int menu_id = id;

            // Menü Yemekleri
            IEnumerable<object> Menu_Foods = await _imdm.Get_FoodsofMenu_Async(id);

            // Menü Kategorileri (Distinct edilmiş)
            IEnumerable<object> Menu_Categories = await _imdm.Get_CategoriesofMenu_Async(id);

            // All category
            IEnumerable<Category_of_Food> AllCategories = await _icm.GetActivesAsync(); 


            Dictionary<int, string> bos = new Dictionary<int, string>();

            /*
            
            Dropdown list kısmında backend olmadı client tarafta append kullanıldı
            List<string> pdto = new List<string>();
            var result = new List<string>();

           //int cid = 0; // categoryid nullable olduğu için cid kullanıldı... -->   FoodNames = categoryid == null ? bos : FoodList, burada patlıyor

            //if (categoryid != null)
            //{
            //    cid = (int)categoryid;
            //    result = HttpContext.Session.GetObject<List<string>>("manipulatedData");
            //    pdto = result;
            //}
            //else
            //{
            //    bos = null;
            //}
            */

            // Category_of_FoodDTO cdto = new Category_of_FoodDTO();
            // List<string> FoodList = new List<string>();


            //if (categoryid != null)
            //{
            //    IEnumerable<string> Category_of_FoodNames = await _ifm.GetActivesFoodNamesByCategory_of_FoodIDAsync((int)categoryid);
            //    FoodList = Category_of_FoodNames.Adapt<IEnumerable<string>>().ToList();

            //    cid = (int)categoryid;

            //}
            //else
            //{
            //    bos = new List<string>() { "asdasd", "asdasdasd" };
            //    //           bos = new List<string>();    
            //    cid = 0;
            //}      

            


            MenuDetailVM mvm = new MenuDetailVM
            {
                MenuDetailDTOs = Menu_Foods.Adapt<IEnumerable<MenuDetailDTO>>().ToList(),

                Categories_of_Menu_DTOs = Menu_Categories.Adapt<IEnumerable<Category_of_FoodDTO>>().ToList(), 

                Categories_of_AllFoods_DTOs = AllCategories.Adapt<IEnumerable<Category_of_FoodDTO>>().ToList(),
                menu_id = id,

                // FoodNames = cid == 0 ? bos : pdto,
                // FoodNames = bos
            };


            HttpContext.Session.SetObject("manipulatedData", mvm.MenuDetailDTOs);
            HttpContext.Session.SetObject("manipulatedData2", mvm.Categories_of_Menu_DTOs);
            HttpContext.Session.SetObject("manipulatedData3", mvm.Categories_of_AllFoods_DTOs);
            HttpContext.Session.SetObject("manipulatedData4", mvm.menu_id);
            // HttpContext.Session.SetObject("manipulatedData", mvm.Categories_of_Menu_DTOs);


            return View(mvm);
        }

        // Get
        [Route("Get_FoodsbyCategoryID_Ajax")]
        public async Task<Dictionary<int,string>> Get_FoodsbyCategoryID_Ajax(int id, string name)
        {
            // async Task<IActionResult>


            //IEnumerable<Food> FoodEnumerableList = await _ifm.GetActivesFoodsByCategory_of_FoodIDAsync(id);

            //FoodVM fvm_post = new FoodVM
            //{
            //    FoodDTOs = FoodEnumerableList.Adapt<IEnumerable<FoodDTO>>().ToList()            
            //};

            // TempData["HttpContext"] = "valid";
            // HttpContext.Session.SetObject("manipulatedData", fvm_post.FoodDTOs);

            // Category_of_Food selectedCategory = await _icm.FirstOrDefault(x => x.ID == id);
            // HttpContext.Session.SetObject("md2", selectedCategory);

            List<FoodDTO> FoodList = new List<FoodDTO>();
 
           // Category_of_Food category =  await _icm.GetByIdAsync(id);
           TempData["Selected_Category_Name"] = name;

           // Kategorinin yemekleri
           IEnumerable<Food> Category_Foods = await _ifm.Get_FoodsByCategoryID_Async((int)id);
                
           FoodList = Category_Foods.Adapt<IEnumerable<FoodDTO>>().ToList();
            // GetFoodsByCategoryID_Async
            // GetActivesFoodsByCategory_of_FoodIDAsync

            Dictionary<int, string> food_items = new Dictionary<int, string>();
           //List<string> food_items = new List<string>();


            for (int i = 0; i < FoodList.Count; i++)
            {
                food_items.Add(FoodList[i].ID,FoodList[i].FoodName);
            }

            ///////////////////////////////////////////////
            //string menunamed = (string)TempData["Menu_Name"];
            //return View("MenuDetailList", mvm);
            // return RedirectToAction("MenuDetailList", new { id = (int)TempData["Menu_ID"], menuName = menunamed, categoryid = id });

            ViewBag.food_items_IDs = new SelectList(food_items.Keys);

            return food_items;


        }

        // Add Food to Menu
        [Route("AddFoodtoMenu")]
        public async Task<IActionResult> AddFoodtoMenu(MenuDetailVM mvm_post)
        {
            ModelState.Remove("MenuDetailDTOs");
            ModelState.Remove("MenuDetailDTO");
            // ModelState.Remove("FoodDTOs");
            ModelState.Remove("JavascriptToRun");
            ModelState.Remove("Categories_of_Menu_DTOs");
            ModelState.Remove("Categories_of_AllFoods_DTOs");
 ;

            //mvm_post.MenuDetailDTO.CategoryName_of_Food = TempData["Selected_Category_Name"].ToString();
            //mvm_post.MenuDetailDTO.MenuID = (int) TempData["Menu_ID"];
            //mvm_post.MenuDetailDTO.FoodID = 
            if (ModelState.IsValid)
            {
                //string selected_food_name = mvm_post._foodList.Values.ToList()[0];
                //int selected_food_ID = mvm_post._foodList.Keys.ToList()[0];

                int selected_foodID = mvm_post._foodList_ID[0];
                string category_Name = mvm_post._categoryList[0].ToString();
                int menu_ID = mvm_post.menu_id;

                bool food_exists = await _imdm.IsExist_FoodinMenu_Async(selected_foodID, menu_ID);

                if (!food_exists)
                {
                    //add selected food
                    return null;
                }

                else
                {
                    // selected food all ready on menu 
                    ModelState.AddModelError("", "Seçili yemek menüde bulunmaktadır! Başka bir ürün seçiniz."); 
                    return null;

                }
            }

            ModelState.AddModelError("", "Kategori ve yemek tercihi yapınız...");

            MenuDetailVM mvm = new MenuDetailVM
            {
                //MenuDetailDTOs = Menu_Foods.Adapt<IEnumerable<MenuDetailDTO>>().ToList(),

                //Categories_of_Menu_DTOs = Menu_Categories.Adapt<IEnumerable<Category_of_FoodDTO>>().ToList(),

                //Categories_of_AllFoods_DTOs = AllCategories.Adapt<IEnumerable<Category_of_FoodDTO>>().ToList(),
                _foodList_ID = mvm_post._foodList_ID,
                _categoryList = mvm_post._categoryList,
                menu_id = mvm_post.menu_id,

                // FoodNames = cid == 0 ? bos : pdto,
                // FoodNames = bos
            };

            // HttpContext.Session.SetObject("manipulatedData", null);

            return View("MenuDetailList", mvm);
        }


        [Route("MenuList")]
        public async Task<IActionResult> MenuList(string? JSpopupPage)
        {

            if (TempData["JavascriptToRun"] == null)
            {
                JSpopupPage = null; // pop-up sıfırlanır yoksa sayfayı reflesleyince geliyor
            }

            IEnumerable<Menu> MenuList = await _imm.GetActivesAsync();

            MenuVM mvm = new MenuVM
            {
                MenuDTOs = MenuList.Adapt<IEnumerable<MenuDTO>>().ToList(),
                JavascriptToRun = JSpopupPage

            };
            
            return View(mvm);  
        }

        [Route("AddMenuAjax")]
        public PartialViewResult AddMenuAjax()
        {
            // ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status))); => yerine                                                                 asp-items="Html.GetEnumSelectList<Technosoft_Project.Enums.Status>()" kullanıldı..

            var result = new MenuDTO();
            MenuDTO mDTO = new MenuDTO();

            // HttpContext.Session.SetObject("manipulatedData", pvm_post.MenuDTO);
            if (TempData["HttpContext"] != null)
            {
                //mDTO = new MenuDTO();
                result = HttpContext.Session.GetObject<MenuDTO>("manipulatedData");
                mDTO = result;

            }

            MenuVM mVM = new MenuVM
            {
                MenuDTO = mDTO
            };

            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(mVM.MenuDTO.Menu_Name))
                {
                    ModelState.AddModelError("MenuDTO.MenuName", "Menü adı giriniz.");
                }

                HttpContext.Session.SetObject("manipulatedData", null);
            }

            Thread.Sleep(500); // pop-up sayfasını tekrar açmayı tetikleyince bazen gelmiyor o yüzden bu kod eklendi..

            return PartialView("_CrudMenuPartial", mVM);
        }

        [Route("UpdateMenuAjax")]
        public async Task<PartialViewResult> UpdateMenuAjax(int id)
        {
            // Menu Menu_item = await _icm.GetByIdAsync(id);
            // MenuDTO cDTO = Menu_item.Adapt<MenuDTO>();

            // ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status))); => yerine                                                                 asp-items="Html.GetEnumSelectList<Technosoft_Project.Enums.Status>()" kullanıldı..


            MenuDTO cDTO = new MenuDTO();

            var result = new MenuDTO();

            if (TempData["HttpContext"] != null)
            {
                result = HttpContext.Session.GetObject<MenuDTO>("manipulatedData");
                cDTO = result;

                // HttpContext.Session.SetObject("manipulatedData", null);
            }
            else
            {
                Menu Menu_item = await _imm.GetByIdAsync(id);
                cDTO = Menu_item.Adapt<MenuDTO>();
                // cdto = Food_item.Adapt<MenuDTO>();
            }


            MenuVM cVM = new MenuVM
            {
                MenuDTO = cDTO
            };

            if (TempData["HttpContext"] != null)
            {
                TempData["HttpContext"] = null;

                if (string.IsNullOrEmpty(cVM.MenuDTO.Menu_Name))
                {
                    ModelState.AddModelError("MenuDTO.MenuName", "Menü adı giriniz.");
                }

                HttpContext.Session.SetObject("manipulatedData", null);
            }

            Thread.Sleep(500);

            return PartialView("_CrudMenuPartial", cVM);
        }


        [Route("DeleteMenuAjax")]
        public async Task<PartialViewResult> DeleteMenuAjax(int id)
        {
            Menu Menu_item = await _imm.GetByIdAsync(id);
            MenuDTO cDTO = Menu_item.Adapt<MenuDTO>();

            //ViewBag.Status = new SelectList(Enum.GetNames(typeof(Status)));
            ViewBag.MenuNameDelete = cDTO.Menu_Name;

            ViewBag.CRUD = "delete_operation";

            MenuVM cVM = new MenuVM
            {
                MenuDTO = cDTO
            };

            return PartialView("_CrudMenuPartial", cVM);
        }

        [Route("CRUDMenu")]
        [HttpPost]
        public async Task<IActionResult> CRUDMenu(MenuVM mvm_post)
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
                ModelState.Remove("MenuDTOs");
                ModelState.Remove("JavascriptToRun");
                ModelState.Remove("MenuDTO.FoodName");
                

                if (ModelState.IsValid)
                {
                    Menu ctg = mvm_post.MenuDTO.Adapt<Menu>();

                    ctg.Status = (int)mvm_post.MenuDTO.Status;



                    //////
                    ///


                    if (ctg.ID == 0)
                    {
                        await _imm.AddAsync(ctg);
                        TempData["messageMenu"] = "Kategori eklendi";
                    }
                    else
                    {
                        _imm.Update(ctg);
                        // yapılacak ödev:  Menu pasife çekilirse Foodları da pasife çekilsin!!! Update metodu içerisinde yapılabilir... ekstra metoda gerek yok

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

                        TempData["messageMenu"] = "Kategori güncellendi";

                    }

                    return RedirectToAction("MenuList");
                }

            }
            else
            {
                _imm.Delete(await _imm.GetByIdAsync(mvm_post.MenuDTO.ID));

                // Menu ctg = cdto.Adapt<Menu>();

                // _icm.Delete(ctg);
                TempData["messageMenu"] = "Kategori silindi";

                TempData["Deleted"] = null;

                return RedirectToAction("MenuList");
            }

            // TempData["mesaj"] = "Kategori adı ve statü giriniz..";
            // ModelState.AddModelError("", "Ürün adı ve statü giriniz..");

            MenuVM cVM = new MenuVM();
            HttpContext.Session.SetObject("manipulatedData", mvm_post.MenuDTO);

            TempData["JavascriptToRun"] = "valid";
            TempData["HttpContext"] = "valid";

            if (mvm_post.MenuDTO.ID != 0) //update
            {
                cVM.JavascriptToRun = $"ShowErrorUpdateOperationPopup({mvm_post.MenuDTO.ID})";
                return RedirectToAction("MenuList", new { JSpopupPage = cVM.JavascriptToRun });

            }
            else // add // (pvm_post.FoodDTO.ID == 0) çevir...
            {
                // pvm.JavascriptToRun = $"ShowErrorPopup( {pvm_post.FoodDTO} )";

                // pvm.JavascriptToRun = $"ShowErrorInsertOperationPopup()";

                TempData["JSpopupPage"] = $"ShowErrorInsertOperationPopup()";
                return RedirectToAction("MenuList", new { JSpopupPage = TempData["JSpopupPage"].ToString() });
            }


        }


    }
}
