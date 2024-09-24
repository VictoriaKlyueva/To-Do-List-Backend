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

        public void Create(Models.Task item)
        {
            Context.Tasks.Add(item);
            Context.SaveChanges();
        }

        public void Update(Models.Task updatedTask)
        {
            Models.Task currentItem = Get(updatedTask.Id);
            currentItem.IsCompleted = updatedTask.IsCompleted;
            currentItem.Description = updatedTask.Description;

            Context.Tasks.Update(currentItem);
            Context.SaveChanges();
        }

        public Models.Task Delete(int Id)
        {
            Models.Task todoItem = Get(Id);

            if (todoItem != null)
            {
                Context.Tasks.Remove(todoItem);
                Context.SaveChanges();
            }

            return todoItem!;
        }

        public EFTodoRepository(EFTodoDBContext context)
        {
            Context = context;
        }
    }
}
