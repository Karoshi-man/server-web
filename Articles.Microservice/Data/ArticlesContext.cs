using Articles.Microservice.Models;
using Microsoft.EntityFrameworkCore;

namespace Articles.Microservice.Data
{
    public class ArticlesContext : DbContext
    {
        public ArticlesContext(DbContextOptions<ArticlesContext> options) : base(options) { }

        public DbSet<Article> Articles { get; set; }
    }
}