using System.Net;

namespace SimuRest.Core.Services.Http;

public class SimuServer
{
    public RequestHandler Handler;
    public Router Router;
    public ResponseWriter Writer;
    public Parser Parser;
    public HttpListener Listener;

    private readonly object _tasksLock = new();
    private List<Task> _activeRequests = new List<Task>();
    private int _port = 5000;
    private CancellationTokenSource _cts = new();

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
        // Setup the listener
        Listener.Start();
        Listener.Prefixes.Add($"http://localhost:{_port}/");

        HttpListenerContext context;
        
        // Accept incoming requests and handle them as long as the token doesnt cancel the inflow
        while (Listener.IsListening && !_cts.IsCancellationRequested)
        {
            try
            {
                // Get the request the user sent
                context = await Listener.GetContextAsync();
            }
            catch (Exception e)
            {
                break;
            }
            
            // Handle the user request on a different thread
            Task requestTask = Handler.Handle(context);
            
            // Add the current request to keep track of active requests
            lock(_tasksLock)
            {
                _activeRequests.Add(requestTask);
            }

            _ = requestTask.ContinueWith(t =>
            {
                lock (_tasksLock)
                {
                    _activeRequests.Remove(t);
                }
            });
        }
        
        
    }

    public async Task Stop()
    {
        await _cts.CancelAsync();
        
        // stops the flow of incoming requests but does not close the connection yet
        Listener.Stop();

        // array of all the remaining active tasks
        Task[] tasks;
        lock (_tasksLock)
        {
            tasks = _activeRequests.ToArray();
        }
        
        // Await that every task in 'tasks' is finished
        await Task.WhenAll(tasks);
        // Close the listener
        Listener.Close();
    }
}