using Microsoft.EntityFrameworkCore;

namespace To_Do_List.Data
{
    public class EFTodoDBContext : DbContext
    {
        public EFTodoDBContext(DbContextOptions<EFTodoDBContext> options) : base(options)
        { }
        public DbSet<Models.Task> Tasks { get; set; }
    }
}
