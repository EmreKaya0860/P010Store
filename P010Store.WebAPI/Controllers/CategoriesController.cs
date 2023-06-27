using Microsoft.AspNetCore.Mvc;
using P010Store.Entities;
using P010Store.Service.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace P010Store.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<IEnumerable<Category>> Get()
        {
            return await _categoryService.GetAllAsync();
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<Category> Get(int id)
        {
            return await _categoryService.GetCategoryByProducts(id);
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<Category> Post([FromBody] Category value)
        {
            await _categoryService.AddAsync(value);
            await _categoryService.SaveChangesAsync();
            return value;
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Category value)
        {
            _categoryService.Update(value);
            _categoryService.SaveChanges();
            return NoContent();
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var kayit = _categoryService.Find(id);
            if (kayit == null)// eğer kayıt bulunamamış ise
            {
                return BadRequest(); // geriye geçersiz istek hatası dön ki işlemde sorun olduğunu anlayalım
            }
            _categoryService.Delete(kayit);
            _categoryService.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);// Kayıt başarılı ise geriye 200OK ile işlem başarılı mesajı döndür
        }
    }
}
