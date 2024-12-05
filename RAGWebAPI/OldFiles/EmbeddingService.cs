//using Microsoft.EntityFrameworkCore;
//using Pgvector.EntityFrameworkCore;
//using Pgvector;
//using OllamaSharp;
//using OllamaSharp.Models;
//using RAGWebAPI.Database;
//using RAGWebAPI.Models.Responses;
//using RAGWebAPI.Models.Entities;

//namespace RAGWebAPI.Services;


//public class EmbeddingService(RAGDbContext context, OllamaApiClient ollamaApiClient) : IEmbeddingService
//{
//    private readonly RAGDbContext _context = context;
//    private readonly OllamaApiClient _ollamaApiClient = ollamaApiClient;

//    public async Task<bool> AddDocument(string text)
//    {
//        _ollamaApiClient.SelectedModel = "mxbai-embed-large";

//        EmbedResponse response = await _ollamaApiClient.EmbedAsync(text);
        
//        await _context.Documents.AddAsync(new Document { Embedding = new Vector(response.Embeddings.SelectMany(e => e).ToArray()), Content = text });
//        await _context.SaveChangesAsync();

//        return true;
//    }

//    public async Task<bool> AddDocuments(string[] texts)
//    {
//        _ollamaApiClient.SelectedModel = "mxbai-embed-large";

//        List<Document> newDocuments = [];
//        foreach (string text in texts)
//        {
//            EmbedResponse response = await _ollamaApiClient.EmbedAsync(text);

//            Document newDoc = new() { Embedding = new Vector(response.Embeddings.SelectMany(e => e).ToArray()), Content = text };

//            newDocuments.Add(newDoc);
//        }

//        await _context.Documents.AddRangeAsync(newDocuments);
//        await _context.SaveChangesAsync();

//        return true;
//    }

//    public async Task<List<DocumentResponse>> SearchDocument(string prompt)
//    {
//        _ollamaApiClient.SelectedModel = "mxbai-embed-large";

//        EmbedResponse response = await _ollamaApiClient.EmbedAsync(prompt);
//        Vector responseVector = new(response.Embeddings.SelectMany(e => e).ToArray());

//        var docsDistance = await _context.Documents
//            .Select(x => new { Entity = x, Distance = x.Embedding!.L2Distance(responseVector) })
//            .OrderBy(x => x.Distance)
//            .Take(5)
//            .ToListAsync();

//        var documentResponses = docsDistance.Select(doc => new DocumentResponse
//        {
//            Id = doc.Entity.Id,
//            Content = doc.Entity.Content,
//            Distance = doc.Distance
//        }).ToList();

//        return documentResponses;
//    }
//}
