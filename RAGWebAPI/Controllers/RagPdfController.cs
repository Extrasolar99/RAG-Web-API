using Microsoft.AspNetCore.Mvc;
using RAGWebAPI.Services;

namespace RAGWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RagPdfController(IRagPdfService ragPdfService) : ControllerBase
{
    private readonly IRagPdfService _ragPdfService = ragPdfService;

    [HttpPost("file")]
    public async Task<IActionResult> AddRagPdfDocument(IFormFile file)
    {
        var serviceResult = await _ragPdfService.AddPdfDocument(file);

        if (serviceResult.IsSuccess)
        {
            return Ok(serviceResult.Data);
        }

        return BadRequest(serviceResult.Error);
    }

    [HttpGet("prompt")]
    public async Task<IActionResult> SearchPdfPagesByPrompt([FromQuery] string prompt)
    {
        var serviceResult = await _ragPdfService.SearchPdfPagesByPrompt(prompt);

        if (serviceResult.IsSuccess)
        {
            return Ok(serviceResult.Data);
        }

        return BadRequest(serviceResult.Error);
    }
}
