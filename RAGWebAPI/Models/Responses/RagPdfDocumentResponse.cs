namespace RAGWebAPI.Models.Responses;

public class RagPdfDocumentResponse
{
    public int Id { get; set; }
    public int PageCount { get; set; }
    public string Title { get; set; } = "";
    public List<RagPdfPageResponse> Pages { get; set; } = [];
}
