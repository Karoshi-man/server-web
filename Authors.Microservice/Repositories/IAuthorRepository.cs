using Authors.Microservice.Models;

namespace Authors.Microservice.Repositories
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