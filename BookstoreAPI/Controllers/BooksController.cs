using BookstoreAPI.Data;
using BookstoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookstoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   // resolves to api/books
    public class BooksController : ControllerBase
    {
        private readonly BookstoreDbContext _db;

        // DbContext injected via constructor DI — same pattern you learned in Project 3
        public BooksController(BookstoreDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetAll()
        {
            return await _db.Books
                .Include(b => b.Author)   // SQL: LEFT JOIN Authors ON Books.AuthorId = Authors.Id
                .OrderBy(b => b.Title)    // SQL: ORDER BY Title ASC
                .ToListAsync();            // executes the query, returns List<Book>
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Book>>> Search([FromQuery] string title) =>
            await _db.Books.Where(b => b.Title.ToLower().Contains(title.ToLower())).Include(b => b.Author).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetById(int id)
        {
            var book = await _db.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
            return book is null ? NotFound() : Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Create(Book book)
        {
            _db.Books.Add(book);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Book updated)
        {
            var book = await _db.Books.FindAsync(id);
            if (book is null) return NotFound();
            book.Title = updated.Title; book.Price = updated.Price; book.AuthorId = updated.AuthorId;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book is null) return NotFound();
            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("cheap")]
        public async Task<List<Book>> GetCheapBooks([FromQuery] decimal maxPrice = 35m) =>
            await _db.Books.FromSqlRaw("SELECT * FROM Books WHERE Price < {0}", maxPrice)
                .Include(b => b.Author).ToListAsync();

        [HttpPost("discount")]
        public async Task<IActionResult> ApplyDiscount([FromQuery] decimal percent)
        {
            int rows = await _db.Database.ExecuteSqlRawAsync(
                "UPDATE Books SET Price = Price * {0}", 1 - (percent / 100));
            return Ok(new { message = "Discount applied", rowsUpdated = rows });
        }
    }
}
