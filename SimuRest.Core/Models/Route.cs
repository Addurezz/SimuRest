namespace SimuRest.Core.Models;

public class Route
{
    public HttpMethod Method { get; }
    public string Path { get; }
    public Route(HttpMethod method, string path)
    {
        if (String.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path cannot be null or empty.", nameof(path));
        if (method is null) throw new ArgumentException("Method cannot be null.", nameof(method));
        
        Method = method;
        Path = path;
    }
}