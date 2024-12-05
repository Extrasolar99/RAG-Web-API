using Microsoft.AspNetCore.Mvc;
using OllamaSharp;
using OllamaSharp.Models;
using Pgvector.EntityFrameworkCore;
using System.Numerics;
using Pgvector;
using System.Numerics.Tensors;
using OllamaSharp.Models.Chat;
using RAGWebAPI.Services;
using Newtonsoft.Json;

namespace RAGWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController(IEmbeddingService embeddingService) : ControllerBase
{
    private readonly IEmbeddingService _embeddingService = embeddingService;

    //private static readonly string[] Summaries = new[]
    //{
    //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    //};

    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        var uri = "http://host.docker.internal:11434";
        var ollama = new OllamaApiClient(uri);

        ollama.SelectedModel = "mxbai-embed-large";

        EmbedResponse response = await ollama.EmbedAsync("How are you today?");

        //TensorPrimitives.CosineSimilarity(response.Embeddings, response.Embeddings);

        return Ok(response.Embeddings);
    }

    [HttpPost()]
    public async Task<IActionResult> AddDocument([FromQuery] string testDocument)
    {
        var uri = "http://host.docker.internal:11434";
        var ollama = new OllamaApiClient(uri);

        ollama.SelectedModel = "mxbai-embed-large";

        EmbedResponse response = await ollama.EmbedAsync("How are you today?");

        //TensorPrimitives.CosineSimilarity(response.Embeddings, response.Embeddings);

        return Ok(response.Embeddings);
    }

    [HttpGet("generative")]
    public async Task<IActionResult> TestGenerativeModel([FromQuery] string prompt)
    {
        var results = await _embeddingService.SearchDocument(prompt);
        string formattedResults = JsonConvert.SerializeObject(results);

        var uri = "http://host.docker.internal:11434";
        var ollama = new OllamaApiClient(uri);
        ollama.SelectedModel = "llama3.1:8b";

        var chat = new Chat(ollama);

        string response = "";
        await foreach (var stream in ollama.GenerateAsync(new GenerateRequest()
        {
            Model = "llama3.1:8b",
            Prompt = $"Using this data: {formattedResults}. Respond to this prompt: {prompt}"
        }))
        {
            response += stream.Response;
        }

        return Ok(response);
    }



    //[HttpPost()]
    //public async Task<IActionResult> PostTest([FromQuery] string testPrompt)
    //{
    //    string[] examples =
    //    {
    //        "What is an amphibian?",
    //        "Cos'è un anfibio?",
    //        "A frog is an amphibian.",
    //        "Frogs, toads, and salamanders are all examples.",
    //        "Amphibians are four-limbed and ectothermic vertebrates of the class Amphibia.",
    //        "They are four-limbed and ectothermic vertebrates.",
    //        "A frog is green.",
    //        "A tree is green.",
    //        "It's not easy bein' green.",
    //        "A dog is a mammal.",
    //        "A dog is a man's best friend.",
    //        "You ain't never had a friend like me.",
    //        "Rachel, Monica, Phoebe, Joey, Chandler, Ross",
    //    };

    //    var uri = "http://host.docker.internal:11434";
    //    var ollama = new OllamaApiClient(uri);

    //    ollama.SelectedModel = "mxbai-embed-large";

    //    EmbedResponse testResponse = await ollama.EmbedAsync(testPrompt);

    //    Pgvector.Vector testReponseVector = new(testResponse.Embeddings.SelectMany(e => e).ToArray());

    //    foreach (string example in examples)
    //    {
    //        EmbedResponse exampleResponse = await ollama.EmbedAsync(example);

    //        Console.WriteLine(testResponse.Embeddings.CosineDistance(exampleResponse.Embeddings));
    //    }

    //    return Ok(testResponse.Embeddings);
    //}
}
