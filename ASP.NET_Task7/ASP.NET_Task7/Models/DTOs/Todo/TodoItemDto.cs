namespace ASP.NET_Task7.Models.DTOs.Todo
{
    public class TodoItemDto
    {
        public TodoItemDto(int id, string text, bool isCompleted, DateTime createdTime, DateTime deadline)
        {
            Id = id;
            Text = text;
            IsCompleted = isCompleted;
            CreatedTime = createdTime;
            Deadline = deadline;
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime Deadline { get; set; }
    }

}
