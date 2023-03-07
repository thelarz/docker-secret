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