using To_Do_List.Models;

namespace To_Do_List.Data.Repositories
{
    public interface ITodoRepository
    {
        IEnumerable<Models.Task> Get();
        Models.Task Get(int id);
        void Create(Models.Task item);
        void Update(Models.Task item);
        Models.Task Delete(int id);
    }
}
