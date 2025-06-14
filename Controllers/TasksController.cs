using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerForToDoList.DBContext;
using System.Text.Json.Serialization;

namespace ServerForToDoList.Controllers
{
    [ApiController]
    [Route("api/task")]
    public class TasksController : ControllerBase
    {
        private readonly ToDoContext _context;

        public TasksController(ToDoContext context)
        {
            _context = context;
        }



        // GET api/task/get/'id'
        [HttpGet("get/{id}")]
        public IActionResult GetTaskById(int id)
        {
            try
            {
                var task = _context.Tasks.FirstOrDefault(t => t.TaskId == id);

                if (task == null)
                {
                    return NotFound($"Задача с ID {id} не найдена");
                }

                var taskDto = Extensions.TaskExtensions.ToDto(task);
                return Ok(taskDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла внутренняя ошибка сервера");
            }
        }

        // POST api/task
        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskDTO jsTask)
        {   
            try
            {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            Model.Task task =Extensions.TaskExtensions.ToEntity(jsTask);
            _context.Tasks.Add(task);
            _context.SaveChanges();
            TaskDTO responceTask= Extensions.TaskExtensions.ToDto(task);
            return CreatedAtAction(
                actionName: nameof(GetTaskById),
                routeValues: new { id = responceTask.TaskId }, 
                value: responceTask
            );
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Произошла внутренняя ошибка сервера");
            }
           
        }

        // PUT api/put/task/'id'
        [HttpPut("put/{id}")]
        public IActionResult UpdateTask(int id, [FromBody] Model.Task updatedTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 + ошибки валидации
            }

            var existingTask = _context.Tasks.FirstOrDefault(t => t.TaskId == id);
            if (existingTask == null)
            {
                return NotFound(); // 404
            }
            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.Status = updatedTask.Status;
            _context.SaveChanges();

            return NoContent(); // 204
        }
    }

    public class TaskDTO
    {
        [JsonPropertyName("task_id")]
        public int TaskId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("due_date")]
        public DateTime DueDate { get; set; }

        [JsonPropertyName("due_time")]
        public TimeSpan? DueTime { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("is_important")]
        public bool IsImportant { get; set; }

        [JsonPropertyName("type_id")]
        public int TypeId { get; set; }

        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("created_by")]
        public int CreatedBy { get; set; }

        [JsonPropertyName("completed_at")]
        public DateTime? CompletedAt { get; set; }

        [JsonPropertyName("is_confirmed")]
        public bool IsConfirmed {  get; set; }

        public List<TaskAssignmentsDTO>? Assignments { get; set; }


    }
    public class TaskAssignmentsDTO
    {
        [JsonPropertyName("assignment_id")]
        public int? AssignmentId { get; set; }

        [JsonPropertyName("task_id")]
        public int? TaskId { get; set; }

        [JsonPropertyName("user_id")]
        public int? UserId { get; set; }

        [JsonPropertyName("assigned_at")]
        public DateTime? AssignedAt { get; set; }

        [JsonPropertyName("assigned_by")]
        public int? AssignedBy { get; set; }

        [JsonPropertyName("to_delete")]
        public bool ToDelete { get; set; } = false;
    }
}
