using Api;
using FluentAssertions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Test;

public class PokeApiServiceTests
{
  WireMockServer _server;
  HttpClient httpClient;

  public PokeApiServiceTests(){
    _server = WireMockServer.Start();
    httpClient = new HttpClient{BaseAddress = new Uri(_server.Urls[0])};
  }

  [Fact]
  public async Task PokeApiService_should_return_Location_Object(){
    _server
    .Given(Request.Create().WithPath("/location/test-location").UsingGet())
    .RespondWith(
      Response.Create()
        .WithStatusCode(200)
        .WithBody(@"{ ""id"": 99, ""name"": ""Test Location"" }")
    );
     
    var service = new PokeApiService(httpClient);

    var result = await service.GetLocation("test-location", CancellationToken.None);

    result.Should().BeEquivalentTo(new Location{ Id = 99, Name ="Test Location"});
  }

  [Fact]
  public async Task PokeApiService_should_return_404_Not_Found(){
    _server
    .Given(Request.Create().WithPath("/location/test-location").UsingGet())
    .RespondWith(
      Response.Create()
        .WithStatusCode(404)
    );
     
    var service = new PokeApiService(httpClient);

    var result = await service.GetLocation("test-location", CancellationToken.None);

    result.Should().BeNull();
  }
}
