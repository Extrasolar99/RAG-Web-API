using OllamaSharp;
using Pgvector;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using RAGWebAPI.Database;
using RAGWebAPI.Models;
using RAGWebAPI.Models.Entities;
using RAGWebAPI.Models.Responses;
using OllamaSharp.Models;
using Pgvector.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace RAGWebAPI.Services;

public class RagPdfService(
    RAGDbContext context,
    IGenerativeService generativeService,
    IEmbedService embedService,
    OllamaApiClient ollamaApiClient
    ) : IRagPdfService
{
    private readonly RAGDbContext _context = context;
    private readonly IGenerativeService _generativeService = generativeService;
    private readonly IEmbedService _embedService = embedService;
    private readonly OllamaApiClient _ollamaApiClient = ollamaApiClient;

    public async Task<ServiceResult<RagPdfDocumentResponse>> AddPdfDocument(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return ServiceResult<RagPdfDocumentResponse>.Failure("No file uploaded");
        }

        if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
        {
            return ServiceResult<RagPdfDocumentResponse>.Failure("Only PDF files are allowed.");
        }

        RagPdfDocument? savedDocument = default;
        using (var pdf = PdfDocument.Open(file.OpenReadStream()))
        {
            string title = pdf.Information.Title ?? "";
            int pageCount = pdf.NumberOfPages;

            RagPdfDocument newDocument = new() { Title = title, PageCount = pageCount };
            savedDocument = newDocument;

            await _context.RagPdfDocuments.AddAsync(newDocument);
            await _context.SaveChangesAsync();

            List<RagPdfPage> pagesToAdd = [];
            foreach (var page in pdf.GetPages())
            {
                var text = ContentOrderTextExtractor.GetText(page);
                Vector embedding = await _embedService.GenerateVector(text);

                RagPdfPage newPage = new() 
                { 
                    Content = text,
                    RagPdfDocumentId = newDocument.Id,
                    PageNumber = page.Number,
                    Embedding = embedding
                };

                pagesToAdd.Add(newPage);
            }

            await _context.RagPdfPages.AddRangeAsync(pagesToAdd);
            await _context.SaveChangesAsync();
        }

        RagPdfDocumentResponse savedDocumentResponse = new()
        {
            Id = savedDocument.Id,
            Title = savedDocument.Title,
            PageCount = savedDocument.PageCount,
            Pages = savedDocument.RagPdfPages.Select(p => new RagPdfPageResponse
            {
                Id = p.Id,
                RagPdfDocumentId = p.RagPdfDocumentId,
                PageNumber = p.PageNumber,
                Content = p.Content,
                Embedding = p.Embedding
            }).ToList()
        };

        return ServiceResult<RagPdfDocumentResponse>.Success(savedDocumentResponse, $"Document and pages added successfully");
    }

    public async Task<ServiceResult<string>> SearchPdfPagesByPrompt(string prompt)
    {
        Vector promptEmbedding = await _embedService.GenerateVector(prompt);

        var pagesDistance = await _context.RagPdfPages
            .Include("RagPdfDocument")
            .Select(x => new { Entity = x, Distance = x.Embedding!.L2Distance(promptEmbedding) })
            .OrderBy(x => x.Distance)
            .Take(5)
            .ToListAsync();

        var pageResponses = pagesDistance.Select(page => new RagPdfPageResponse
        {
            Id = page.Entity.Id,
            RagPdfDocumentId = page.Entity.RagPdfDocumentId,
            RagPdfDocumentResponse = new RagPdfDocumentResponse
            {
                Id = page.Entity.RagPdfDocument!.Id,
                Title = page.Entity.RagPdfDocument.Title,
                PageCount = page.Entity.RagPdfDocument.PageCount
            },
            PageNumber = page.Entity.PageNumber,
            Content = page.Entity.Content,
            Embedding = page.Entity.Embedding
        }).ToList();

        string formattedResults = JsonConvert.SerializeObject(pageResponses);
        var chatResponse = await _generativeService.GenerateResponseWithData(prompt, formattedResults);

        return ServiceResult<string>.Success(chatResponse);
    }
}

