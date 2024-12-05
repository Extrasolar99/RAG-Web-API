namespace RAGWebAPI.Models;

public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public string? Error { get; set; }

    public static ServiceResult<T> Success(T? data, string message = "", int statusCode = 200) => new()
    {
        IsSuccess = true,
        Data = data,
        Message = message,
        StatusCode = statusCode
    };

    public static ServiceResult<T> Failure(string message, int statusCode = 400, string? error = null) => new()
    {
        IsSuccess = false,
        Message = message,
        StatusCode = statusCode,
        Error = error
    };
}
