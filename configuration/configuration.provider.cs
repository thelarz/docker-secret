using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace DockerSecret.Configuration
{
    public class DockerSecretConfigurationProvider : ConfigurationProvider
    {
        private readonly Options options = new();
        private readonly List<string> PrivateDataKeys = new List<string> {
            "pass", "secret", "token", "private", "password"
        };

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
                            if (PrivateDataKeys.Any(x => kv.Key.Split('.').Last().Contains(x, StringComparison.OrdinalIgnoreCase)))
                                Console.WriteLine($"{kv.Key} : *redacted*");
                            else
                                Console.WriteLine($"{kv.Key} : {kv.Value}");
                        }
                    }
                }
                if (secret.Type == typeof(string)) {
                    this.Data.Add(secret.Name, value);
                    if (options.Logging) {
                        if (PrivateDataKeys.Any(x => secret.Name.Contains(x, StringComparison.OrdinalIgnoreCase)))
                        {
                            Console.WriteLine($"{secret.Name} : *redacted*");
                            return;
                        }
                        value = Regex.Replace(value ?? "", "Password=([^;]*;)", "password=*redacted*;", RegexOptions.IgnoreCase);
                        Console.WriteLine($"{secret.Name} : {value}");
                    }
                }
            }

        }
    }
}