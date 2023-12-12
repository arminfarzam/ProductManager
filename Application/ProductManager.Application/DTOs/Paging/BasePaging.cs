namespace ProductManager.Application.DTOs.Paging;

public class BasePaging
{
    public BasePaging()
    {
        PageNumber = 1;
        PageSize = 10;
    }
    public int PageNumber { get; set; }

    public int PageCount { get; set; }

    public int PageSize { get; set; }

    public int SkipSize { get; set; }
}