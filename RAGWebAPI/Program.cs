using Microsoft.EntityFrameworkCore;
using OllamaSharp;

using RAGWebAPI.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<RAGDbContext>(optionsBuilder => 
    optionsBuilder.UseNpgsql("Host=rag.database;Port=5432;Database=rag;Username=rag;Password=rag123", o => o.UseVector()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Auto apply migrations in development
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<RAGDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.UseAuthorization();

app.MapControllers();

app.Run();
