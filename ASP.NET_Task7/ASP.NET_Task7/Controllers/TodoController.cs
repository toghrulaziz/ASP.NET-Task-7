using ASP.NET_Task7.Models.DTOs.Pagination;
using ASP.NET_Task7.Models.DTOs.Todo;
using ASP.NET_Task7.Providers;
using ASP.NET_Task7.Services.TodoServices;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP.NET_Task7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly IRequestUserProvider _provider;
        public TodoController(ITodoService todoService, IRequestUserProvider provider)
        {
            _todoService = todoService;
            _provider = provider;
        }


        [HttpGet("get/{id}")]
        public async Task<ActionResult<TodoItemDto>> Get(int id)
        {
            UserInfo? userInfo = _provider.GetUserInfo();
            var item = await _todoService.GetTodoItem(id, userInfo!);

            return item is not null
                ? item
                : NotFound();
        }


        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            UserInfo? userInfo = _provider.GetUserInfo();
            var isDeleted = await _todoService.DeleteTodo(id, userInfo!);

            return isDeleted 
                ? Ok(true)
                : NotFound(false);
        }



        [HttpPost("create")]
        public async Task<ActionResult<TodoItemDto>> Create([FromBody] CreateTodoItemRequest request)
        {
            UserInfo? userInfo = _provider.GetUserInfo();
            try
            {
                var createdTodoItemDto = await _todoService.CreateTodo(request, userInfo!);
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
                UserInfo? userInfo = _provider.GetUserInfo();
                var changedTodoItemDto = await _todoService.ChangeTodoItemStatus(id, isCompleted, userInfo!);
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
            UserInfo? userInfo = _provider.GetUserInfo();
            var result = await _todoService.GetAll(request.Page, request.PageSize, userInfo!);
            return result is not null ? result : null;
        }


    }
}
