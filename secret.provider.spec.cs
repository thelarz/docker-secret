using Xunit;

namespace DockerSecret;

public class SecretProviderTests
{

    private const string testLocation = "TEST-SECRETS";


    [Fact]
    public async void ShouldReturnASecretOfTypeString () {
        var service = new SecretProvider(testLocation);
        Assert.IsType<string>(await service.Get<string>("TEST-SECRET"));        
    }

    [Fact]
    public async void ShouldReturnASecret () {
        var service = new SecretProvider(testLocation);
        Assert.Equal("TEST VALUE", await service.Get("TEST-SECRET"));
    }

    [Fact]
    public async void ShouldReturnACorrectlyTypedIntValue () {
        var service = new SecretProvider(testLocation);
        Assert.IsType<int>(await service.Get<int>("TEST-INT"));
        Assert.Equal(23, await service.Get<int>("TEST-INT"));
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

    [Fact]
    public async void ShouldReturnACorrectlyTypedIBoolValue () {
        var service = new SecretProvider(testLocation);
         Assert.IsType<bool>(await service.Get<bool>("TEST-BOOL"));
        Assert.True(await service.Get<bool>("TEST-BOOL"));
    }

}