using Microsoft.Extensions.Configuration;

namespace DockerSecret.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddDockerSecret(
            this IConfigurationBuilder builder,
            Options options)
        {

            builder.Add(new DockerSecretConfigurationSource(options));
            return builder;
        }

        public static IConfigurationBuilder AddDotNetEnvMulti(
            this IConfigurationBuilder builder,
            Options options)
        {
            builder.Add(new DockerSecretConfigurationSource(options));
            return builder;
        }
    }
}