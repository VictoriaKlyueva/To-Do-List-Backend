namespace To_Do_List.Models
{
    public class Task
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public int IsCompleted { get; set; }
    }
}
