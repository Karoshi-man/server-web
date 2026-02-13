using lab1.Models;
using Microsoft.EntityFrameworkCore;
using lab1.Models;

namespace lab1.Data
{
    public class JournalContext : DbContext
    {
        public JournalContext(DbContextOptions<JournalContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleAuthor> ArticleAuthors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Many-to-Many
            modelBuilder.Entity<ArticleAuthor>()
                .HasKey(aa => new { aa.AuthorId, aa.ArticleId });

            modelBuilder.Entity<ArticleAuthor>()
                .HasOne(aa => aa.Article)
                .WithMany(a => a.ArticleAuthors)
                .HasForeignKey(aa => aa.ArticleId);

            modelBuilder.Entity<ArticleAuthor>()
                .HasOne(aa => aa.Author)
                .WithMany(a => a.ArticleAuthors)
                .HasForeignKey(aa => aa.AuthorId);
        }
    }
}