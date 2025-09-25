namespace SimuRest.Core.Models;


/// <summary>
/// Represents the request the user sent to the server.
/// </summary>
public class SimuRequest
{
    /// <summary>
    /// Gets the path of the <see cref="SimuRequest"/>.
    /// </summary>
    public string Path { get; }
    
    /// <summary>
    /// Gets the <see cref="HttpMethod"/> of the <see cref="SimuRequest"/>.
    /// </summary>
    public HttpMethod Method { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="SimuRequest"/>
    /// </summary>
    /// <param name="method">The <see cref="HttpMethod"/> of the <see cref="SimuRequest"/>.</param>
    /// <param name="path">The <paramref name="path"/> of the <see cref="SimuRequest"/>.</param>
    /// <exception cref="ArgumentException">Throws if <paramref name="path"/> is null or empty.</exception>
    /// <exception cref="ArgumentNullException">Throws if <paramref name="method"/> is null.</exception>
    public SimuRequest(HttpMethod method, string path)
    {
        if (String.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path cannot be null or empty.", nameof(path));
        if (method is null) throw new ArgumentNullException();
        
        Path = path;
        Method = method;
    }

    public override bool Equals(object? obj)
    {
        if (obj.GetType() == this.GetType()) return this.Equals((SimuRequest)obj);
        
        return base.Equals(obj);
    }

    protected bool Equals(SimuRequest other)
    {
        return Path == other.Path && Method.Equals(other.Method);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Path, Method);
    }
}