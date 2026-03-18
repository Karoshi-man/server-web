using System;

namespace lab1.DTOs
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public DateTime PublicationDate { get; set; }
        public double Rating { get; set; }
    }
}