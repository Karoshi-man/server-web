using Authors.Microservice.DTOs;
using Authors.Microservice.Models;
using Authors.Microservice.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Authors.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsApiController : ControllerBase
    {
        private readonly IAuthorRepository _repository;

        public AuthorsApiController(IAuthorRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authors = await _repository.GetAllAsync();
            return Ok(authors.Select(a => new AuthorDto { Id = a.Id, FullName = a.FullName, Nickname = a.Nickname, Bio = a.Bio, Rating = a.Rating }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            var author = await _repository.GetByIdAsync(id);
            if (author == null) return NotFound();
            return Ok(new AuthorDto { Id = author.Id, FullName = author.FullName, Nickname = author.Nickname, Bio = author.Bio, Rating = author.Rating });
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> PostAuthor(AuthorDto dto)
        {
            var author = new Author { FullName = dto.FullName, Nickname = dto.Nickname, Bio = dto.Bio };
            await _repository.AddAsync(author);
            dto.Id = author.Id;
            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, dto);
        }
    }
}