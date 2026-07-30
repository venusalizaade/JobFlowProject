namespace JobFlowProject.Domain.Entites.User;

public class Paging
{
    public Paging(int pageNumber, int count)
    {
        PageNumber = pageNumber;
        PageSize = count;

    }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public int Skip => (PageNumber - 1) * PageSize;
}