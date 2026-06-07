# BookstoreApi — EF Core + ASP.NET Core Web API

A RESTful Bookstore API backed by
a real database using Entity Framework Core. Demonstrates code-first migrations,
LINQ queries, navigation properties, raw SQL, and seed data.

**Stack:** ASP.NET Core 8 · Entity Framework Core · SQLite (dev) / SQL Server (prod) · Swagger

## What this project covers

| Concept | Where it appears |
|---|---|
| Entity Framework Core | `BookstoreDbContext`, `DbSet<T>` |
| Code-first migrations | `Migrations/` folder, `add-migration` |
| LINQ queries | `BooksController` — `Where`, `Include`, `OrderBy` |
| Navigation properties | `Book.Author` loaded via `Include()` |
| Parameterised raw SQL | `FromSqlRaw` in `/books/cheap` endpoint |
| Bulk SQL via `ExecuteSqlRaw` | `/books/discount` endpoint |
| Seed data | `OnModelCreating` in `BookstoreDbContext` |
| One-to-many relationship | `Author` → `List<Book>` with FK |
| Async/await throughout | Every controller action uses `async Task` |
| SQL injection prevention | `{0}` placeholders in all raw SQL |

## Running locally

Prerequisites: [.NET 8 SDK](https://dotnet.microsoft.com/download)

```bash
git clone https://github.com/YOUR_USERNAME/BookstoreApi
cd BookstoreApi
dotnet ef database update    # creates bookstore.db + runs seed data
dotnet run                   # starts on https://localhost:
```

Open **https://localhost:5001/swagger** to explore all endpoints interactively.

## API endpoints

| Method | Route | Description | DB path |
|--------|-------|-------------|---------|
| GET | `/api/books` | All books with authors | LINQ + Include |
| GET | `/api/books/{id}` | Single book or 404 | LINQ FirstOrDefault |
| GET | `/api/books/search?title=` | Filter by title keyword | LINQ Where |
| GET | `/api/books/cheap?maxPrice=` | Price filter | Raw SQL (FromSqlRaw) |
| POST | `/api/books` | Create a book | EF Add + SaveChanges |
| PUT | `/api/books/{id}` | Update a book | EF change tracking |
| DELETE | `/api/books/{id}` | Delete a book | EF Remove + SaveChanges |
| POST | `/api/books/discount?percent=` | Bulk % discount | ExecuteSqlRaw |

## Project structure

```
BookstoreApi/
├── Controllers/
│   └── BooksController.cs     # 8 endpoints across 3 DB access paths
├── Data/
│   └── BookstoreDbContext.cs  # DbContext with seed data
├── Migrations/                # Auto-generated — never edit by hand
├── Models/
│   ├── Author.cs              # Navigation: Author → List<Book>
│   └── Book.cs                # FK: Book.AuthorId → Author.Id
├── appsettings.json
└── Program.cs                 # DI registration, SQLite config

```

## Key decisions

**SQLite for local dev:** Zero-setup, stores in a single `.db` file. Swap to SQL Server
by changing one line in `Program.cs` — the EF Core LINQ code is identical.

**Seed data via migrations:** Books and authors are inserted in the `InitialCreate`
migration. Any developer who clones this repo gets an identical, pre-populated database
after `dotnet ef database update`.

**Three database access paths:**
1. **LINQ** (everyday work) — type-safe, compiler-checked, no SQL injection risk
2. **`FromSqlRaw`** (raw SQL) — parameterised, still maps to C# objects via EF Core
3. **`ExecuteSqlRaw`** (bulk operations) — returns row count, not entities
