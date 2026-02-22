using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace lab1.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "The 'Title' field is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The title must be between 3 and 100 characters")]
        public string Title { get; set; }

        [Display(Name = "Content")]
        [Required(ErrorMessage = "You forgot to write the article content")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Publication Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please specify the publication date")]
        public DateTime PublicationDate { get; set; } = DateTime.Now;

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please specify a category")]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
        public string Category { get; set; }

        [Display(Name = "Rating")]
        public double Rating { get; set; } = 0.0;

        public ICollection<ArticleRating>? Ratings { get; set; }

        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
        public bool IsDraft { get; set; } = false;
        public ICollection<ArticleAuthor>? ArticleAuthors { get; set; }
    }
}