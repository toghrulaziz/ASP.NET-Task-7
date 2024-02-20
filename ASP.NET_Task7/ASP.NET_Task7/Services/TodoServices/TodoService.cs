using ASP.NET_Task7.Data;
using ASP.NET_Task7.Models.DTOs.Pagination;
using ASP.NET_Task7.Models.DTOs.Todo;
using ASP.NET_Task7.Models.Entities;
using ASP.NET_Task7.Providers;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Task7.Services.TodoServices
{
    public class TodoService : ITodoService
    {
        private readonly TodoDbContext _context;
        public TodoService(TodoDbContext context, IRequestUserProvider provider)
        {
            _context = context;
        }
        public async Task<TodoItemDto> ChangeTodoItemStatus(int id, bool isCompleted, UserInfo userInfo)
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userInfo.Id);

            if (todoItem == null)
            {
                throw new ArgumentException("Not Found");
            }

            todoItem.IsCompleted = isCompleted;
            await _context.SaveChangesAsync();

            var changedTodoItemDto = new TodoItemDto(
                    id: todoItem.Id,
                    text: todoItem.Text,
                    isCompleted: todoItem.IsCompleted,
                    createdTime: todoItem.CreatedTime,
                    deadline: todoItem.Deadline
                );

            return changedTodoItemDto;
        }

        public async Task<TodoItemDto> CreateTodo(CreateTodoItemRequest request, UserInfo userInfo)
        {
            var newTodoItem = new TodoItem
            {
                Text = request.Text,
                CreatedTime = DateTime.Now,
                UserId = userInfo.Id,
                Deadline = request.Deadline
            };

            _context.TodoItems.Add(newTodoItem);
            await _context.SaveChangesAsync();

            var todoItemDto = new TodoItemDto(
                    id: newTodoItem.Id,
                    text: newTodoItem.Text,
                    isCompleted: newTodoItem.IsCompleted,
                    createdTime: newTodoItem.CreatedTime,
                    deadline: newTodoItem.Deadline
                );

            return todoItemDto;
        }



        public async Task<bool> DeleteTodo(int id, UserInfo userInfo)
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userInfo.Id);

            if (todoItem == null)
            {
                return false;
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PaginatedListDto<TodoItemDto>> GetAll(int page, int pageSize, UserInfo userInfo)
        {
            IQueryable<TodoItem> query = _context.TodoItems.Where(e => e.UserId == userInfo.Id);
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalCount = await query.CountAsync();

            return new PaginatedListDto<TodoItemDto>(
                items.Select(e => new TodoItemDto(
                    id: e.Id,
                    text: e.Text,
                    isCompleted: e.IsCompleted,
                    createdTime: e.CreatedTime,
                    deadline: e.Deadline
                )),
            new PaginationMeta(page, pageSize, totalCount)
            );
        }

        public async Task<TodoItemDto?> GetTodoItem(int id, UserInfo userInfo)
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userInfo.Id);

            return todoItem is not null
                ? new TodoItemDto(
                    id: todoItem.Id,
                    text: todoItem.Text,
                    isCompleted: todoItem.IsCompleted,
                    createdTime: todoItem.CreatedTime,
                    deadline: todoItem.Deadline)
                : null;
        }


        public async Task<List<TodoItem>> GetItemsWithUpcomingDeadlinesAsync()
        {
            var upcomingItems = await _context.TodoItems
                .Where(item => item.Deadline.Date == DateTime.UtcNow.Date.AddDays(1)) 
                .ToListAsync();

            return upcomingItems;
        }


        public async Task<AppUser> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }


    }
}
