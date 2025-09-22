using System.Net;

namespace SimuRest.Core.Services.Http;

public class SimuServer
{
    public RequestHandler Handler;
    public Router Router;
    public ResponseWriter Writer;
    public Parser Parser;
    public HttpListener Listener;
    
    public int Port { get; set; } = 5000;

    public SimuServer(Router router, ResponseWriter writer, Parser parser)
    {
        Parser = parser;
        Router = router;
        Writer = writer;
        Handler = new RequestHandler(Router, Parser, Writer);
        Listener = new HttpListener();
    }

    public void Start()
    {   
        Listener.Prefixes.Add($"http://localhost:{Port}/");
        Listener.Start();
        
        while (Listener.IsListening)
        {
            HttpListenerContext context = Listener.GetContext(); // blockiert auf Request
            
            Handler.Handle(context);
        }
    }

    public void Stop()
    {
        if (Listener == null) return;
        if (!Listener.IsListening) return;

        try
        {
            Listener.Stop();
            Listener.Close();
        }
        catch (ObjectDisposedException)
        {
            // Already disposed – ignore
        }
    }
}