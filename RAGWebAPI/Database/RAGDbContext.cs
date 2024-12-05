using Microsoft.EntityFrameworkCore;
using RAGWebAPI.Models.Entities;

namespace RAGWebAPI.Database;


public class RAGDbContext(DbContextOptions<RAGDbContext> options) : DbContext(options)
{
    public DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.Entity<Document>()
            .Property(d => d.Embedding)
            .HasColumnType("vector(1024)");
    }
}