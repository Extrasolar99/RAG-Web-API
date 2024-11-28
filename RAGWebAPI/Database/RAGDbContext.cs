// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pgvector;

namespace RAGWebAPI.Database;


public class RAGDbContext(DbContextOptions<RAGDbContext> options) : DbContext(options)
{
    public DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.Entity<Document>()
            .Property(d => d.Embedding)
            .HasColumnType("vector(300)");
    }

    private void SeedData(ModelBuilder modelBuilder)
    {

    }
}

public class Document
{
    public int Id { get; set; }
    public string Content { get; set; }
    public Vector Embedding { get; set; }
}