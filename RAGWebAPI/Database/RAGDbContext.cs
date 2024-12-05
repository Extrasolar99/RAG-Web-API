using Microsoft.EntityFrameworkCore;
using RAGWebAPI.Models.Entities;

namespace RAGWebAPI.Database;


public class RAGDbContext(DbContextOptions<RAGDbContext> options) : DbContext(options)
{
    public DbSet<RagPdfDocument> RagPdfDocuments { get; set; }
    public DbSet<RagPdfPage> RagPdfPages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity<RagPdfDocument>()
            .HasMany(pd => pd.RagPdfPages)
            .WithOne(p => p.RagPdfDocument)
            .HasForeignKey(p => p.RagPdfDocumentId);

        modelBuilder.Entity<RagPdfPage>()
            .Property(pp => pp.Embedding)
            .HasColumnType("vector(1024)");
    }
}