namespace RAGWebAPI.Services;

public interface IGenerativeService
{
    public Task<string> GenerateResponseWithData(string prompt, string jsonData);
}
