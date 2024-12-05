using Pgvector;

namespace RAGWebAPI.Services;

public interface IEmbedService
{
    public Task<Vector> GenerateVector(string prompt);
}
