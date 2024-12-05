using System.ComponentModel.DataAnnotations.Schema;
using Pgvector;

namespace RAGWebAPI.Models.Entities;

public class Document
{
    public int Id { get; set; }
    public string Content { get; set; } = "";

    [Column(TypeName = "vector(1024)")]
    public Vector? Embedding { get; set; }
}
