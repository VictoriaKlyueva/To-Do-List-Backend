namespace To_Do_List.Models
{
    public class TaskList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Task> Tasks { get; set; }
    }
}
