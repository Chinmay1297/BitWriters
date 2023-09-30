using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BitWriters.API.Models.DTO;
using BitWriters.API.Models.Domain;
using BitWriters.API.Data;
using BitWriters.API.Repositories.Interface;

namespace BitWriters.API.Controllers
{
    //https://localhost:xxxx/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        //POST: /api/categories
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {
            //Converting/mapping DTO to Domain model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };

            await categoryRepository.CreateAsync(category);

            //Domain model to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok(response);
        }

        //GET: https://localhost:7163/api/Categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();

            //Map Domain model to DTO

            var response = new List<CategoryDto>();
            foreach(var category in categories) {
                response.Add(new CategoryDto
                {
                    Id=category.Id,
                    Name=category.Name,
                    UrlHandle=category.UrlHandle,
                });
            }

            return Ok(response);
        }

        //GET: https://localhost:7163/api/Categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingCategory =  await categoryRepository.GetById(id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle
            };
            return Ok(response);
        }
    }
}
