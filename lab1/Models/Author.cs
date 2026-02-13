using System.ComponentModel.DataAnnotations;

namespace lab1.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Display(Name = "ПІБ")]
        [Required(ErrorMessage = "Введіть повне ім'я автора")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ім'я має бути від 2 до 100 символів")]
        public string FullName { get; set; }

        [Display(Name = "Псевдонім")]
        [StringLength(50, ErrorMessage = "Псевдонім не може бути довшим за 50 символів")]
        public string? Nickname { get; set; } // Додав '?', бо псевдонім може бути не у всіх

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email є обов'язковим")]
        [EmailAddress(ErrorMessage = "Введіть коректну електронну пошту (наприклад, user@example.com)")]
        [StringLength(100)]
        public string Email { get; set; }

        [Display(Name = "Біографія")]
        [DataType(DataType.MultilineText)] // Робить велике вікно для тексту (textarea)
        [StringLength(1000, ErrorMessage = "Біографія занадто довга (максимум 1000 символів)")]
        public string? Bio { get; set; } // Теж може бути порожнім

        [Display(Name = "Дата народження")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Вкажіть дату народження")]
        public DateTime DateOfBirth { get; set; }

        public ICollection<ArticleAuthor>? ArticleAuthors { get; set; }
    }
}