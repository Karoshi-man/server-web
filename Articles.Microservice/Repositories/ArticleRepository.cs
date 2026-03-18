using Articles.Microservice.Data;
using Articles.Microservice.Models;
using Microsoft.EntityFrameworkCore;

namespace Articles.Microservice.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ArticlesContext _context;
        public ArticleRepository(ArticlesContext context) { _context = context; }

        public async Task<IEnumerable<Article>> GetAllAsync() => await _context.Articles.ToListAsync();

        public async Task<Article> GetByIdAsync(int id) => await _context.Articles.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

        public async Task<Article> AddAsync(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article;
        }
    }
}