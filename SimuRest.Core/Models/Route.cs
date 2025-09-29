namespace SimuRest.Core.Models;


/// <summary>
/// Displays a route to the server with a HttpMethod and a path.
/// </summary>
public class Route
{
    /// <summary>
    /// Gets the <see cref="HttpMethod"/>.
    /// </summary>
    public HttpMethod Method { get; }
    
    /// <summary>
    /// Gets the path.
    /// </summary>
    public string Path { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Route"/> class.
    /// </summary>
    /// <param name="method">Method to determine what the server should do on incoming requests.</param>
    /// <param name="path">Path the method is working on.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="path"/> is null or empty</exception>
    /// <exception cref="ArgumentNullException"> Thrown if <paramref name="method"/> is null</exception>
    public Route(HttpMethod method, string path)
    {
        if (String.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path cannot be null or empty.", nameof(path));
        if (method is null) throw new ArgumentNullException();
        
        Method = method;
        Path = path;
    }
}