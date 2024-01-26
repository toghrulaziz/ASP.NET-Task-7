namespace ASP.NET_Task7.Models.DTOs.Pagination
{
    public class PaginatedListDto<TModel>
    {
        public PaginatedListDto(IEnumerable<TModel> items, PaginationMeta meta)
        {
            Items = items;
            Meta = meta;
        }

        public IEnumerable<TModel> Items { get; set; }
        public PaginationMeta Meta { get; set; }
    }
}
