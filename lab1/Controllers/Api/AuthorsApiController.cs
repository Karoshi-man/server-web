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
    public class AuthorsApiController : ControllerBase
    {
        private readonly IAuthorRepository _repository;

        public AuthorsApiController(IAuthorRepository repository)
        {
            _repository = repository;
        }

        // GET: api/AuthorsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authors = await _repository.GetAllAsync();
            var dtos = authors.Select(a => new AuthorDto
            {
                Id = a.Id,
                FullName = a.FullName,
                Nickname = a.Nickname,
                Bio = a.Bio,
                Rating = a.Rating
            });
            return Ok(dtos);
        }

        // GET: api/AuthorsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            var author = await _repository.GetByIdAsync(id);
            if (author == null) return NotFound();

            var dto = new AuthorDto
            {
                Id = author.Id,
                FullName = author.FullName,
                Nickname = author.Nickname,
                Bio = author.Bio,
                Rating = author.Rating
            };
            return Ok(dto);
        }

        // POST: api/AuthorsApi
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> PostAuthor(AuthorDto dto)
        {
            var author = new Author
            {
                FullName = dto.FullName,
                Nickname = dto.Nickname,
                Bio = dto.Bio,
                DateOfBirth = System.DateTime.Now.AddYears(-20),
                Email = "api@user.com"
            };

            await _repository.AddAsync(author);
            dto.Id = author.Id;

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, dto);
        }

        // PUT: api/AuthorsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var author = await _repository.GetByIdAsync(id);
            if (author == null) return NotFound();

            author.FullName = dto.FullName;
            author.Nickname = dto.Nickname;
            author.Bio = dto.Bio;

            await _repository.UpdateAsync(author);
            return NoContent();
        }

        // DELETE: api/AuthorsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (!await _repository.ExistsAsync(id)) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}