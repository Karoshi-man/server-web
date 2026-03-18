namespace Articles.Microservice.DTOs
{
    public class ArticleDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public DateTime PublicationDate { get; set; }
        public string AuthorFullName { get; set; }
    }
}