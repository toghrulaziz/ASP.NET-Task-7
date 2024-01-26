namespace ASP.NET_Task7.Models.DTOs.Todo
{
    public class TodoItemDto
    {
        public TodoItemDto(int id, string text, bool isCompleted, DateTime createdTime)
        {
            Id = id;
            Text = text;
            IsCompleted = isCompleted;
            CreatedTime = createdTime;
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedTime { get; set; }
    }

}
