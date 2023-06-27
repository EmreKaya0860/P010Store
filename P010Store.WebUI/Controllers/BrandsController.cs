using Microsoft.AspNetCore.Mvc;
using P010Store.Entities;
using P010Store.Service.Abstract;
using P010Store.WebUI.Models;

namespace P010Store.WebUI.Controllers
{
    public class BrandsController : Controller
    {
        private readonly IService<Brand> _serviceBrand;
        private readonly IService<Product> _serviceProduct;
        public BrandsController(IService<Brand> serviceBrand, IService<Product> serviceProduct)
        {
            _serviceBrand = serviceBrand;
            _serviceProduct = serviceProduct;
        }

        public async Task<IActionResult> Index(int id)
        {
            var model = new BrandPageViewModel()
            {
                Brand = await _serviceBrand.FindAsync(id),
                Products = await _serviceProduct.GetAllAsync(p => p.IsActive && p.BrandId == id)
            };
            return View(model);
        }
    }
}
