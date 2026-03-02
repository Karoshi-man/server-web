using lab1.Data;
using lab1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace lab1.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly JournalContext _context;

        private static readonly ConcurrentDictionary<string, string> OnlineUsers = new();
        public ChatHub(JournalContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                OnlineUsers[userId] = Context.ConnectionId;
                await Clients.All.SendAsync("UserStatusChanged", userId, true);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                OnlineUsers.TryRemove(userId, out _);
                await Clients.All.SendAsync("UserStatusChanged", userId, false);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public bool CheckIfOnline(string authorUserId)
        {
            return OnlineUsers.ContainsKey(authorUserId);
        }

        public async Task MarkAsRead(int senderAuthorId)
        {
            var currentUserId = Context.User?.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var receiver = await _context.Authors.FirstOrDefaultAsync(a => a.UserId == currentUserId);
            if (receiver == null) return;

            var unreadMessages = await _context.ChatMessages
                .Where(m => m.SenderId == senderAuthorId && m.ReceiverId == receiver.Id && !m.IsRead)
                .ToListAsync();

            if (unreadMessages.Any())
            {
                foreach (var msg in unreadMessages) msg.IsRead = true;
                await _context.SaveChangesAsync();
            }

            var sender = await _context.Authors.FindAsync(senderAuthorId);
            if (sender != null && sender.UserId != null && OnlineUsers.ContainsKey(sender.UserId))
            {
                await Clients.User(sender.UserId).SendAsync("MessagesWereRead", receiver.Id);
            }
        }

        public async Task JoinArticleGroup(string articleId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Article_{articleId}");
        }

        public async Task LeaveArticleGroup(string articleId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Article_{articleId}");
        }

        public async Task SendArticleMessage(int articleId, string message, string? attachmentUrl = null)
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.UserId == userId);

            if (author == null) return;

            var chatMessage = new ChatMessage
            {
                Content = message,
                AttachmentUrl = attachmentUrl,
                SentAt = DateTime.Now,
                SenderId = author.Id,
                ArticleId = articleId
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();
            await Clients.Group($"Article_{articleId}").SendAsync("ReceiveArticleMessage",
                author.FullName,
                message,
                chatMessage.SentAt.ToString("HH:mm"),
                attachmentUrl);
        }

        public async Task SendPrivateMessage(int receiverId, string message, string? attachmentUrl = null)
        {
            var userId = Context.User?.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var sender = await _context.Authors.FirstOrDefaultAsync(a => a.UserId == userId);
            if (sender == null) return;

            var chatMessage = new ChatMessage
            {
                Content = message,
                AttachmentUrl = attachmentUrl,
                SentAt = DateTime.Now,
                SenderId = sender.Id,
                ReceiverId = receiverId,
                IsRead = false
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            var receiver = await _context.Authors.FindAsync(receiverId);
            if (receiver != null && receiver.UserId != null)
            {
                await Clients.User(receiver.UserId).SendAsync("ReceivePrivateMessage",
                    sender.Id, sender.FullName, message, chatMessage.SentAt.ToString("HH:mm"), attachmentUrl);
                await Clients.User(receiver.UserId).SendAsync("GlobalNotification");
            }
        }
    }
}