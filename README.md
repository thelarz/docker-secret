# docker-secret
A dotnet core 6 library that will read Kubernetes style secrets from a file system folder


Installing the package
```
> dotnet add package DockerSecret
```


Usage
```
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DockerSecret;
using SecretConsumer;

await Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostcontext, services) => {
            services
                .AddHostedService<SecretConsumerApp>()
                .AddSingleton<SecretProvider, SecretProvider>(
                    x => new SecretProvider("DEV-SECRETS")
                );
        })
        .RunConsoleAsync();   
```


```
var dbName = await _secretProvider.Get<string>("DBNAME");
Console.WriteLine(dbName);
```

Using DockerSecret as a configuration provider
```
IConfiguration configuration = new ConfigurationBuilder()
    .AddDockerSecret(
            new Options()
                .FromLocation("test-secrets")
                .Load("DBNAME")
                .Load("PASSWORD")
        )
    .Build();
```

Credit to https://github.com/tonerdo/dotnet-env for reference.