using System.Data.SqlClient;
using Dapper;

namespace Data;

public class AutomobileRepository<T> : IRepository<T> where T : class
{
  private readonly string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

  public async Task<T> Create(T entity, CancellationToken cancellationToken)
  {
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
      string query = @"
      INSERT Garage.dbo.Automobile (
        Id,
        Make,
        Model,
        [Year],
        [Name],
        Last_Service_Date
      )
      VALUES
      (@Id, @Make, @Model, @Year, @Name, @LastServiceDate);
      
      SELECT 
        @Id AS Id,
        @Make AS Make, 
        @Model AS Model, 
        @Year AS Year, 
        @Name AS Name,
        @LastServiceDate AS Last_Service_Date;
      ";

      var parameters = new DynamicParameters(entity);
      var result = await conn.QuerySingleAsync<T>(query, parameters);

      return result;
    }
  }

  public Task Delete(T entity, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken)
  {
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
      /*
      if filtering on db side is preferred

      SELECT
       *
      FROM Garage.dbo.Automobile
      WHERE (Make = @Make OR @Make IS NULL)
        AND (Model = @Model OR @Model IS NULL)
        AND ([Year] = @Year OR @Year IS NULL)
        AND ([Name] = @Name OR @Name IS NULL)
        AND (Last_Service_Date = @LastServiceDate OR @LastServiceDate IS NULL)";
      */
      string query = "SELECT * FROM Garage.dbo.Automobile";
      var resultSet = await conn.QueryAsync<T>(query, cancellationToken);

      return resultSet;
    }
  }

  public Task<T> GetById(Guid id, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task<T> Update(T entity, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}
