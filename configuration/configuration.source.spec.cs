using System;
using DockerSecret.Configuration;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DockerSecret.Tests
{

    public class DockerSecretConfigurationTests
    {
        private IConfigurationRoot? configuration;

        [Fact]
        public void AddSourceToBuilderAndLoad()
        {
            this.configuration = new ConfigurationBuilder()
                .AddDockerSecret(
                    new Options()
                        .FromLocation("test-secrets")
                        .Load("TEST-SECRET")
                )
                .Build();

            Assert.Equal("TEST VALUE", this.configuration["TEST-SECRET"]);
        }

        [Fact]
        public void LoadAndGetATypedValue()
        {
            this.configuration = new ConfigurationBuilder()
                .AddDockerSecret(
                    new Options()
                        .FromLocation("test-secrets")
                        .Load("TEST-BOOL")
                        .Load("TEST-INT")
                )
                .Build();
            Assert.True(Convert.ToBoolean(this.configuration["TEST-BOOL"]));
            Assert.Equal(23, Convert.ToInt32(this.configuration["TEST-INT"]));
        }

        [Fact]
        public void LoadAndGetAJsonValue()
        {
            this.configuration = new ConfigurationBuilder()
                .AddDockerSecret(
                    new Options()
                        .FromLocation("test-secrets")
                        .LoadJson("TEST-JSON")
                )
                .Build();
            Assert.Equal("Test name", this.configuration["TEST-JSON.Name"]);
            Assert.Equal("My Street", this.configuration["TEST-JSON.Address.StreetName"]);
        }

        [Fact]
        public void ShouldLogTheScalarSecretsAdded()
        {
            var stringWriter = new StringWriter();
	        Console.SetOut(stringWriter);

            this.configuration = new ConfigurationBuilder()
                .AddDockerSecret(
                    new Options()
                        .FromLocation("test-secrets")
                        .Load("TEST-INT")
                        .Log(true)
                )
                .Build();

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            Assert.Equal("23", this.configuration["TEST-INT"]);
            Assert.Equal("TEST-INT : 23\n", stringWriter.ToString());
        }

        [Fact]
        public void ShouldLogTheJsonSecretsAdded()
        {
            var stringWriter = new StringWriter();
	        Console.SetOut(stringWriter);

            this.configuration = new ConfigurationBuilder()
                .AddDockerSecret(
                    new Options()
                        .FromLocation("test-secrets")
                        .LoadJson("TEST-JSON")
                        .Log(true)
                )
                .Build();
            
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            Assert.Equal("TEST-JSON.Name : Test name\nTEST-JSON.Address.StreetName : My Street\n", stringWriter.ToString());

        }

        [Fact]
        public void ShouldNotLogTheValueForPrivateKeys()
        {
            var stringWriter = new StringWriter();
	        Console.SetOut(stringWriter);

            this.configuration = new ConfigurationBuilder()
                .AddDockerSecret(
                    new Options()
                        .FromLocation("test-secrets")
                        .Load("PRIVATE-KEY")
                        .Log(true)
                )
                .Build();
            
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            Assert.Equal("PRIVATE-KEY : *redacted*\n", stringWriter.ToString());
        }

        [Fact]
        public void ShouldNotLogTheValueForPrivateKeysFromJson()
        {
            var stringWriter = new StringWriter();
	        Console.SetOut(stringWriter);

            this.configuration = new ConfigurationBuilder()
                .AddDockerSecret(
                    new Options()
                        .FromLocation("test-secrets")
                        .LoadJson("PRIVATE-JSON")
                        .Log(true)
                )
                .Build();
            
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            Assert.Equal("PRIVATE-JSON.UserName : TestUser\nPRIVATE-JSON.Password : *redacted*\n", stringWriter.ToString());
        }

    }
}