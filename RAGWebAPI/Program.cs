using Microsoft.EntityFrameworkCore;
using OllamaSharp;
using RAGWebAPI.Background;
using RAGWebAPI.Database;
using RAGWebAPI.Services;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//Debugger.Launch();
builder.Services.AddDbContext<RAGDbContext>(optionsBuilder => 
    optionsBuilder.UseNpgsql(connectionString, o => o.UseVector()));

builder.Services.AddScoped<IEmbedService, EmbedService>();
builder.Services.AddScoped<IGenerativeService, GenerativeService>();
builder.Services.AddScoped<IRagPdfService, RagPdfService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(new OllamaApiClient(Environment.GetEnvironmentVariable("OLLAMA_API_URL") ?? "http://rag.ollama:11434"));
builder.Services.AddHostedService<ModelPullingService>();

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
