namespace Data;

public interface IRepository<T>
{
  public Task<T> GetById(Guid id, CancellationToken cancellationToken);

  public Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);

  public Task<T> Create(T entity, CancellationToken cancellationToken);

  public Task<T> Update(T entity, CancellationToken cancellationToken);

  public Task Delete(T entity, CancellationToken cancellationToken);
}
