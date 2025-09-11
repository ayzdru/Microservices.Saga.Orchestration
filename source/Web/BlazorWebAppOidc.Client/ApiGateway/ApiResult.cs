using System.Text.Json.Serialization;

namespace BlazorWebAppOidc.Client;

public class ApiResult<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }

    public T Data { get; set; }

    public ApiResult(bool success, T data)
    {
        Data = data;
        Success = success;
    }
    [JsonConstructor]
    public ApiResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}