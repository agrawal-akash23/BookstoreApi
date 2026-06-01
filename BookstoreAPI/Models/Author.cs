namespace BookstoreAPI.Models
{
    public class Author
    {
        public int Id { get; set; }           // EF Core sees "Id" → primary key (convention)
        public string Name { get; set; } = string.Empty;

        // Navigation property — NOT a database column.
        // EF Core uses this to load related Books when you call .Include(a => a.Books)
        public List<Book> Books { get; set; } = new();
    }
}
