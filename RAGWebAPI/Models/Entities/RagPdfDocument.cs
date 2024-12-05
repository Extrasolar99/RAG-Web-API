namespace RAGWebAPI.Models.Entities;

public class RagPdfDocument
{
    public int Id { get; set; }
    public int PageCount { get; set; }
    public string Title { get; set; } = "";

    public virtual List<RagPdfPage> RagPdfPages { get; set; } = [];
}
