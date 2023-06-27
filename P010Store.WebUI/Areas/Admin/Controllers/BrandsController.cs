using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P010Store.Entities;
using P010Store.Service.Abstract;
using P010Store.WebUI.Utils;

namespace P010Store.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class BrandsController : Controller
    {
        private readonly IService<Brand> _service; // veritabanı işlemleri için generic olarak tasarladığımız repository sınıfını kullanan service interface ini brand class ı için

        public BrandsController(IService<Brand> service)
        {
            _service = service;
        }

        // kullanılmak üzere tanımladık

        // GET: BrandsController
        public ActionResult Index()
        {
            //var model = _service.GetAll();  Alttaki ile aynı
            List<Brand> brands = _service.GetAll();
            return View(brands);
        }

        // GET: BrandsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BrandsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BrandsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Brand brand, IFormFile? Logo)
        {
            if (ModelState.IsValid) // Model class ımız olan brand nesnesinin validasyonu için koyduğumuz kurallara (örneğin marka adı required-boş geçilemez gibi) uyulmuşsa
            {
                try
                {
                    brand.Logo = await FileHelper.FileLoaderAsync(Logo);
                    _service.Add(brand);
                    _service.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu");
                }
            }

            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create2(Brand brand, IFormFile? Logo)
        {
            if (ModelState.IsValid) // Model class ımız olan brand nesnesinin validasyonu için koyduğumuz kurallara (örneğin marka adı required-boş geçilemez gibi) uyulmuşsa
            {
                try
                {
                    brand.Logo = await FileHelper.FileLoaderAsync(Logo);
                    _service.Add(brand);
                    _service.SaveChanges();
                    return RedirectToAction("Create", "Products");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu");
                }
            }

            return View(brand);
        }

        // GET: BrandsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _service.FindAsync(id);
            return View(model);
        }

        // POST: BrandsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Brand brand, IFormFile? Logo)
        {

            if (ModelState.IsValid) // Model class ımız olan brand nesnesinin validasyonu için koyduğumuz kurallara (örneğin marka adı required-boş geçilemez gibi) uyulmuşsa
            {
                try
                {
                    if (Logo is not null)
                    {
                        brand.Logo = await FileHelper.FileLoaderAsync(Logo);
                    }
                    _service.Update(brand);
                    _service.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu");
                }
            }

            return View(brand);
        }

        // GET: BrandsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _service.FindAsync(id);
            return View(model);
        }

        // POST: BrandsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Brand brand)
        {
            try
            {
                FileHelper.FileRemover(brand.Logo);//kayıdı silerken resmi de silmemize yarıyor
                _service.Delete(brand);
                _service.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
