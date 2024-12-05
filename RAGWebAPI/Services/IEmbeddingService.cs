using RAGWebAPI.Models.Responses;

namespace RAGWebAPI.Services;

public interface IEmbeddingService
{
    public Task<bool> AddDocument(string text);
    public Task<List<DocumentResponse>> SearchDocument(string prompt);
}
