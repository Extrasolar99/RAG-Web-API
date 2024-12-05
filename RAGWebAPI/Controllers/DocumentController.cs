using Microsoft.AspNetCore.Mvc;
using RAGWebAPI.Services;
using RAGWebAPI.Database;

namespace RAGWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController(IEmbeddingService embeddingService) : ControllerBase
{
    private readonly IEmbeddingService _embeddingService = embeddingService;

    [HttpPost()]
    public async Task<IActionResult> AddDocument([FromQuery] string document)
    {
        var response = await _embeddingService.AddDocument(document);

        return Ok(response);
    }

    [HttpGet()]
    public async Task<IActionResult> SearchDocument([FromQuery] string searchPrompt)
    {
        var response = await _embeddingService.SearchDocument(searchPrompt);

        return Ok(response);
    }
}

