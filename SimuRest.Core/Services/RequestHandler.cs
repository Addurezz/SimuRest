using System.Net;
using SimuRest.Core.Models;

namespace SimuRest.Core.Services;

public class RequestHandler
{
    private Router Router { get; }
    
    public SimuRequest Parse(HttpListenerContext ctx)
    {
        throw new NotImplementedException();
    }
    
    public SimuResponse ProcessRequest(SimuRequest req)
    {
        throw new NotImplementedException();
    }

    public void Handle(HttpListenerContext ctx)
    {
        
    }
}