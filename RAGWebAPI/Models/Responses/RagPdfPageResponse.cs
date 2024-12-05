using Pgvector;
using RAGWebAPI.Models.Entities;

namespace RAGWebAPI.Models.Responses;

public class RagPdfPageResponse
{
    public int Id { get; set; }
    public int RagPdfDocumentId { get; set; }
    public RagPdfDocumentResponse? RagPdfDocumentResponse { get; set; }
    public int PageNumber { get; set; }
    public string Content { get; set; } = "";

    public Vector? Embedding { get; set; }
}
