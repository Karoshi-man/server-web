using Authors.Microservice.Models;
using Microsoft.EntityFrameworkCore;

namespace Authors.Microservice.Data
{
    public class AuthorsContext : DbContext
    {
        public AuthorsContext(DbContextOptions<AuthorsContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
    }
}