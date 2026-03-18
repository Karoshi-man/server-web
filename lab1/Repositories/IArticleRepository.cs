using lab1.Models;

using lab1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lab1.Repositories
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetAllAsync();
        Task<Article> GetByIdAsync(int id);
        Task<Article> AddAsync(Article article);
        Task UpdateAsync(Article article);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}