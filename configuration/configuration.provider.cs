using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace DockerSecret.Configuration
{
    public class DockerSecretConfigurationProvider : ConfigurationProvider
    {
        private readonly Options options = new();

        public DockerSecretConfigurationProvider(
            Options options)
        {
            this.options = options ?? Options.DEFAULT;
        }

        public override void Load()
        {
            var secretProvider = new SecretProvider(this.options.Location);
            foreach(var secret in options.Secrets!) {
                var value = Task.Run(async () => {
                    return await secretProvider.Get<string>(secret.Name);
                }).Result;
                if (secret.Type == typeof(object)) {
                    var flattened = JObject.Parse(value)
                        .Descendants()
                        .OfType<JValue>()
                        .ToDictionary(jv => $"{secret.Name}.{jv.Path}", jv => jv.ToString());
                    foreach (var kv in flattened) {
                        this.Data.Add(kv.Key, kv.Value);
                        if (options.Logging) {
                            Console.WriteLine($"{kv.Key} : {kv.Value}");
                        }
                    }
                }
                if (secret.Type == typeof(string)) {
                    this.Data.Add(secret.Name, value);
                    if (options.Logging) {
                        Console.WriteLine($"{secret.Name} : {value}");
                    }
                }
            }

        }
    }
}