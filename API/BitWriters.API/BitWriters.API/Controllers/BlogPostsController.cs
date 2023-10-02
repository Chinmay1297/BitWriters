using BitWriters.API.Models.Domain;
using BitWriters.API.Models.DTO;
using BitWriters.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BitWriters.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }
        //POST: {apiBaseUrl}/api/blogposts
        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody]CreateBlogPostRequestDto requestDto)
        {
            //Convert from DTO to Domain Model
            var blogPost = new BlogPost
            {
                Title = requestDto.Title,
                Author = requestDto.Author,
                Content = requestDto.Content,
                FeaturedImageUrl = requestDto.FeaturedImageUrl,
                IsVisible = requestDto.IsVisible,
                ShortDescription = requestDto.ShortDescription,
                UrlHandle = requestDto.UrlHandle,
                PublishedDate = requestDto.PublishedDate,
                Categories = new List<Category>()
            };

            //Adding categories from requestDto to DomainModel variable
            foreach (var categoryGuid in requestDto.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);
                //Checking if its a valid category
                if(existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            blogPost = await blogPostRepository.CreateAsync(blogPost);

            //Convert from Domain Model to DTO
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Author = blogPost.Author,
                Content = blogPost.Content,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x=>new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList(),
            };

            return Ok(response);
        }

        //GET: {apiBaseUrl}/api/blogposts
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();

            //Convert Domain Model to DTO
            var response = new List<BlogPostDto>();
            foreach(var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    PublishedDate = blogPost.PublishedDate,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    Categories = blogPost.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList(),
                });
            }

            return Ok(response);
        }

        //GET: {apiBaseUrl}/api/blogposts/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute]Guid id)
        {
            var blogPost = await blogPostRepository.GetByIdAsync(id);
            if(blogPost == null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Author = blogPost.Author,
                Content = blogPost.Content,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList(),
            };

            return Ok(response);
        }

        //GET: {apibaseurl}/api/blogposts/{urlhandle}
        [HttpGet]
        [Route("{urlhandle}")]
        public async Task<IActionResult> getBlogPostByUrlHandle([FromRoute] string urlhandle)
        {
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlhandle);
            if (blogPost == null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Author = blogPost.Author,
                Content = blogPost.Content,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList(),
            };

            return Ok(response);
        }

        //PUT: {apiBaseUrl}/api/blogposts/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute]Guid id, UpdateBlogPostRequestDto requestDto)
        {
            //Map Dto to Domain Model
            var newBlogPost = new BlogPost
            {
                Id = id,
                Title = requestDto.Title,
                Author = requestDto.Author,
                Content = requestDto.Content,
                FeaturedImageUrl = requestDto.FeaturedImageUrl,
                IsVisible = requestDto.IsVisible,
                ShortDescription = requestDto.ShortDescription,
                UrlHandle = requestDto.UrlHandle,
                PublishedDate = requestDto.PublishedDate,
                Categories = new List<Category>()
            };

            //Adding categories from requestDto to DomainModel variable
            foreach (var categoryGuid in requestDto.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);
                //Checking if its a valid category
                if (existingCategory != null)
                {
                    newBlogPost.Categories.Add(existingCategory);
                }   
            }

            //Call repository to update BlogPost Model
            var updatedBlogPost = await blogPostRepository.UpdateAsync(newBlogPost);
            if (updatedBlogPost == null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = newBlogPost.Id,
                Title = newBlogPost.Title,
                Author = newBlogPost.Author,
                Content = newBlogPost.Content,
                PublishedDate = newBlogPost.PublishedDate,
                FeaturedImageUrl = newBlogPost.FeaturedImageUrl,
                IsVisible = newBlogPost.IsVisible,
                ShortDescription = newBlogPost.ShortDescription,
                UrlHandle = newBlogPost.UrlHandle,
                Categories = newBlogPost.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList(),
            };

            return Ok(response);
        }

        //DELETE: {apiBaseUrl}/api/blogposts/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute]Guid id)
        {
            var deletedBlogPost = await blogPostRepository.DeleteAsync(id);
            if(deletedBlogPost == null)
            { return NotFound(); }

            //converting domain model to dto
            var response = new BlogPostDto
            {
                Id = deletedBlogPost.Id,
                Title = deletedBlogPost.Title,
                Author = deletedBlogPost.Author,
                Content = deletedBlogPost.Content,
                PublishedDate = deletedBlogPost.PublishedDate,
                FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
                IsVisible = deletedBlogPost.IsVisible,
                ShortDescription = deletedBlogPost.ShortDescription,
                UrlHandle = deletedBlogPost.UrlHandle
            };

            return Ok(response);
        }
    }
}
