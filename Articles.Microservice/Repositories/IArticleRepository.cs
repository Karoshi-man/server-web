using Articles.Microservice.Models;

namespace Articles.Microservice.Repositories
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetAllAsync();
        Task<Article> GetByIdAsync(int id);
        Task<Article> AddAsync(Article article);
    }
}