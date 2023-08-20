using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

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
                if (value != null) {
                    this.Data.Add(secret.Name, value);
                }
            }

        }
    }
}