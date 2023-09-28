using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BitWriters.API.Models.DTO;
using BitWriters.API.Models.Domain;
using BitWriters.API.Data;

namespace BitWriters.API.Controllers
{
    //https://localhost:xxxx/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public CategoriesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        //
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            //Converting/mapping DTO to Domain model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };

            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            //Domain model to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok(response);
        }
    }
}
