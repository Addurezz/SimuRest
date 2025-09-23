using System.Net;

namespace SimuRest.Core.Services.Http;

public class SimuServer
{
    public RequestHandler Handler;
    public Router Router;
    public ResponseWriter Writer;
    public Parser Parser;
    public HttpListener Listener;

    private int _port = 5000;

    public int Port
    {
        get => _port;
        set
        {
            if (value is < 1024 or > 49151)
                throw new ArgumentException("Ports must be be between 1024 and 49151", nameof(value));

            _port = value;
        }
    }

    public SimuServer(Router router, ResponseWriter writer, Parser parser)
    {
        Parser = parser;
        Router = router;
        Writer = writer;
        Handler = new RequestHandler(Router, Parser, Writer);
        Listener = new HttpListener();
    }

    public async Task Start()
    {
        try
        {
            Listener.Prefixes.Add($"http://localhost:{Port}/");
            Listener.Start();
        
            while (Listener.IsListening)
            {
                HttpListenerContext context = await Listener.GetContextAsync(); // blockiert auf Request

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await Handler.Handle(context);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Oops, Something went wrong with the server.\n" + e);
            this.Stop();
        }
    }
    
    public void Stop()
    {
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