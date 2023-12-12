namespace ProductManager.Application.DTOs.Paging;

public class Pager
{
    public static BasePaging Build(int pageNumber, int pageSize, int pageCount)
    {
        if (pageNumber <= 1) pageNumber = 1;

        return new BasePaging
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            PageCount = pageCount,
            SkipSize = (pageNumber - 1) * pageSize,
        };
    }
}