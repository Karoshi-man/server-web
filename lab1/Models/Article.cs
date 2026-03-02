using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace lab1.Models
{
    public class Article
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public IdentityUser? User { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 150 characters.")]
        public string Title { get; set; }

        [Display(Name = "Content")]
        [Required(ErrorMessage = "Content cannot be empty.")]
        [MinLength(50, ErrorMessage = "Article content is too short. Please write at least 50 characters.")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please select a category.")]
        [StringLength(50)]
        public string Category { get; set; }

        [Display(Name = "Publication Date")]
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [Display(Name = "Article Rating")]
        [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10.")]
        public double Rating { get; set; } = 0.0;
        public bool IsDeleted { get; set; } = false;

        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }
        public bool IsDraft { get; set; } = false;
        public virtual ICollection<ArticleAuthor> ArticleAuthors { get; set; } = new List<ArticleAuthor>();
        public virtual ICollection<ArticleRating> Ratings { get; set; } = new List<ArticleRating>();
        public virtual ICollection<CoAuthorInvitation> Invitations { get; set; } = new List<CoAuthorInvitation>();
    }
}