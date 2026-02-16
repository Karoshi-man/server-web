using lab1.Data;
using lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace lab1.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly JournalContext _context;

        public ArticlesController(JournalContext context)
        {
            _context = context;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Articles.ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var article = await _context.Articles
                .Include(a => a.ArticleAuthors).ThenInclude(aa => aa.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null) return NotFound();
            return View(article);
        }


        public IActionResult Create()
        {
            // 1. Відновлюємо дані з сесії (прості рядки - це надійно)
            var draftTitle = HttpContext.Session.GetString("Draft_Create_Title");
            var draftContent = HttpContext.Session.GetString("Draft_Create_Content");
            var draftCategory = HttpContext.Session.GetString("Draft_Create_Category");
            // Можна додати й інші поля за аналогією

            var article = new Article();

            // Якщо в сесії щось є - заповнюємо форму
            if (!string.IsNullOrEmpty(draftTitle) || !string.IsNullOrEmpty(draftContent))
            {
                article.Title = draftTitle;
                article.Content = draftContent;
                article.Category = draftCategory;
                // Додаємо повідомлення, щоб користувач бачив, що це чернетка
                TempData["Message"] = "Restored unpreserved draft";
            }
            else
            {
                article.PublicationDate = DateTime.Now;
            }

            ViewData["AuthorsList"] = new MultiSelectList(_context.Authors, "Id", "FullName");
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article, int[] selectedAuthors, string action)
        {
            // 1. ПРИМУСОВО ЗБЕРІГАЄМО В СЕСІЮ (спочатку, прості рядки)
            HttpContext.Session.SetString("Draft_Create_Title", article.Title ?? "");
            HttpContext.Session.SetString("Draft_Create_Content", article.Content ?? "");
            HttpContext.Session.SetString("Draft_Create_Category", article.Category ?? "");

            // 2. ЛОГІКА КНОПОК

            // Якщо натиснули "Зберегти чернетку"
            if (action == "Draft")
            {
                TempData["Message"] = "Saved";
                // Повертаємо ту саму сторінку, дані підтягнуться з моделі article
                ViewData["AuthorsList"] = new MultiSelectList(_context.Authors, "Id", "FullName", selectedAuthors);
                return View(article);
            }

            // Якщо натиснули "Опублікувати"
            if (action == "Publish")
            {
                if (ModelState.IsValid)
                {
                    // Додаємо авторів
                    if (selectedAuthors != null)
                    {
                        article.ArticleAuthors = new List<ArticleAuthor>();
                        foreach (var id in selectedAuthors)
                        {
                            article.ArticleAuthors.Add(new ArticleAuthor { AuthorId = id, Article = article });
                        }
                    }

                    _context.Add(article);
                    await _context.SaveChangesAsync();

                    // ОЧИЩАЄМО СЕСІЮ (видаляємо ключі)
                    HttpContext.Session.Remove("Draft_Create_Title");
                    HttpContext.Session.Remove("Draft_Create_Content");
                    HttpContext.Session.Remove("Draft_Create_Category");

                    return RedirectToAction(nameof(Index));
                }
            }

            // Якщо помилка валідації або щось пішло не так
            ViewData["AuthorsList"] = new MultiSelectList(_context.Authors, "Id", "FullName", selectedAuthors);
            return View(article);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Ключі унікальні для кожної статті (по ID)
            string keyTitle = $"Draft_Edit_{id}_Title";
            string keyContent = $"Draft_Edit_{id}_Content";

            // 1. Перевіряємо сесію
            var draftTitle = HttpContext.Session.GetString(keyTitle);
            var draftContent = HttpContext.Session.GetString(keyContent);

            Article articleToEdit;

            if (!string.IsNullOrEmpty(draftTitle))
            {
                // Є чернетка -> беремо оригінал з БД для ID, але текст замінюємо з сесії
                articleToEdit = await _context.Articles
                    .Include(a => a.ArticleAuthors)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (articleToEdit != null)
                {
                    articleToEdit.Title = draftTitle;
                    articleToEdit.Content = draftContent;
                    TempData["Message"] = "You are editing a saved draft";
                }
            }
            else
            {
                // Немає чернетки -> беремо чисті дані з БД
                articleToEdit = await _context.Articles
                    .Include(a => a.ArticleAuthors)
                    .FirstOrDefaultAsync(m => m.Id == id);

                // Одразу ініціалізуємо сесію поточними даними
                if (articleToEdit != null)
                {
                    HttpContext.Session.SetString(keyTitle, articleToEdit.Title ?? "");
                    HttpContext.Session.SetString(keyContent, articleToEdit.Content ?? "");
                }
            }

            if (articleToEdit == null) return NotFound();

            var selectedIds = articleToEdit.ArticleAuthors?.Select(x => x.AuthorId).ToArray();
            ViewData["AuthorsList"] = new MultiSelectList(_context.Authors, "Id", "FullName", selectedIds);
            return View(articleToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Article article, int[] selectedAuthors, string action)
        {
            if (id != article.Id) return NotFound();

            string keyTitle = $"Draft_Edit_{id}_Title";
            string keyContent = $"Draft_Edit_{id}_Content";

            // 1. Оновлюємо сесію при будь-якій дії
            HttpContext.Session.SetString(keyTitle, article.Title ?? "");
            HttpContext.Session.SetString(keyContent, article.Content ?? "");

            if (action == "Draft")
            {
                TempData["Message"] = "Зміни збережено в сесії (БД не змінено)";
                ViewData["AuthorsList"] = new MultiSelectList(_context.Authors, "Id", "FullName", selectedAuthors);
                return View(article);
            }

            if (action == "Publish")
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(article);

                        // Оновлення авторів
                        var existing = await _context.Articles.Include(a => a.ArticleAuthors).FirstOrDefaultAsync(a => a.Id == id);
                        if (existing != null)
                        {
                            _context.ArticleAuthors.RemoveRange(existing.ArticleAuthors);
                            if (selectedAuthors != null)
                            {
                                foreach (var authId in selectedAuthors)
                                {
                                    _context.ArticleAuthors.Add(new ArticleAuthor { ArticleId = id, AuthorId = authId });
                                }
                            }
                        }

                        await _context.SaveChangesAsync();

                        // Чистимо сесію
                        HttpContext.Session.Remove(keyTitle);
                        HttpContext.Session.Remove(keyContent);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ArticleExists(article.Id)) return NotFound();
                        throw;
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["AuthorsList"] = new MultiSelectList(_context.Authors, "Id", "FullName", selectedAuthors);
            return View(article);
        }

        // Delete (без змін)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var article = await _context.Articles.FirstOrDefaultAsync(m => m.Id == id);
            if (article == null) return NotFound();
            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null) _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id) => _context.Articles.Any(e => e.Id == id);
    }
}