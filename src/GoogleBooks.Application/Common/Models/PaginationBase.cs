namespace GoogleBooks.Application.Common.Models
{
    public abstract class PaginationBase
    {
        public int? Page { get; protected set; }
        public int? PageSize { get; protected set; }
    }
}