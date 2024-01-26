using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Task7.Models.DTOs.Pagination
{
    public class PaginationRequest
    {
        [Range(0, int.MaxValue)]
        [FromQuery(Name = "page")]
        [Required]
        public int Page { get; set; } = 1;
        [Range(0, int.MaxValue)]
        [FromQuery(Name = "pagesize")]
        [Required]
        public int PageSize { get; set; } = 10;
    }
}
