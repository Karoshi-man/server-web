using System.ComponentModel.DataAnnotations;

namespace lab1.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Display(Name = "Заголовок статті")]
        [Required(ErrorMessage = "Поле 'Заголовок' є обов'язковим")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Заголовок має бути від 3 до 100 символів")]
        public string Title { get; set; }

        [Display(Name = "Текст статті")]
        [Required(ErrorMessage = "Ви забули написати текст статті")]
        [DataType(DataType.MultilineText)] // Робить велике поле для вводу
        public string Content { get; set; }

        [Display(Name = "Дата публікації")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Вкажіть дату публікації")]
        public DateTime PublicationDate { get; set; } = DateTime.Now;

        [Display(Name = "Категорія")]
        [Required(ErrorMessage = "Вкажіть категорію")]
        [StringLength(50, ErrorMessage = "Назва категорії не може перевищувати 50 символів")]
        public string Category { get; set; }

        [Display(Name = "Рейтинг")]
        [Required(ErrorMessage = "Вкажіть рейтинг")]
        [Range(0, 10, ErrorMessage = "Рейтинг має бути від 0 до 10")]
        public int Rating { get; set; }

        public ICollection<ArticleAuthor>? ArticleAuthors { get; set; }
    }
}