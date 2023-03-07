using Xunit;

namespace DockerSecret;

public class SecretProviderTests
{

    private const string testLocation = "TEST-SECRETS";

    [Fact]
    public async void ShouldReturnASecret () {
        var service = new SecretProvider(testLocation);
        Assert.Equal("TEST VALUE", await service.Get("TEST-SECRET"));
    }

     [Fact]
    public void ShouldReturnDefaultLocation () {
        var service = new SecretProvider();
        Assert.Equal("/run/secrets", service.GetLocation());
    }

    [Fact]
    public void ShouldReturnProperLocation () {
        var service = new SecretProvider(testLocation);
        Assert.Equal(testLocation, service.GetLocation());
    }

    [Fact]
    public async void ShouldReturnNullForSecretNotFound () {
        var service = new SecretProvider(testLocation);
        Assert.Null(await service.Get("FAKE"));
    }

}