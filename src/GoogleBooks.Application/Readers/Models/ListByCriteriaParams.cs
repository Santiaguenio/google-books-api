using GoogleBooks.Application.Common.Models;

namespace GoogleBooks.Application.Readers.Models;

public class ListByCriteriaParams : PaginationBase
{
    public ListByCriteriaParams(int? page, int? pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }
}
