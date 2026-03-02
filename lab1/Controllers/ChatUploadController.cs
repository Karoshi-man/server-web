using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatUploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public ChatUploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file uploaded." });

            const long maxFileSize = 5 * 1024 * 1024;
            if (file.Length > maxFileSize)
                return BadRequest(new { message = "File is too large. Maximum size is 5MB." });

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !allowedExtensions.Contains(ext))
                return BadRequest(new { message = "Invalid file type. Only JPG, PNG, GIF, and WEBP are allowed." });

            var fileName = Guid.NewGuid().ToString() + ext;

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "chat");
            Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { url = $"/uploads/chat/{fileName}" });
        }
    }
}