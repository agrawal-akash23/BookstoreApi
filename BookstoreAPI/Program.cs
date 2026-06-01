using BookstoreAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        // Handle circular references - stops at the first loop
        options.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register BookstoreDbContext with the DI container
// The DI container will inject it into any constructor that asks for it 
builder.Services.AddDbContext<BookstoreDbContext>(options => 
options.UseSqlite("Data Source=bookstore.db")
.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
.EnableSensitiveDataLogging()); // shows actual parameter values

// To use SQL Server instead, replace with:
// options.UseSqlServer(builder.Configuration.ConnectionString("DefaultConnection"))
// Then add to appsettings.json: "ConnectionStrings": { "DefaultConnection": "Server=.....;...."}

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseAuthorization();
app.MapControllers();
app.Run();

