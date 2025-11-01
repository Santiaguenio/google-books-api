namespace GoogleBooks.Application.Common
{
    public interface IGoogleBooksMapper
    {
        TTargetedEntity Map<TTargetedEntity>(object sourceEntity);
    }
}