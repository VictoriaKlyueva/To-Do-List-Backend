using To_Do_List.Data.Enums;
using TaskStatus = To_Do_List.Data.Enums.TaskStatus;

namespace To_Do_List.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime? Deadline { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Active;

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }
    }
}
