namespace GoogleBooks.Application.Common.Services;

public interface IService<TEntity>
{
    Task<TEntity> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken);
}