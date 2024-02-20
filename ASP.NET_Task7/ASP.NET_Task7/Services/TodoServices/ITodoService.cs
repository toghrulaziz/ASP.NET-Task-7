using ASP.NET_Task7.Models.DTOs.Pagination;
using ASP.NET_Task7.Models.DTOs.Todo;
using ASP.NET_Task7.Models.Entities;
using ASP.NET_Task7.Providers;

namespace ASP.NET_Task7.Services.TodoServices
{
    public interface ITodoService
    {
        Task<TodoItemDto?> GetTodoItem(int id, UserInfo userInfo);
        Task<TodoItemDto> CreateTodo(CreateTodoItemRequest request, UserInfo userInfo);
        Task<TodoItemDto> ChangeTodoItemStatus(int id, bool isCompleted, UserInfo userInfo);
        Task<bool> DeleteTodo(int id, UserInfo userInfo);
        Task<PaginatedListDto<TodoItemDto>> GetAll(int page, int pageSize, UserInfo userInfo);
        Task<List<TodoItem>> GetItemsWithUpcomingDeadlinesAsync();
        Task<AppUser> GetUserByIdAsync(string userId);
    }
}
