using lab1.Data;
using lab1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace lab1.Controllers
{
    [Authorize]
    public class AuthorsController : Controller
    {
        private readonly JournalContext _context;

        public AuthorsController(JournalContext context)
        {
            _context = context;
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            var authors = await _context.Authors
                .Include(a => a.ArticleAuthors)
                .ToListAsync();

            return View(authors);
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var author = await _context.Authors
                .Include(a => a.ArticleAuthors)
                    .ThenInclude(aa => aa.Article)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (author == null) return NotFound();

            ViewBag.HasProfile = false;
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId != null)
            {
                ViewBag.HasProfile = await _context.Authors.AnyAsync(a => a.UserId == currentUserId);
            }

            return View(author);
        }

        // GET: Authors/Create
        // ЗМІНЕНО: Прибрано [Authorize(Roles = "Admin")], щоб звичайні юзери могли створити профіль
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // ЗМІНЕНО: Прибрано [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Nickname,Email,Bio,DateOfBirth")] Author author)
        {
            if (!string.IsNullOrEmpty(author.Nickname))
            {
                bool nicknameExists = await _context.Authors.AnyAsync(a => a.Nickname.ToLower() == author.Nickname.ToLower());
                if (nicknameExists)
                {
                    ModelState.AddModelError("Nickname", "This username is already taken.");
                }
            }

            if (ModelState.IsValid)
            {
                // ЗМІНЕНО: Прив'язуємо створюваний профіль до поточного користувача
                author.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Nickname,Email,Bio,DateOfBirth")] Author author)
        {
            if (id != author.Id) return NotFound();

            if (!string.IsNullOrEmpty(author.Nickname))
            {
                bool nicknameExists = await _context.Authors.AnyAsync(a => a.Nickname.ToLower() == author.Nickname.ToLower() && a.Id != id);
                if (nicknameExists)
                {
                    ModelState.AddModelError("Nickname", "This username is already taken.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }

        // Допоміжна модель для отримання даних з AJAX
        public class RatingRequest
        {
            public int AuthorId { get; set; }
            public int Score { get; set; }
        }

        // POST: Authors/Rate
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Rate([FromBody] RatingRequest request)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
                return Json(new { success = false, message = "Please log in to rate." });

            var author = await _context.Authors.FindAsync(request.AuthorId);
            if (author == null)
                return Json(new { success = false, message = "Author not found." });

            if (author.UserId == currentUserId)
                return Json(new { success = false, message = "You cannot rate yourself." });

            var existingRating = await _context.AuthorRatings
                .FirstOrDefaultAsync(r => r.AuthorId == request.AuthorId && r.UserId == currentUserId);

            if (existingRating != null)
            {
                existingRating.Score = request.Score;
            }
            else
            {
                _context.AuthorRatings.Add(new AuthorRating
                {
                    AuthorId = request.AuthorId,
                    UserId = currentUserId,
                    Score = request.Score
                });
            }

            await _context.SaveChangesAsync();

            var averageRating = await _context.AuthorRatings
                .Where(r => r.AuthorId == request.AuthorId)
                .AverageAsync(r => (double)r.Score);

            author.Rating = Math.Round(averageRating, 1);
            await _context.SaveChangesAsync();

            return Json(new { success = true, newRating = author.Rating.ToString("0.0") });
        }

        // GET, POST: Authors/VerifyNickname
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyNickname(string nickname, int id)
        {
            if (string.IsNullOrEmpty(nickname))
            {
                return Json(true);
            }

            bool isTaken = await _context.Authors
                .AnyAsync(a => a.Nickname != null &&
                               a.Nickname.ToLower() == nickname.ToLower() &&
                               a.Id != id);

            if (isTaken)
            {
                return Json($"Username '@{nickname}' is already taken. Please choose another one.");
            }

            return Json(true);
        }
    }
}