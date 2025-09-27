
namespace SimuRest.Core.Services.Perseverance;

/// <summary>
/// Provides access to save and retrieve information related to server requests.
/// </summary>
public class ServerMemory
{
    private Dictionary<string, object?> _memory = new Dictionary<string, object?>();

    /// <summary>
    /// Tries to add data to the memory.
    /// </summary>
    /// <param name="path">The <paramref name="path"/> to store the data.</param>
    /// <param name="data">The data to be stored.</param>
    public void Save(string path, object data)
    {
        _memory.TryAdd(path, data);
    }

    /// <summary>
    /// Gets the data for the specified key (<paramref name="path"></paramref>).
    /// </summary>
    /// <param name="path">The key to look the data for.</param>
    /// <returns>Null or <see cref="Object"/></returns>
    /// <exception cref="KeyNotFoundException">Throws if the key does not exist.</exception>
    public object? Get(string path)
    {
        if (!_memory.TryGetValue(path, out var val)) throw new KeyNotFoundException();

        return val;
    }
}