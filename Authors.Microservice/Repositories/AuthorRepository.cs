using Authors.Microservice.Data;
using Authors.Microservice.Models;
using Microsoft.EntityFrameworkCore;

namespace Authors.Microservice.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AuthorsContext _context;

        public AuthorRepository(AuthorsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAllAsync() => await _context.Authors.ToListAsync();

        public async Task<Author> GetByIdAsync(int id) => await _context.Authors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

        public async Task<Author> AddAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task UpdateAsync(Author author)
        {
            _context.Entry(author).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) => await _context.Authors.AnyAsync(e => e.Id == id);
    }
}