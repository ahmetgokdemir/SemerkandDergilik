using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.ManagerServices.Abstracts;
using Project.ENTITIES.Models;
using Semerkand_Dergilik.ViewModels;
using Semerkand_Dergilik.VMClasses;
using System.Data;

namespace Semerkand_Dergilik.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Authorize(Roles = "Admin")] // case sensitive  
    public class ProductController : Controller
    {
        readonly IProductManager _ipm;

        public ProductController(IProductManager ipm) // services.AddRepManServices(); 
        {
            _ipm = ipm;
        }

        [Route("ProductIndex")]
        public IActionResult Index()
        {
            return View();
        }


        [Route("ProductList")]
        public async Task<IActionResult> ProductList(int id)
        {
            int category_id = id;

            IEnumerable<Product> productList = await _ipm.GetActivesProductsByCategoryIDAsync(category_id);

            ProductVM pvm = new ProductVM
            {
                Products = productList.Adapt<IEnumerable<ProductDTO>>().ToList(),

            };

            return View(pvm);
        }


    }
}
