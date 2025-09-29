using System.Net;
using SimuRest.Core.Models;
using SimuRest.Core.Services.Perseverance;

namespace SimuRest.Core.Services.Http;


/// <summary>
/// Represents a small and lightweight Http simulation server that listens to incoming requests and dispatches them to the configured <see cref="Router"/> and <see cref="RequestHandler"/>.
/// </summary>
public class SimuServer
{
    /// <summary>
    /// Gets the <see cref="ServiceProvider"/>.
    /// </summary>
    public ServiceProvider ServiceProvider { get; }
    
    /// <summary>
    /// Gets the <see cref="HttpListener"/>.
    /// </summary>
    public HttpListener Listener { get; }
    
    /// <summary>
    /// Gets the <see cref="RequestHandler"/>.
    /// </summary>
    public RequestHandler Handler { get; }
    
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
    
    // the lock for editing '_activeRequests' in multi-threading
    private readonly object _tasksLock = new();
    
    // list of all currently active requests
    private readonly List<Task> _activeRequests = new List<Task>();
    
    // the user port the server listens to
    private int _port = 5000;
    
    // the token to cancel the listening for requests and stopping the server
    private CancellationTokenSource _cts = new();

    /// <summary>
    /// Initializes a new instance of a <see cref="SimuServer"/>.
    /// </summary>
    /// <param name="provider">The <see cref="ServiceProvider"/> that stores the services.</param>
    public SimuServer(ServiceProvider provider)
    {
        Handler = new RequestHandler();
        Listener = new HttpListener();
        ServiceProvider = provider;
    }

    /// <summary>
    /// Starts the server and the <see cref="Listener"/> on a specific <see cref="Port"/> as an asynchronous operation.
    /// </summary>
    public async Task Start()
    {
        // Set up the listener
        Listener.Prefixes.Add($"http://localhost:{_port}/");
        Listener.Start();

        HttpListenerContext context;
        
        // Accept incoming requests and handle them as long as the token doesn't cancel the inflow
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
            Task requestTask = Handler.Handle(context, ServiceProvider);
            
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