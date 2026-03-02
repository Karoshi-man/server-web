using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab1.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }
        public string? AttachmentUrl { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;

        [Required]
        public int SenderId { get; set; }
        [ForeignKey("SenderId")]
        public virtual Author? Sender { get; set; }
        public int? ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        public virtual Author? Receiver { get; set; }
        public int? ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public virtual Article? Article { get; set; }
    }
}