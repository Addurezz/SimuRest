namespace SimuRest.Core.Models;

public class SimuResponse
{
    public int StatusCode { get; }
    public string Body { get; }

    public SimuResponse(int status, string body)
    {
        if (String.IsNullOrWhiteSpace(body)) throw new ArgumentException("Body cannot be null", nameof(body));
        StatusCode = status;
        Body = body;
    }
}