using Articles.Microservice.DTOs;
using Articles.Microservice.Models;
using Articles.Microservice.Repositories;
using Articles.Microservice.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesApiController : ControllerBase
    {
        private readonly IArticleRepository _repository;
        private readonly AuthorIntegrationService _authorIntegrationService;

        public ArticlesApiController(IArticleRepository repository, AuthorIntegrationService authorIntegrationService)
        {
            _repository = repository;
            _authorIntegrationService = authorIntegrationService;
        }

        // GET: api/articlesapi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticles()
        {
            var articles = await _repository.GetAllAsync();

            var dtos = articles.Select(a => new ArticleDto
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Category = a.Category,
                PublicationDate = a.PublicationDate,
                AuthorId = a.AuthorId
            });

            return Ok(dtos);
        }

        // POST: api/articlesapi
        [HttpPost]
        public async Task<ActionResult<ArticleDto>> PostArticle(ArticleDto dto)
        {
            var article = new Article
            {
                Title = dto.Title,
                Content = dto.Content,
                Category = dto.Category,
                PublicationDate = DateTime.Now,
                AuthorId = dto.AuthorId
            };

            await _repository.AddAsync(article);
            dto.Id = article.Id;

            return CreatedAtAction(nameof(GetArticles), new { id = article.Id }, dto);
        }

        // GET: api/articlesapi/{id}/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<ArticleDetailsDto>> GetArticleDetails(int id)
        {
            var article = await _repository.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            var authorData = await _authorIntegrationService.GetAuthorByIdAsync(article.AuthorId);
            var result = new ArticleDetailsDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Category = article.Category,
                PublicationDate = article.PublicationDate,
                AuthorFullName = authorData != null ? authorData.FullName : "Unknown Author"
            };

            return Ok(result);
        }
    }
}