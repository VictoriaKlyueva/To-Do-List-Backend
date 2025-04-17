namespace To_Do_List.Data.Repositories
{
    namespace To_Do_List.Data.Repositories
    {
        public class EFTodoRepository : ITodoRepository
        {
            private readonly EFTodoDBContext _context;

            public EFTodoRepository(EFTodoDBContext context)
            {
                _context = context;
            }

            public IEnumerable<Models.Task> Get()
            {
                return _context.Tasks;
            }

            public Models.Task Get(int id)
            {
                return _context.Tasks.Find(id)!;
            }

            public void Create(Models.Task task)
            {
                task.CreatedDate = DateTime.UtcNow;
                _context.Tasks.Add(task);
                _context.SaveChanges();
            }

            public void Update(Models.Task updatedTask)
            {
                var currentTask = Get(updatedTask.Id);

                currentTask.Title = updatedTask.Title;
                currentTask.Description = updatedTask.Description;
                currentTask.Deadline = updatedTask.Deadline;
                currentTask.Status = updatedTask.Status;
                currentTask.Priority = updatedTask.Priority;
                currentTask.ModifiedDate = DateTime.UtcNow;

                _context.Tasks.Update(currentTask);
                _context.SaveChanges();
            }

            public void UpdateRange(IEnumerable<Models.Task> items)
            {
                foreach (var item in items)
                {
                    var existingItem = _context.Tasks.Find(item.Id);
                    if (existingItem != null)
                    {
                        _context.Entry(existingItem).CurrentValues.SetValues(item);
                        existingItem.ModifiedDate = DateTime.UtcNow;
                    }
                }
                _context.SaveChanges();
            }

            public Models.Task Delete(int id)
            {
                var task = Get(id);

                if (task != null)
                {
                    _context.Tasks.Remove(task);
                    _context.SaveChanges();
                }

                return task!;
            }

            public void DeleteAll()
            {
                _context.Tasks.RemoveRange(_context.Tasks);
                _context.SaveChanges();
            }
        }
    }
}
