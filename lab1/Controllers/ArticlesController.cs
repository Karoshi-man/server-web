using lab1.Data;
using lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;

namespace lab1.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private readonly JournalContext _context;

        public ArticlesController(JournalContext context)
        {
            _context = context;
        }

        // GET: Articles
        public async Task<IActionResult> Index(string category, string sortBy)
        {
            var query = _context.Articles
                .Include(a => a.User)
                .Include(a => a.ArticleAuthors).ThenInclude(aa => aa.Author)
                .Where(a => !a.IsDraft)
                .AsQueryable();

            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                query = query.Where(a => a.Category == category);
            }

            switch (sortBy)
            {
                case "rating":
                    query = query.OrderByDescending(a => a.Rating);
                    break;
                case "top_author":
                    query = query.OrderByDescending(a => a.ArticleAuthors.Max(aa => aa.Author.Rating));
                    break;
                case "oldest":
                    query = query.OrderBy(a => a.PublicationDate);
                    break;
                case "newest":
                default:
                    query = query.OrderByDescending(a => a.PublicationDate);
                    break;
            }

            // Передаємо список унікальних категорій у View для випадаючого списку
            ViewBag.Categories = await _context.Articles
                .Where(a => !a.IsDraft)
                .Select(a => a.Category)
                .Distinct()
                .ToListAsync();

            ViewBag.CurrentCategory = category;
            ViewBag.CurrentSort = sortBy;

            return View(await query.ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Articles
                .Include(a => a.ArticleAuthors).ThenInclude(aa => aa.Author)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (article == null) return NotFound();
            return View(article);
        }

        // GET: Articles/Create
        public async Task<IActionResult> Create()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.UserId == currentUserId);

            if (currentAuthor == null)
            {
                TempData["Message"] = "To publish articles, please initialize your Author Profile first.";
                return RedirectToAction("Index", "Cabinet");
            }
            return View(new Article { PublicationDate = DateTime.Now });
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article, string action)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.UserId == currentUserId);

            if (currentAuthor == null) return RedirectToAction("Index", "Cabinet");

            article.UserId = currentUserId;

            article.IsDraft = (action == "Draft");

            ModelState.Remove("UserId");
            ModelState.Remove("User");
            ModelState.Remove("ArticleAuthors");

            if (ModelState.IsValid)
            {
                article.ArticleAuthors = new List<ArticleAuthor>
                {
                    new ArticleAuthor { AuthorId = currentAuthor.Id, Article = article }
                };

                _context.Add(article);
                await _context.SaveChangesAsync();

                if (article.IsDraft)
                {
                    TempData["Message"] = "Draft saved successfully to your workspace.";
                    return RedirectToAction("Drafts", "Cabinet");
                }

                return RedirectToAction(nameof(Index));
            }

            return View(article);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var articleToEdit = await _context.Articles
                    .Include(a => a.ArticleAuthors)
                    .FirstOrDefaultAsync(m => m.Id == id);

            if (articleToEdit == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.UserId == currentUserId);

            bool isOwner = articleToEdit.UserId == currentUserId;
            bool isAdmin = User.IsInRole("Admin");
            bool isCoAuthor = currentAuthor != null && articleToEdit.ArticleAuthors.Any(aa => aa.AuthorId == currentAuthor.Id);

            if (!isOwner && !isAdmin && !isCoAuthor)
            {
                return Forbid();
            }

            return View(articleToEdit);
        }

        // POST: Articles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Article article, string action)
        {
            if (id != article.Id) return NotFound();

            var originalArticle = await _context.Articles
                .Include(a => a.ArticleAuthors)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (originalArticle == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.UserId == currentUserId);

            bool isOwner = originalArticle.UserId == currentUserId;
            bool isAdmin = User.IsInRole("Admin");
            bool isCoAuthor = currentAuthor != null && originalArticle.ArticleAuthors.Any(aa => aa.AuthorId == currentAuthor.Id);

            if (!isOwner && !isAdmin && !isCoAuthor)
            {
                return Forbid();
            }

            article.UserId = originalArticle.UserId;

            ModelState.Remove("UserId");
            ModelState.Remove("User");
            ModelState.Remove("ArticleAuthors");

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);

                    if (existing != null)
                    {
                        existing.Title = article.Title;
                        existing.Content = article.Content;
                        existing.Category = article.Category;
                        existing.PublicationDate = article.PublicationDate;

                        existing.IsDraft = (action == "Draft");
                    }

                    await _context.SaveChangesAsync();

                    if (action == "Draft")
                    {
                        TempData["Message"] = "Draft updated successfully.";
                        return RedirectToAction("Drafts", "Cabinet");
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id)) return NotFound();
                    throw;
                }
            }

            return View(article);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Articles.FirstOrDefaultAsync(m => m.Id == id);
            if (article == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (article.UserId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles
                .Include(a => a.ArticleAuthors)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article != null)
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (article.UserId != currentUserId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var invitations = _context.CoAuthorInvitations.Where(i => i.ArticleId == id);
                if (invitations.Any())
                {
                    _context.CoAuthorInvitations.RemoveRange(invitations);
                }

                if (article.ArticleAuthors != null && article.ArticleAuthors.Any())
                {
                    _context.ArticleAuthors.RemoveRange(article.ArticleAuthors);
                }

                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Голосування за статтю
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RateArticle(int articleId, int score)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var article = await _context.Articles.Include(a => a.Ratings).FirstOrDefaultAsync(a => a.Id == articleId);

            if (article == null) return NotFound();

            if (article.UserId == currentUserId)
            {
                TempData["Message"] = "You cannot rate your own article.";
                return RedirectToAction(nameof(Details), new { id = articleId });
            }

            var existingRating = await _context.ArticleRatings
                .FirstOrDefaultAsync(r => r.ArticleId == articleId && r.UserId == currentUserId);

            if (existingRating != null)
            {
                existingRating.Score = score;
            }
            else
            {
                _context.ArticleRatings.Add(new ArticleRating
                {
                    ArticleId = articleId,
                    UserId = currentUserId,
                    Score = score
                });
            }
            await _context.SaveChangesAsync();

            var allRatings = await _context.ArticleRatings.Where(r => r.ArticleId == articleId).ToListAsync();
            article.Rating = Math.Round(allRatings.Average(r => (double)r.Score), 1);

            await _context.SaveChangesAsync();

            TempData["Message"] = "Thank you! Your rating has been saved";
            return RedirectToAction(nameof(Details), new { id = articleId });
        }

        private bool ArticleExists(int id) => _context.Articles.Any(e => e.Id == id);
    }
}