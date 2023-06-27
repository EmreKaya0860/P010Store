using Microsoft.AspNetCore.Mvc;
using P010Store.Service.Abstract;
using P010Store.Service.Concrete;
using P010Store.WebUI.Models;

namespace P010Store.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _productService.GetAllAsync(p=>p.IsActive);
            return View(model);
        }

        public async Task<IActionResult> Search(string q)
        {
            var model = await _productService.GetAllAsync(p=>p.IsActive && p.Name.Contains(q));
            return View(model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productService.GetProductByCategoriesBrandsAsync(id);
            var model = new ProductDetailViewModel()
            {
                Product = product,
                Products = await _productService.GetAllAsync(p=>p.CategoryId == product.CategoryId && p.Id != id)
            };


            return View(model);
        }
    }
}
