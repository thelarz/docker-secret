using Newtonsoft.Json;

namespace DockerSecret;

public class SecretProvider : ISecretProvider
{
    private readonly string? _location;

    public async Task<string?> Get(string name)
    {
        return await this.Get<string>(name);
    }
    public async Task<T?> Get<T>(string name)
    {
        var location = _location ?? "/run/secrets";
        var file = location + "/" + name;

        if (!File.Exists(file))
        {
            return default;
        }

        string value = (await File.ReadAllTextAsync(file)).Trim();
        if (typeof(T).IsClass && typeof(T) != typeof(string))
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        return (T)Convert.ChangeType(value, typeof(T));
    }

    public string GetLocation()
    {
        return this._location ?? "/run/secrets";
    }

    public SecretProvider(string? location = null)
    {
        this._location = location;
    }
}


public interface ISecretProvider
{
    Task<string?> Get(string name);
    Task<T?> Get<T>(string name);
    string GetLocation();
}