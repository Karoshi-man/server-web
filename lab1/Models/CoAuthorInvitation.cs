using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace lab1.Models
{
    public class CoAuthorInvitation
    {
        public int Id { get; set; }

        [Required]
        public int ArticleId { get; set; }
        public Article? Article { get; set; }

        [Required]
        public string InviterId { get; set; }
        public IdentityUser? Inviter { get; set; }

        [Required]
        public string InviteeEmail { get; set; }

        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public enum InvitationStatus
    {
        Pending,
        Accepted,
        Declined
    }
}