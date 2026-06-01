using BookstoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreAPI.Data
{
    public class BookstoreDbContext : DbContext
    {
        // Constructor: receives DbContextOptions via Dependency Injection
        // This is how ASP.NET Core passes the connection string in at runtime
        public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options)
            : base(options) { }

        // Each DbSet = one database table
        // EF Core pluralises the class name for the table name (Book → Books)
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();

        // OnModelCreating: configure relationships and seed starter data
        // Runs once when EF Core builds the model for the first time
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data — inserted automatically when you run your first migration
            // The IDs must be hardcoded (not auto-generated) for seed data to be stable
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Robert C. Martin" },
                new Author { Id = 2, Name = "Martin Fowler" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Clean Code", Price = 39.99m, AuthorId = 1 },
                new Book { Id = 2, Title = "The Clean Coder", Price = 29.99m, AuthorId = 1 },
                new Book { Id = 3, Title = "Refactoring", Price = 44.99m, AuthorId = 2 }
            );
        }
    }
}
