namespace SimuRest.Core.Models;

public class SimuRequest
{
    public string Path { get; }
    public HttpMethod Method { get; }

    public SimuRequest(HttpMethod method, string path)
    {
        if (String.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path cannot be null or empty.", nameof(path));
        if (method is null) throw new ArgumentException("Method cannot be null.", nameof(method));
        
        Path = path;
        Method = method;
    }
}