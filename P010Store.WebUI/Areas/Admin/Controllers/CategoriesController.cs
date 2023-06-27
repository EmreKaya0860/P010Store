using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using P010Store.Entities;
using P010Store.Service.Abstract;
using P010Store.WebUI.Utils;

namespace P010Store.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class CategoriesController : Controller
    {
        private readonly IService<Category> _service;

        public CategoriesController(IService<Category> service)
        {
            _service = service;
        }

        // GET: CategoriesController
        public async Task<ActionResult> Index()
        {
            var model = await _service.GetAllAsync();
            return View(model);
        }

        // GET: CategoriesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoriesController/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.ParentId = new SelectList(await _service.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: CategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Category category, IFormFile? Image)
        {
            if (ModelState.IsValid) // Model class ımız olan brand nesnesinin validasyonu için koyduğumuz kurallara (örneğin marka adı required-boş geçilemez gibi) uyulmuşsa
            {
                try
                {
                    if (Image is not null) category.Image = await FileHelper.FileLoaderAsync(Image);
                    await _service.AddAsync(category);
                    await _service.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu");
                }
            }
            ViewBag.ParentId = new SelectList(await _service.GetAllAsync(), "Id", "Name");
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create2(Category category, IFormFile? Image)
        {
            if (ModelState.IsValid) // Model class ımız olan brand nesnesinin validasyonu için koyduğumuz kurallara (örneğin marka adı required-boş geçilemez gibi) uyulmuşsa
            {
                try
                {
                    if (Image is not null) category.Image = await FileHelper.FileLoaderAsync(Image);
                    await _service.AddAsync(category);
                    await _service.SaveChangesAsync();
                    return RedirectToAction("Create", "Products");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu");
                }
            }
            ViewBag.ParentId = new SelectList(await _service.GetAllAsync(), "Id", "Name");
            return View(category);
        }

        // GET: CategoriesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.ParentId = new SelectList(await _service.GetAllAsync(), "Id", "Name");
            var model = await _service.FindAsync(id);
            return View(model);
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Category category, IFormFile? Image)
        {
            try
            {
                if (Image is not null)
                {
                    category.Image = await FileHelper.FileLoaderAsync(Image);
                }
                _service.Update(category);
                await _service.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            ViewBag.ParentId = new SelectList(await _service.GetAllAsync(), "Id", "Name");
            return View(category);
        }

        // GET: CategoriesController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _service.FindAsync(id);
            return View(model);
        }

        // POST: CategoriesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Category category)
        {
            try
            {
                _service.Delete(category);
                await _service.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
