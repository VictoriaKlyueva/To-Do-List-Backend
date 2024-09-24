namespace To_Do_List.Data.Repositories
{
    public class EFTodoRepository : ITodoRepository
    {
        private EFTodoDBContext Context;

        public IEnumerable<Models.Task> Get()
        {
            return Context.Tasks;
        }

        public Models.Task Get(int Id)
        {
            return Context.Tasks.Find(Id)!;
        }

        public void Create(Models.Task task)
        {
            Context.Tasks.Add(task);
            Context.SaveChanges();
        }

        public void Update(Models.Task updatedTask)
        {
            Models.Task currentTask = Get(updatedTask.Id);
            currentTask.IsCompleted = updatedTask.IsCompleted;
            currentTask.Description = updatedTask.Description;

            Context.Tasks.Update(currentTask);
            Context.SaveChanges();
        }

        public Models.Task Delete(int Id)
        {
            Models.Task task = Get(Id);

            if (task != null)
            {
                Context.Tasks.Remove(task);
                Context.SaveChanges();
            }

            return task!;
        }

        public void DeleteAll()
        {
            foreach (var item in Context.Tasks)
            {
                Context.Tasks.Remove(item);
            }
            Context.SaveChanges();
        }

        public EFTodoRepository(EFTodoDBContext context)
        {
            Context = context;
        }
    }
}
