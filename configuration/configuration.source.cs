using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace DockerSecret.Configuration
{
    public class DockerSecretConfigurationSource : IConfigurationSource
    {
        private readonly Options options;

        public DockerSecretConfigurationSource(
            Options options)
        {
            this.options = options;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DockerSecretConfigurationProvider(this.options);
        }
    }
}