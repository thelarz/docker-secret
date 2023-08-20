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

    }
}