using ASP.NET_Task7.Data;
using ASP.NET_Task7.Models.DTOs.Pagination;
using ASP.NET_Task7.Models.DTOs.Todo;
using ASP.NET_Task7.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Task7.Services.TodoServices
{
    public class TodoService : ITodoService
    {
        private readonly TodoDbContext _context;

        public TodoService(TodoDbContext context)
        {
            _context = context;
        }
        public async Task<TodoItemDto> ChangeTodoItemStatus(int id, bool isCompleted)
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(e => e.Id == id);

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
                    createdTime: todoItem.CreatedTime
                );

            return changedTodoItemDto;
        }

        public async Task<TodoItemDto> CreateTodo(CreateTodoItemRequest request)
        {
            var newTodoItem = new TodoItem
            {
                Text = request.Text,
                CreatedTime = DateTime.Now,
            };

            _context.TodoItems.Add(newTodoItem);
            await _context.SaveChangesAsync();

            var todoItemDto = new TodoItemDto(
                    id: newTodoItem.Id,
                    text: newTodoItem.Text,
                    isCompleted: newTodoItem.IsCompleted,
                    createdTime: newTodoItem.CreatedTime
                );

            return todoItemDto;
        }

        public async Task<bool> DeleteTodo(int id)
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(e => e.Id == id);

            if (todoItem == null)
            {
                return false;
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PaginatedListDto<TodoItemDto>> GetAll(int page, int pageSize)
        {
            IQueryable<TodoItem> query = _context.TodoItems.AsQueryable();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalCount = await query.CountAsync();

            return new PaginatedListDto<TodoItemDto>(
                items.Select(e => new TodoItemDto(
                    id: e.Id,
                    text: e.Text,
                    isCompleted: e.IsCompleted,
                    createdTime: e.CreatedTime
                )),
            new PaginationMeta(page, pageSize, totalCount)
            );
        }

        public async Task<TodoItemDto?> GetTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(e => e.Id == id);

            return todoItem is not null
                ? new TodoItemDto(
                    id: todoItem.Id,
                    text: todoItem.Text,
                    isCompleted: todoItem.IsCompleted,
                    createdTime: todoItem.CreatedTime)
                : null;
        }
    }
}
