namespace BookstoreAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }   // decimal, not double — always use decimal for money

        // Foreign key — EF Core convention: "[NavigationPropertyName]Id"
        // This column will exist in the Books table as a FK to Authors.Id
        public int AuthorId { get; set; }

        // Navigation property — loads the related Author object
        // null! tells the compiler "trust me, this will be set by EF Core"
        public Author Author { get; set; } = null!;
    }
}
