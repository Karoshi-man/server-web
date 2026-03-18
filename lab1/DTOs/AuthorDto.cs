namespace lab1.DTOs
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? Nickname { get; set; }
        public string? Bio { get; set; }
        public double Rating { get; set; }
    }
}