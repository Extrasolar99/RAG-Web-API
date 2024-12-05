using System.ComponentModel.DataAnnotations.Schema;
using Pgvector;

namespace RAGWebAPI.Models.Entities;

public class RagPdfPage
{
    public int Id { get; set; }
    public int RagPdfDocumentId { get; set; }
    public virtual RagPdfDocument? RagPdfDocument { get; set; }
    public int PageNumber { get; set; }
    public string Content { get; set; } = "";

    [Column(TypeName = "vector(1024)")]
    public Vector? Embedding { get; set; }
}
