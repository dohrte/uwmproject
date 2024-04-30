using System.Net;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Test.Functional;

public class FunctionalTests
{
  readonly HttpClient httpClient;

  public FunctionalTests(){
    var container = StartContainers().Result;

    httpClient = new HttpClient
    {
      BaseAddress = new UriBuilder(Uri.UriSchemeHttp, container.Hostname, container.GetMappedPublicPort(8080)).Uri
    };
  }

  [Fact]
  public async Task Test1()
  {
    var response = await httpClient.GetAsync("/garage/automobiles");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }

  [Fact]
  public async Task Test2()
  {
    var response = await httpClient.GetAsync("/thirdparty/locations/canalave-city");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }

  private async Task<IContainer> StartContainers(){
    var network = new NetworkBuilder()
      .WithName(Guid.NewGuid().ToString("D"))
      .Build();

    // start database
    var dbContainer = new ContainerBuilder()
      .WithName("sqlserver")
      .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
      .WithNetwork(network)
      .WithPortBinding(1433, true)
      .WithEnvironment("ACCEPT_EULA", "Y")
      .WithEnvironment("MSSQL_SA_PASSWORD", "Passw0rd!")      
      .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
      .Build();

    // setup db with test data using DbUp
    var dbUpContainer = new ContainerBuilder()
      .WithImage("uwmproject-dbup:latest")
      .WithNetwork(network)
      .WithEnvironment("CONNECTION_STRING", "Server=sqlserver,1433; Database=Garage; User Id=SA; Password=Passw0rd!; Trusted_connection=false;TrustServerCertificate=True")
      .DependsOn(dbContainer) 
      .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("Success!"))        
      .Build();

    var apiContainer = new ContainerBuilder()
      .WithImage("uwmproject-api:latest")
      .WithNetwork(network)
      .WithEnvironment("CONNECTION_STRING", "Server=sqlserver,1433; Database=Garage; User Id=SA; Password=Passw0rd!; Trusted_connection=false;TrustServerCertificate=True")
      .DependsOn(dbContainer)
      .WithPortBinding(8080, true)
      .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
      .Build();

    await network.CreateAsync().ConfigureAwait(false);

    // Start the container.     
    await Task.WhenAll(
        dbContainer.StartAsync(), 
        dbUpContainer.StartAsync(),
        apiContainer.StartAsync()
      ).ConfigureAwait(false);

    return apiContainer;
  }
}
