using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookstoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceRangeView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW IF NOT EXISTS vw_AffordableBooks AS
                SELECT b.Id, b.Title, b.Price, b.AuthorId
                FROM Books b
                WHERE b.Price < 30.00
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_AffordableBooks");
        }
    }
}
