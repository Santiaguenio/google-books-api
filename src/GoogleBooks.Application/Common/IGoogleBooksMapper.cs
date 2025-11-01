namespace GoogleBooks.Application.Common
{
    public interface IGoogleBooksMapper
    {
        TEntity Map<TEntity>(object targetedEntity);
    }
}