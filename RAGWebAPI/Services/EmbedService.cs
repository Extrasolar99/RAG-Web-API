using OllamaSharp;
using Pgvector;

namespace RAGWebAPI.Services;

public class EmbedService(OllamaApiClient ollamaApiClient) : IEmbedService
{
    private readonly OllamaApiClient _ollamaApiClient = ollamaApiClient;

    public async Task<Vector> GenerateVector(string prompt)
    {
        _ollamaApiClient.SelectedModel = Environment.GetEnvironmentVariable("OLLAMA_EMBED_MODEL") ?? "mxbai-embed-large";
        var embeddingResponse = await _ollamaApiClient.EmbedAsync(prompt);

        Vector embedding = new(embeddingResponse.Embeddings.SelectMany(e => e).ToArray());

        return embedding;
    }
}
