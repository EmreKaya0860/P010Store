using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P010Store.Entities;
using P010Store.WebAPIUsing.Utils;

namespace P010Store.WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class CarouselController : Controller
    {
        private readonly HttpClient _httpClient;

        private readonly string _apiAdres = "https://localhost:7083/api/Carousel";
        public CarouselController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: CarouselController
        public async Task<ActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Carousel>>(_apiAdres);
            return View(model);
        }

        // GET: CarouselController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CarouselController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarouselController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Carousel carousel, IFormFile? Image)
        {
            if (ModelState.IsValid) // Model class ımız olan brand nesnesinin validasyonu için koyduğumuz kurallara (örneğin marka adı required-boş geçilemez gibi) uyulmuşsa
            {
                try
                {
                    carousel.Image = await FileHelper.FileLoaderAsync(Image);
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres, carousel);
                    if (response.IsSuccessStatusCode) // Eğer response değişkeni api dan IsSuccessStatusCode yani başarılı bir durum kodu dönmüş ise sayfayı index e yönlendir
                    {
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu");
                }
            }

            return View(carousel);
        }

        // GET: CarouselController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Carousel>(_apiAdres + "/" + id);
            return View(model);
        }

        // POST: CarouselController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Carousel carousel, IFormFile? Image)
        {

            if (ModelState.IsValid) // Model class ımız olan brand nesnesinin validasyonu için koyduğumuz kurallara (örneğin marka adı required-boş geçilemez gibi) uyulmuşsa
            {
                try
                {
                    if (Image is not null)
                    {
                        carousel.Image = await FileHelper.FileLoaderAsync(Image);
                    }

                    var cevap = await _httpClient.PutAsJsonAsync(_apiAdres + "/" + id, carousel);

                    if (cevap.IsSuccessStatusCode) // Eğer response değişkeni api dan IsSuccessStatusCode yani başarılı bir durum kodu dönmüş ise sayfayı index e yönlendir
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu");
                }
            }

            return View(carousel);
        }

        // GET: CarouselController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Carousel>(_apiAdres + "/" + id);
            return View(model);
        }

        // POST: CarouselController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Carousel carousel)
        {
            try
            {
                var sonuc = await _httpClient.DeleteAsync(_apiAdres + "/" + id);
                if (sonuc.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            return View(carousel);
        }
    }
}
