using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace lab1.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Display(Name = "Name & Surname")]
        [Required(ErrorMessage = "Please enter your full name")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string FullName { get; set; }

        [Display(Name = "Username")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string? Nickname { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100)]
        public string Email { get; set; }

        [Display(Name = "Bio")]
        [DataType(DataType.MultilineText)]
        [StringLength(1000, ErrorMessage = "Bio is too long (maximum 1000 characters)")]
        public string? Bio { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please specify your date of birth")]
        public DateTime DateOfBirth { get; set; }

        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }

        [Display(Name = "Author Rating")]
        public double Rating { get; set; } = 0.0;

        public ICollection<AuthorRating>? Ratings { get; set; }

        public ICollection<ArticleAuthor>? ArticleAuthors { get; set; }
    }
}