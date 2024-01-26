using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Task7.Models.DTOs.Todo
{
    public class CreateTodoItemRequest
    {
        [Required]
        [MinLength(5)]
        public string Text { get; set; } = string.Empty;
    }
}
