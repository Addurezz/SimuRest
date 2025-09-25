using System.Net;
using SimuRest.Core.Models;
namespace SimuRest.Core.Services.Http;


/// <summary>
/// Represents a small and lightweight Http simulation server that listens to incoming requests and dispatches them to the configured <see cref="Router"/> and <see cref="RequestHandler"/>.
/// </summary>
public class SimuServer
{
    /// <summary>
    /// Gets the configured <see cref="RequestHandler"/>.
    /// </summary>
    public RequestHandler Handler { get; }

    /// <summary>
    /// Gets the configured <see cref="Router"/>.
    /// </summary>
    public Router Router { get; }

    /// <summary>
    /// Gets the configured <see cref="ResponseWriter"/>.
    /// </summary>
    public ResponseWriter Writer { get; }

    /// <summary>
    /// Gets the configured <see cref="Parser"/>.
    /// </summary>
    public Parser Parser { get; }

    /// <summary>
    /// Gets the <see cref="HttpListener"/>.
    /// </summary>
    public HttpListener Listener;
    
    // the lock for editing '_activeRequests' in multi-threading
    private readonly object _tasksLock = new();
    
    // list of all currently active requests
    private List<Task> _activeRequests = new List<Task>();
    
    // the user port the server listens to
    private int _port = 5000;
    
    // the token to cancel the listening for requests and stopping the server
    private CancellationTokenSource _cts = new();

    /// <summary>
    /// Sets the port the server is listening to. 
    /// </summary>
    /// <exception cref="ArgumentException"> Throws if the port is smaller than 1024 or bigger than 49151.</exception>
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

    /// <summary>
    /// Initializes a new instance of a <see cref="SimuServer"/>.
    /// </summary>
    /// <param name="router">The <see cref="Router"/> that matches with <see cref="RouteTable"/> for <see cref="SimuRequest"/>.</param>
    /// <param name="writer">The <see cref="ResponseWriter"/> that writes a specified file stream to the <see cref="HttpListenerResponse"/>.</param>
    /// <param name="parser">The <see cref="Parser"/> that that translates incoming requests into <see cref="SimuRequest"/>.</param>
    public SimuServer(Router router, ResponseWriter writer, Parser parser)
    {
        Parser = parser;
        Router = router;
        Writer = writer;
        Handler = new RequestHandler(Router, Parser, Writer);
        Listener = new HttpListener();
    }

    /// <summary>
    /// Starts the server and the <see cref="Listener"/> on a specific <see cref="Port"/> as an asynchronous operation.
    /// </summary>
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
    
    /// <summary>
    /// Stops the <see cref="Listener"/> and waits for the remaining tasks to finish. Closes the <see cref="Listener"/> right after. 
    /// </summary>
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