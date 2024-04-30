using System.Net;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Test.Functional;

public class FunctionalTests : IClassFixture<ContainerFixture>
{
  readonly HttpClient client;

  public FunctionalTests(ContainerFixture fixture)
  {
    client = fixture.Client;
  }

  [Fact]
  public async Task Test1()
  {
    var response = await client.GetAsync("/garage/automobiles");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }

  [Fact]
  public async Task Test2()
  {
    var response = await client.GetAsync("/thirdparty/locations/canalave-city");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }
}
