using OllamaSharp;
using OllamaSharp.Models;

namespace RAGWebAPI.Services;

public class GenerativeService(OllamaApiClient ollamaApiClient) : IGenerativeService
{
    private readonly OllamaApiClient _ollamaApiClient = ollamaApiClient;

    public async Task<string> GenerateResponseWithData(string prompt, string data)
    {
        _ollamaApiClient.SelectedModel = Environment.GetEnvironmentVariable("OLLAMA_GENERATIVE_MODEL") ?? "llama3.1:8b";

        string chatResponse = "";
        await foreach (var stream in _ollamaApiClient.GenerateAsync(new GenerateRequest()
        {
            Model = Environment.GetEnvironmentVariable("OLLAMA_GENERATIVE_MODEL")!,
            Prompt = $"Using this data: {data}. Respond to this prompt: {prompt}"
        }))
        {
            if (stream != null) chatResponse += stream.Response;
        }

        return chatResponse;
    }
}
