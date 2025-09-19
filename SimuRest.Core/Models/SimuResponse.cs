namespace SimuRest.Core.Models;

public class SimuResponse
{
    public int StatusCode { get; }
    public string Body { get; }

    public SimuResponse(int status, string body)
    {
        StatusCode = status;
        Body = body;
    }

    public override bool Equals(object? obj)
    {
        if (obj.GetType() == this.GetType()) return this.Equals((SimuResponse)obj);

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