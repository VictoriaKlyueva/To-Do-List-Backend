using Microsoft.AspNetCore.Mvc;
using To_Do_List.Data.Repositories;

namespace To_Do_List.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        ITodoRepository TodoRepository;

        [HttpGet(Name = "GetAllTasks")]
        public IEnumerable<Models.Task> Get()
        {
            return TodoRepository.Get();
        }

        [HttpGet("{id}", Name = "GetTask")]
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
            return CreatedAtRoute("GetTask", new { id = task.Id }, task);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateDescription(int Id, [FromBody] string updatedDescription)
        {
            if (updatedDescription == null)
            {
                return BadRequest("Хули описание пустое");
            }

            var task = TodoRepository.Get(Id);
            if (task == null)
            {
                return NotFound("Эм такого дела нету :/");
            }

            Models.Task updatedTask = task;
            updatedTask.Description = updatedDescription;

            TodoRepository.Update(updatedTask);
            return RedirectToRoute("GetAllTasks");
        }

        [HttpPut("complete/{id}")]
        public IActionResult MarkTaskAsCompleted(int id)
        {
            var task = TodoRepository.Get(id);
            if (task == null)
            {
                return NotFound("Эм такого дела нету :/");
            }

            task.IsCompleted = true;
            TodoRepository.Update(task);

            return CreatedAtRoute("GetTask", new { id = task.Id }, task);
        }

        [HttpPut("incomplete/{id}")]
        public IActionResult MarkTaskAsIncomplete(int id)
        {
            var task = TodoRepository.Get(id);
            if (task == null)
            {
                return NotFound("Эм такого дела нету :/");
            }

            task.IsCompleted = false;
            TodoRepository.Update(task);

            return CreatedAtRoute("GetTask", new { id = task.Id }, task);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int Id)
        {
            var deletedTodoItem = TodoRepository.Delete(Id);

            if (deletedTodoItem == null)
            {
                return BadRequest("Эм такого дела нету :/");
            }

            return new ObjectResult(deletedTodoItem);
        }

        public TodoController(ITodoRepository todoRepository)
        {
            TodoRepository = todoRepository;
        }
    }
}
