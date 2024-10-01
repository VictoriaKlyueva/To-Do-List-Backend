using Microsoft.AspNetCore.Mvc;
using To_Do_List.Data.Repositories;
using To_Do_List.Models;

namespace To_Do_List.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        ITodoRepository TodoRepository;

        [HttpGet(Name = "GetAllTasks")]
        public IActionResult Get()
        {
            try
            {
                var tasks = TodoRepository.Get();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Success = false,
                    Message = "Произошла ошибка при получении задач.",
                    Error = ex.Message
                };
                return BadRequest(errorResponse);
            }
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

        [HttpPost("addList")]
        public IActionResult CreateMultiple([FromBody] List<Models.Task> tasks)
        {
            if (tasks == null || !tasks.Any())
            {
                return BadRequest();
            }

            TodoRepository.DeleteAll();

            foreach (var task in tasks)
            {
                TodoRepository.Create(task);
            }

            return CreatedAtRoute("GetAllTasks", null, tasks);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDescription(int Id, [FromBody] Description updatedDescription)
        {
            if (updatedDescription.DescriptionName == null)
            {
                return BadRequest("Аче описание пустое?");
            }

            var task = TodoRepository.Get(Id);
            if (task == null)
            {
                return NotFound("Эм такого дела нету :/");
            }

            Models.Task updatedTask = task;
            updatedTask.Description = updatedDescription.DescriptionName;

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
