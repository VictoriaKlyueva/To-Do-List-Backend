using Microsoft.AspNetCore.Mvc;
using To_Do_List.Data.Repositories;

namespace To_Do_List.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        ITodoRepository TodoRepository;

        [HttpGet(Name = "GetAllItems")]
        public IEnumerable<Models.Task> Get()
        {
            return TodoRepository.Get();
        }

        [HttpGet("{id}", Name = "GetTodoItem")]
        public IActionResult Get(int Id)
        {
            Models.Task task = TodoRepository.Get(Id);

            if (task == null)
            {
                return NotFound();
            }

            return new ObjectResult(task);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Models.Task task)
        {
            if (task == null)
            {
                return BadRequest();
            }
            TodoRepository.Create(task);
            return CreatedAtRoute("GetTodoItem", new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int Id, [FromBody] Models.Task updatedTodoItem)
        {
            if (updatedTodoItem == null || updatedTodoItem.Id != Id)
            {
                return BadRequest();
            }

            var todoItem = TodoRepository.Get(Id);
            if (todoItem == null)
            {
                return NotFound();
            }

            TodoRepository.Update(updatedTodoItem);
            return RedirectToRoute("GetAllItems");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int Id)
        {
            var deletedTodoItem = TodoRepository.Delete(Id);

            if (deletedTodoItem == null)
            {
                return BadRequest();
            }

            return new ObjectResult(deletedTodoItem);
        }

        public TodoController(ITodoRepository todoRepository)
        {
            TodoRepository = todoRepository;
        }
    }
}
