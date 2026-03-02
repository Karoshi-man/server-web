using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using lab1.Validation;

namespace lab1.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }

        [Display(Name = "Name & Surname")]
        [Required(ErrorMessage = "Please enter your full name.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯіІїЇєЄґҐ\'\- ]+$", ErrorMessage = "Name can only contain letters, apostrophes, and hyphens. No numbers allowed.")]
        public string FullName { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        [Remote(action: "VerifyNickname", controller: "Authors", AdditionalFields = nameof(Id))]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain Latin letters, numbers, and underscores (no spaces).")]
        public string? Nickname { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email format.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address (e.g., name@example.com).")]
        public string Email { get; set; }

        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Please tell us a bit about yourself.")]
        [DataType(DataType.MultilineText)]
        [StringLength(1000, MinimumLength = 30, ErrorMessage = "Biography is too short. Please write at least 30 characters.")]
        public string? Bio { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please specify your date of birth.")]
        [SmartAgeValidation(13, 120, ErrorMessage = "You must be at least 13 years old to be an author, and please enter a valid year.")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Author Rating")]
        public double Rating { get; set; } = 0.0;

        public ICollection<AuthorRating>? Ratings { get; set; }
        public ICollection<ArticleAuthor>? ArticleAuthors { get; set; }
    }
}