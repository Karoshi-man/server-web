using System;

namespace Articles.Microservice.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public DateTime PublicationDate { get; set; }
        public int AuthorId { get; set; }
    }
}