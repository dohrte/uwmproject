using Data;

namespace Api;

public class AutomobileService
{
  private readonly IRepository<Automobile> _automobileRepository;

  public AutomobileService(IRepository<Automobile> automobileRepository){
    _automobileRepository = automobileRepository;
  }

  public async Task<IEnumerable<Automobile>> GetAutomobilesByQuery(AutomobileQuery automobileQuery, CancellationToken cancellationToken){
    static bool QueryFilter(Automobile automobile, AutomobileQuery automobileQuery){        
      return (automobileQuery.Make == null || automobileQuery.Make.Equals(automobile.Make, StringComparison.InvariantCultureIgnoreCase))      
        && (automobileQuery.Model == null || automobileQuery.Model.Equals(automobile.Model, StringComparison.InvariantCultureIgnoreCase))      
        && (automobileQuery.Year == null || automobileQuery.Year == automobile.Year)
        && (automobileQuery.Name == null || automobileQuery.Name.Equals(automobile.Name, StringComparison.InvariantCultureIgnoreCase))      
        && (automobileQuery.LastServiceDate == null || automobileQuery.LastServiceDate.Equals(automobile.LastServiceDate));
    };
    
    var queryResult = (await _automobileRepository.GetAll(cancellationToken)).Where(r => QueryFilter(r, automobileQuery));

    return queryResult ?? [];
  }

  public async Task<Automobile> AddAutomobile(NewAutomobile newAutomobile, CancellationToken cancellationToken){
    var automobileToCreate = new Automobile{
      Id = Guid.NewGuid(),
      Name = newAutomobile.Name,
      Make = newAutomobile.Make,
      Model = newAutomobile.Model,
      Year = newAutomobile.Year,
      LastServiceDate = newAutomobile.LastServiceDate
    };

    // verify not existing already
    var query = new AutomobileQuery{ 
      Make = newAutomobile.Make, 
      Model = newAutomobile.Model,
      Name = newAutomobile.Name,
      Year = newAutomobile.Year
    };

    if((await GetAutomobilesByQuery(query, cancellationToken)).Any()){
      return null;
    }

    // add
    var result = await _automobileRepository.Create(automobileToCreate, cancellationToken);

    return result;
  }
}
