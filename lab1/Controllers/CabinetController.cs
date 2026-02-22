using lab1.Data;
using lab1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace lab1.Controllers
{
    [Authorize] // Доступ тільки для зареєстрованих
    public class CabinetController : Controller
    {
        private readonly JournalContext _context;

        public CabinetController(JournalContext context)
        {
            _context = context;
        }

        // Головна сторінка кабінету
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var authorProfile = await _context.Authors.FirstOrDefaultAsync(a => a.UserId == userId);

            if (authorProfile == null)
            {
                return RedirectToAction(nameof(InitializeProfile));
            }

            var myArticles = await _context.ArticleAuthors
                .Include(aa => aa.Article)
                .Where(aa => aa.AuthorId == authorProfile.Id && !aa.Article.IsDraft)
                .Select(aa => aa.Article)
                .ToListAsync();

            var myInvitations = await _context.CoAuthorInvitations
                .Include(i => i.Article)
                .Include(i => i.Inviter)
                .Where(i => i.InviteeEmail == authorProfile.Email && i.Status == InvitationStatus.Pending)
                .ToListAsync();

            ViewBag.MyArticles = myArticles;
            ViewBag.Invitations = myInvitations;

            return View(authorProfile);
        }

        // GET: Сторінка чернеток
        public async Task<IActionResult> Drafts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var authorProfile = await _context.Authors.FirstOrDefaultAsync(a => a.UserId == userId);

            if (authorProfile == null) return RedirectToAction(nameof(InitializeProfile));

            var myDrafts = await _context.ArticleAuthors
                .Include(aa => aa.Article)
                .Where(aa => aa.AuthorId == authorProfile.Id && aa.Article.IsDraft)
                .Select(aa => aa.Article)
                .ToListAsync();

            return View(myDrafts);
        }

        // GET: Форма створення профілю
        public IActionResult InitializeProfile()
        {
            return View();
        }

        // POST: Збереження профілю
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InitializeProfile([Bind("FullName,Nickname,Bio,DateOfBirth")] Author author)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            author.UserId = currentUserId;
            author.Email = User.Identity?.Name ?? "unknown@system.com";

            ModelState.Remove("UserId");
            ModelState.Remove("User");
            ModelState.Remove("Email");

            var minimumAgeDate = DateTime.Now.AddYears(-16);
            if (author.DateOfBirth > minimumAgeDate)
            {
                ModelState.AddModelError("DateOfBirth", "Access denied: You must be at least 14 years old to initialize a profile.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Profile initialized successfully. You can now publish articles.";
                return RedirectToAction(nameof(Index));
            }

            return View(author);
        }

        // POST: Відправка запрошення співавтору
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendInvite(int articleId, string inviteeEmail)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.UserId == currentUserId);

            if (currentAuthor == null) return RedirectToAction(nameof(InitializeProfile));

            var isAuthor = await _context.ArticleAuthors
                .AnyAsync(aa => aa.ArticleId == articleId && aa.AuthorId == currentAuthor.Id);

            if (!isAuthor && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            if (string.IsNullOrWhiteSpace(inviteeEmail) || inviteeEmail.ToLower() == currentAuthor.Email.ToLower())
            {
                TempData["Message"] = "Invalid email address or you are trying to invite yourself.";
                return RedirectToAction(nameof(Index));
            }

            var existingInvite = await _context.CoAuthorInvitations
                .FirstOrDefaultAsync(i => i.ArticleId == articleId
                                       && i.InviteeEmail == inviteeEmail
                                       && i.Status == InvitationStatus.Pending);

            if (existingInvite != null)
            {
                TempData["Message"] = "An invitation is already pending for this email.";
                return RedirectToAction(nameof(Index));
            }

            var invite = new CoAuthorInvitation
            {
                ArticleId = articleId,
                InviterId = currentUserId,
                InviteeEmail = inviteeEmail,
                Status = InvitationStatus.Pending,
                CreatedAt = DateTime.Now
            };

            _context.CoAuthorInvitations.Add(invite);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Invitation sent successfully to {inviteeEmail}.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Форма редагування профілю
        public async Task<IActionResult> EditProfile()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var authorProfile = await _context.Authors.FirstOrDefaultAsync(a => a.UserId == currentUserId);

            if (authorProfile == null) return RedirectToAction(nameof(InitializeProfile));

            return View(authorProfile);
        }

        // POST: Збереження змін у профілі
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(int id, [Bind("Id,FullName,Nickname,Email,Bio,DateOfBirth,UserId")] Author author)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != author.Id || author.UserId != currentUserId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "Profile updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Authors.Any(e => e.Id == author.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // POST: Прийняття запрошення
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptInvite(int inviteId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.UserId == currentUserId);

            if (currentAuthor == null) return RedirectToAction(nameof(InitializeProfile));

            // Шукаємо запрошення в базі
            var invitation = await _context.CoAuthorInvitations
                .Include(i => i.Article)
                .FirstOrDefaultAsync(i => i.Id == inviteId && i.Status == InvitationStatus.Pending);

            if (invitation == null)
            {
                TempData["Message"] = "Invitation not found or has already been processed.";
                return RedirectToAction(nameof(Index));
            }

            // ПЕРЕВІРКА БЕЗПЕКИ: Чи дійсно запрошення вислано на пошту цього профілю?
            if (invitation.InviteeEmail.ToLower() != currentAuthor.Email.ToLower())
            {
                return Forbid();
            }

            // Змінюємо статус на "Прийнято"
            invitation.Status = InvitationStatus.Accepted;

            // Перевіряємо, чи раптом людина вже не є автором (щоб уникнути дублів у БД)
            var alreadyAuthor = await _context.ArticleAuthors
                .AnyAsync(aa => aa.ArticleId == invitation.ArticleId && aa.AuthorId == currentAuthor.Id);

            if (!alreadyAuthor)
            {
                // Урочистий момент: Додаємо людину як співавтора до статті!
                var newCoAuthor = new ArticleAuthor
                {
                    ArticleId = invitation.ArticleId,
                    AuthorId = currentAuthor.Id
                };
                _context.ArticleAuthors.Add(newCoAuthor);
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = $"Access granted. You are now a co-author of '{invitation.Article?.Title}'.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Відхилення запрошення
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeclineInvite(int inviteId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.UserId == currentUserId);

            if (currentAuthor == null) return RedirectToAction(nameof(InitializeProfile));

            var invitation = await _context.CoAuthorInvitations
                .Include(i => i.Article)
                .FirstOrDefaultAsync(i => i.Id == inviteId && i.Status == InvitationStatus.Pending);

            if (invitation == null)
            {
                TempData["Message"] = "Invitation not found or has already been processed.";
                return RedirectToAction(nameof(Index));
            }

            // Перевірка безпеки
            if (invitation.InviteeEmail.ToLower() != currentAuthor.Email.ToLower())
            {
                return Forbid();
            }

            // Оновлюємо статус
            invitation.Status = InvitationStatus.Declined;
            await _context.SaveChangesAsync();

            TempData["Message"] = $"You have declined the invitation to co-author '{invitation.Article?.Title}'.";
            return RedirectToAction(nameof(Index));
        }
    }
}