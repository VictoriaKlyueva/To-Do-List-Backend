using System.ComponentModel.DataAnnotations;
using To_Do_List.Data.Enums;

namespace To_Do_List.Data.Dtos
{
    public class TaskCreateDto
    {
        [Required]
        [MinLength(4)]
        public required string Title { get; set; }

        public string? Description { get; set; }

        public DateTime? Deadline { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    }
}
