using lab1.DTOs;
using lab1.Models;
using lab1.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab1.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesApiController : ControllerBase
    {
        private readonly IArticleRepository _repository;

        public ArticlesApiController(IArticleRepository repository)
        {
            _repository = repository;
        }

        // GET: api/ArticlesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticles()
        {
            var articles = await _repository.GetAllAsync();
            var dtos = articles.Select(a => new ArticleDto
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Category = a.Category ?? "Uncategorized",
                PublicationDate = a.PublicationDate,
                Rating = a.Rating
            });
            return Ok(dtos);
        }

        // GET: api/ArticlesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleDto>> GetArticle(int id)
        {
            var article = await _repository.GetByIdAsync(id);
            if (article == null) return NotFound();

            var dto = new ArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Category = article.Category ?? "Uncategorized",
                PublicationDate = article.PublicationDate,
                Rating = article.Rating
            };
            return Ok(dto);
        }

        // POST: api/ArticlesApi
        [HttpPost]
        public async Task<ActionResult<ArticleDto>> PostArticle(ArticleDto dto)
        {
            var article = new Article
            {
                Title = dto.Title,
                Content = dto.Content,
                Category = dto.Category,
                PublicationDate = dto.PublicationDate,
                IsDraft = false
            };

            await _repository.AddAsync(article);
            dto.Id = article.Id;

            return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, dto);
        }

        // PUT: api/ArticlesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, ArticleDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var article = await _repository.GetByIdAsync(id);
            if (article == null) return NotFound();

            article.Title = dto.Title;
            article.Content = dto.Content;
            article.Category = dto.Category;

            await _repository.UpdateAsync(article);
            return NoContent();
        }

        // DELETE: api/ArticlesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            if (!await _repository.ExistsAsync(id)) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}