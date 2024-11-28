using Microsoft.AspNetCore.Mvc;
using OllamaSharp;
using OllamaSharp.Models;

namespace RAGWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    //private static readonly string[] Summaries = new[]
    //{
    //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    //};

    private readonly ILogger<WeatherForecastController> _logger;

    public TestController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        var uri = "http://host.docker.internal:11434";
        var ollama = new OllamaApiClient(uri);

        ollama.SelectedModel = "mxbai-embed-large";

        EmbedResponse response = await ollama.EmbedAsync("How are you today?");

        return Ok(response.Embeddings);
    }
}
