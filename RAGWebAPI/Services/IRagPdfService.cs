using RAGWebAPI.Models.Entities;
using RAGWebAPI.Models;
using RAGWebAPI.Models.Responses;

namespace RAGWebAPI.Services;

public interface IRagPdfService
{
    public Task<ServiceResult<RagPdfDocumentResponse>> AddPdfDocument(IFormFile file);
    public Task<ServiceResult<string>> SearchPdfPagesByPrompt(string prompt);
}
