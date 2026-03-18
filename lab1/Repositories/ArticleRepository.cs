using lab1.Data;
using lab1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lab1.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly JournalContext _context;

        public ArticleRepository(JournalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<Article> GetByIdAsync(int id)
        {
            return await _context.Articles.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Article> AddAsync(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task UpdateAsync(Article article)
        {
            _context.Entry(article).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Articles.AnyAsync(e => e.Id == id);
        }
    }
}