using ASP.NET_Task7.Models.DTOs.Pagination;
using ASP.NET_Task7.Models.DTOs.Todo;
using ASP.NET_Task7.Services.TodoServices;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Task7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }


        [HttpGet("get/{id}")]
        public async Task<ActionResult<TodoItemDto>> Get(int id)
        {
            var item = await _todoService.GetTodoItem(id);

            return item is not null
                ? item
                : NotFound();
        }


        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var isDeleted = await _todoService.DeleteTodo(id);

            return isDeleted 
                ? Ok(true)
                : NotFound(false);
        }



        [HttpPost("create")]
        public async Task<ActionResult<TodoItemDto>> Create([FromBody] CreateTodoItemRequest request)
        {
            try
            {
                var createdTodoItemDto = await _todoService.CreateTodo(request);
                return CreatedAtAction(nameof(Get), new { id = createdTodoItemDto.Id }, createdTodoItemDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPatch("change-status/{id}")]
        public async Task<ActionResult<TodoItemDto>> ChangeStatus(int id, [FromBody] bool isCompleted)
        {
            try
            {
                var changedTodoItemDto = await _todoService.ChangeTodoItemStatus(id, isCompleted);
                return Ok(changedTodoItemDto);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpGet("all")]
        public async Task<PaginatedListDto<TodoItemDto>?> All(PaginationRequest request)
        {
            var result = await _todoService.GetAll(request.Page, request.PageSize);
            return result is not null ? result : null;
        }


    }
}
