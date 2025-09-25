namespace SimuRest.Core.Models;

/// <summary>
/// Represents the response from the server.
/// </summary>
public class SimuResponse
{
    /// <summary>
    /// Gets the status code of the <see cref="SimuResponse"/>. 
    /// </summary>
    public int StatusCode { get; }
    
    /// <summary>
    /// Gets the body of the <see cref="SimuResponse"/>.
    /// </summary>
    public string Body { get; }

    /// <summary>
    /// A pre-defined <see cref="SimuResponse"/> representing a 404 Not Found response.
    /// </summary>
    public static SimuResponse NotFound => new SimuResponse(404, "Not Found");
    
    /// <summary>
    /// A pre-defined <see cref="SimuResponse"/> representing a 400 Bad Request response.
    /// </summary>
    public static SimuResponse BadRequest => new SimuResponse(400, "Bad Request");
    
    /// <summary>
    /// Initializes a new instance of a <see cref="SimuResponse"/>.
    /// </summary>
    /// <param name="status">The status code of the <see cref="SimuResponse"/>.</param>
    /// <param name="body">The content of the <see cref="SimuResponse"/>.</param>
    public SimuResponse(int status, string body)
    {
        StatusCode = status;
        Body = body;
    }

    public override bool Equals(object? obj)
    {
        if (obj?.GetType() == this.GetType()) return this.Equals((SimuResponse)obj);

        return base.Equals(obj);
    }

    protected bool Equals(SimuResponse other)
    {
        return StatusCode == other.StatusCode && Body == other.Body;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StatusCode, Body);
    }
}