using lab1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lab1.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author> GetByIdAsync(int id);
        Task<Author> AddAsync(Author author);
        Task UpdateAsync(Author author);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}