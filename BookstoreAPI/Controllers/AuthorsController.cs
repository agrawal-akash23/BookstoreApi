using BookstoreAPI.Data;
using BookstoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookstoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly BookstoreDbContext _db;
        public AuthorsController(BookstoreDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<List<Author>>> GetAll() =>
            await _db.Authors.Include(a => a.Books).OrderBy(a => a.Name).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetById(int id)
        {
            var author = await _db.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
            return author is null ? NotFound() : Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult<Author>> Create(Author author)
        {
            bool nameExists = await _db.Authors.AnyAsync(a => a.Name == author.Name);
            if (nameExists) 
                return Conflict(new { message = $"Author '{author.Name}' already exists."});

            _db.Authors.Add(author);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Author updated)
        {
            var author = await _db.Authors.FindAsync(id);
            if (author is null) return NotFound();
            author.Name = updated.Name;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _db.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
            if (author is null) return NotFound();

            if (author.Books.Any())
                return Conflict(new { message = $"Cannot delete '{author.Name}' - they have {author.Books.Count} book(s). Remove the books first." });
            
            _db.Authors.Remove(author);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
