using System.ComponentModel.DataAnnotations;

namespace lab1.Models
{
    public class ArticleRating
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }
        public Article? Article { get; set; }

        public string UserId { get; set; }

        [Range(1, 10)]
        public int Score { get; set; }
    }
}