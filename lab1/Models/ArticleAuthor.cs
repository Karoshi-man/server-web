namespace lab1.Models
{
    public class ArticleAuthor
    {
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}