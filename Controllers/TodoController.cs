using Microsoft.AspNetCore.Mvc;
using To_Do_List.Data.Repositories;

namespace To_Do_List.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;

        public TodoController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        [HttpGet(Name = "GetAllTasks")]
        public IActionResult Get()
        {
            try
            {
                var tasks = _todoRepository.Get();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Произошла ошибка при получении задач.",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("{id}", Name = "GetTask")]
        public IActionResult Get(int id)
        {
            var task = _todoRepository.Get(id);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Models.Task task)
        {
            if (task == null || string.IsNullOrWhiteSpace(task.Title) || task.Title.Length < 4)
            {
                return BadRequest("Название задачи обязательно и должно содержать минимум 4 символа");
            }

            _todoRepository.Create(task);
            return CreatedAtRoute("GetTask", new { id = task.Id }, task);
        }

        [HttpPost("addList")]
        public IActionResult CreateMultiple([FromBody] List<Models.Task> tasks)
        {
            if (tasks == null || !tasks.Any())
            {
                return BadRequest("Список задач не может быть пустым");
            }

            foreach (var task in tasks)
            {
                if (string.IsNullOrWhiteSpace(task.Title) || task.Title.Length < 4)
                {
                    return BadRequest($"Задача с ID {task.Id} имеет недопустимое название");
                }
            }

            _todoRepository.DeleteAll();

            foreach (var task in tasks)
            {
                _todoRepository.Create(task);
            }

            return CreatedAtRoute("GetAllTasks", null, tasks);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Models.Task updatedTask)
        {
            if (updatedTask == null || id != updatedTask.Id)
            {
                return BadRequest("Неверные данные задачи");
            }

            var existingTask = _todoRepository.Get(id);
            if (existingTask == null)
            {
                return NotFound("Задача не найдена");
            }

            if (string.IsNullOrWhiteSpace(updatedTask.Title) || updatedTask.Title.Length < 4)
            {
                return BadRequest("Название задачи обязательно и должно содержать минимум 4 символа");
            }

            _todoRepository.Update(updatedTask);
            return Ok(updatedTask);
        }

        [HttpPut("complete/{id}")]
        public IActionResult MarkTaskAsCompleted(int id)
        {
            var task = _todoRepository.Get(id);
            if (task == null)
            {
                return NotFound("Задача не найдена");
            }

            task.Status = Data.Enums.TaskStatus.Completed;
            _todoRepository.Update(task);

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deletedTask = _todoRepository.Delete(id);
            return deletedTask == null ? NotFound("Задача не найдена") : Ok(deletedTask);
        }

        [HttpPut("updateStatuses")]
        public IActionResult UpdateTasksStatuses()
        {
            var tasks = _todoRepository.Get().ToList();
            var now = DateTime.UtcNow;

            foreach (var task in tasks)
            {
                if (task.Status == Data.Enums.TaskStatus.Completed) continue;

                if (task.Deadline.HasValue)
                {
                    task.Status = now > task.Deadline.Value
                        ? Data.Enums.TaskStatus.Overdue
                        : Data.Enums.TaskStatus.Active;
                }
            }

            _todoRepository.UpdateRange(tasks);
            return Ok();
        }
    }
}