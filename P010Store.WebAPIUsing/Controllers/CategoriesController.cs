using Microsoft.AspNetCore.Mvc;
using P010Store.Entities;

namespace P010Store.WebAPIUsing.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoriesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string _apiAdres = "https://localhost:7083/api/Categories";


        public async Task<IActionResult> Index(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Category>(_apiAdres + "/" + id);
            return View(model);
        }
    }
}
