namespace ASP.NET_Task7.Models.DTOs.Pagination
{
    public class PaginationMeta
    {
        public PaginationMeta(int page, int pageSize, int count)
        {
            Page = page;
            PageSize = pageSize;
            TotalPage = (int)Math.Ceiling((double)count / pageSize);
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
    }
}
