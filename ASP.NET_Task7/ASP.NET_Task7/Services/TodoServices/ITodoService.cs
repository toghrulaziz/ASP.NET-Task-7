using ASP.NET_Task7.Models.DTOs.Pagination;
using ASP.NET_Task7.Models.DTOs.Todo;

namespace ASP.NET_Task7.Services.TodoServices
{
    public interface ITodoService
    {
        Task<TodoItemDto?> GetTodoItem(int id);
        Task<TodoItemDto> CreateTodo(CreateTodoItemRequest request);
        Task<TodoItemDto> ChangeTodoItemStatus(int id, bool isCompleted);
        Task<bool> DeleteTodo(int id);
        Task<PaginatedListDto<TodoItemDto>> GetAll(int page, int pageSize);
    }
}
