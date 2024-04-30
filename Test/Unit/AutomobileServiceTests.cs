using Api;
using Data;
using FluentAssertions;
using Moq;

namespace Test.Unit;

public class AutomobileServiceTests
{
  [Fact]
  public async Task AutomobileService_Should_Return_Empty_Set(){
    var mock = new Mock<IRepository<Api.Automobile>>();
    mock.Setup(x => x.GetAll(CancellationToken.None))
    .Returns(Task.FromResult(new List<Api.Automobile>().AsEnumerable()));
    
    var service = new AutomobileService(mock.Object);

    var response = await service.GetAutomobilesByQuery(new AutomobileQuery(), CancellationToken.None);
    
    response.Should().BeEmpty();
  }  
}
