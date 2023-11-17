using System.Text.Json;

namespace backend.Models;

public class AppException : Exception
{
    public int StatusCode { get; set; }


    public AppException( string message, int statusCode = 400) : base(message)
    {
        StatusCode = statusCode;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(new {
            title = "Error",
            statusCode = StatusCode,
            message = Message
        });
    }

}