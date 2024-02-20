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
        private readonly ILogger<TodoController> _logger;
        public TodoController(ITodoService todoService, IRequestUserProvider provider, ILogger<TodoController> logger)
        {
            _todoService = todoService;
            _provider = provider;
            _logger = logger;
        }


        [HttpGet("get/{id}")]
        public async Task<ActionResult<TodoItemDto>> Get(int id)
        {
            UserInfo? userInfo = _provider.GetUserInfo();
            try
            {
                var item = await _todoService.GetTodoItem(id, userInfo!);
                return item != null ? item : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting Todo item {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            UserInfo? userInfo = _provider.GetUserInfo();
            try
            {
                var isDeleted = await _todoService.DeleteTodo(id, userInfo!);
                return isDeleted ? Ok(true) : NotFound(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting Todo item {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
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
                _logger.LogError(ex, "An error occurred while creating a new Todo item");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request to create a new Todo item");
                return StatusCode(500, "An error occurred while processing your request.");
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
                _logger.LogError(ex, "An error occurred while changing status of Todo item {Id}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request to change status of Todo item {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        [HttpGet("all")]
        public async Task<PaginatedListDto<TodoItemDto>?> All(PaginationRequest request)
        {
            UserInfo? userInfo = _provider.GetUserInfo();
            try
            {
                var result = await _todoService.GetAll(request.Page, request.PageSize, userInfo!);
                return result is not null ? result : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all Todo items");
                return null;
            }
        }


    }
}
