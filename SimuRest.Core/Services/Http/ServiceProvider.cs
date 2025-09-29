namespace SimuRest.Core.Services.Http;

/// <summary>
/// Provides access to services used by <see cref="SimuServer"/> and its components.
/// </summary>
public class ServiceProvider
{
    private Dictionary<Type, object> _services = new Dictionary<Type, object>();
    
    /// <summary>
    /// Registers a service. 
    /// </summary>
    /// <param name="obj">The service to store.</param>
    /// <typeparam name="T">The type of the service.</typeparam>
    /// <exception cref="ArgumentNullException">Throws if <paramref name="obj"/> is null.</exception>
    public void Register<T>(T obj)
    {
        if (obj is null) throw new ArgumentNullException();
        _services.TryAdd(typeof(T), obj);
    }
    
    /// <summary>
    /// Gets a service.
    /// </summary>
    /// <typeparam name="T">The type of the service.</typeparam>
    /// <returns>The service with type T</returns>
    /// <exception cref="InvalidOperationException">Throws of the service does not exist.</exception>
    public T GetService<T>()
    {
        if (_services.TryGetValue(typeof(T), out var value))
            return (T)value;

        throw new InvalidOperationException();
    }
}