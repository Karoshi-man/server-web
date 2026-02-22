using System.ComponentModel.DataAnnotations;

namespace lab1.Models
{
    public class AuthorRating
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public string UserId { get; set; }

        [Range(1, 10)]
        public int Score { get; set; }
    }
}