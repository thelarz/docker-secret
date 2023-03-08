using System.IO;

namespace DockerSecret;

public class SecretProvider
{
    private string? _location;

    public async Task<string?> Get(string name) {
        return await this.Get<string>(name);
    }
    public async Task<T?> Get<T>(string name) {

        var location = _location ?? "/run/secrets";
        var file = location + "/" + name;

        if (!File.Exists(file)) {
            return default(T);
        }

        string value =  (await System.IO.File.ReadAllTextAsync(file)).Trim();
        return (T) Convert.ChangeType(value, typeof(T));
    }

    public string GetLocation() {
        return this._location ?? "/run/secrets";
    }

    public SecretProvider(string? location = null) {
        this._location = location;
    }
}
