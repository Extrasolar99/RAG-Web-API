using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using Pgvector;
using OllamaSharp;
using OllamaSharp.Models;
using RAGWebAPI.Database;
using RAGWebAPI.Models.Responses;
using RAGWebAPI.Models.Entities;

namespace RAGWebAPI.Services;


public class EmbeddingService(RAGDbContext context) : IEmbeddingService
{
    private readonly RAGDbContext _context = context;

    public async Task<bool> AddDocument(string text)
    {
        var uri = "http://host.docker.internal:11434";
        var ollama = new OllamaApiClient(uri);

        ollama.SelectedModel = "mxbai-embed-large";

        EmbedResponse response = await ollama.EmbedAsync(text);
        
        await _context.Documents.AddAsync(new Document { Embedding = new Vector(response.Embeddings.SelectMany(e => e).ToArray()), Content = text });
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<DocumentResponse>> SearchDocument(string prompt)
    {
        var uri = "http://host.docker.internal:11434";
        var ollama = new OllamaApiClient(uri);

        ollama.SelectedModel = "mxbai-embed-large";

        EmbedResponse response = await ollama.EmbedAsync(prompt);
        Vector responseVector = new Vector(response.Embeddings.SelectMany(e => e).ToArray());

        var docsDistance = await _context.Documents
            .Select(x => new { Entity = x, Distance = x.Embedding!.L2Distance(responseVector) })
            .OrderBy(x => x.Distance)
            .Take(5)
            .ToListAsync();

        var documentResponses = docsDistance.Select(doc => new DocumentResponse
        {
            Id = doc.Entity.Id,
            Content = doc.Entity.Content,
            Distance = doc.Distance
        }).ToList();

        return documentResponses;
    }
}
