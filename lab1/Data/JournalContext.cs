using lab1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace lab1.Data
{
    public class JournalContext : IdentityDbContext<IdentityUser>
    {
        public JournalContext(DbContextOptions<JournalContext> options) : base(options)
        {
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<AuthorRating> AuthorRatings { get; set; }
        public DbSet<ArticleAuthor> ArticleAuthors { get; set; }
        public DbSet<ArticleRating> ArticleRatings { get; set; }
        public DbSet<CoAuthorInvitation> CoAuthorInvitations { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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