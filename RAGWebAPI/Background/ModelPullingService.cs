using OllamaSharp;

namespace RAGWebAPI.Background;

public class ModelPullingService(OllamaApiClient ollamaApiClient) : BackgroundService
{
    private readonly OllamaApiClient _ollamaApiClient = ollamaApiClient;
    private readonly string _embedModelName = Environment.GetEnvironmentVariable("OLLAMA_EMBED_MODEL") ?? "mxbai-embed-large";
    private readonly string _generativeModelName = Environment.GetEnvironmentVariable("OLLAMA_GENERATIVE_MODEL") ?? "llama3.1:8b";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await PullModelAsync(_embedModelName, stoppingToken);
        await PullModelAsync(_generativeModelName, stoppingToken);
    }

    private async Task PullModelAsync(string modelName, CancellationToken stoppingToken)
    {
        try
        {
            await foreach (var resp in _ollamaApiClient.PullModelAsync(modelName).WithCancellation(stoppingToken))
            {
                Console.WriteLine($"Pulling {modelName} model, {resp!.Percent} complete");
            }
            return; // Exit the method if successful
        }
        catch (Exception ex)
        {
            Console.WriteLine("Something went wrong while trying to pull the Ollama models", ex.Message);
            throw;
        }
    }
}