using BitWriters.API.Models.Domain;

namespace BitWriters.API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost?> GetByIdAsync(Guid id);
        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
    }
}
