using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerForToDoList.DBContext;
using ServerForToDoList.Model;
using System.Text.Json.Serialization;

namespace ServerForToDoList.Controllers
{
    [ApiController]
    [Route("api/tasktype")]
    public class TaskTypeController : Controller
    {
        private readonly ToDoContext _context;

        public TaskTypeController(ToDoContext context)
        {
            _context = context;
        }

        // GET api/tasktype
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<TaskTypeDto>>> GetAll()
        {
            return await _context.TaskTypes
                .Select(t => new TaskTypeDto
                {
                    TypeId = t.TypeId,
                    TypeName = t.TypeName,
                    IsAccessible = t.IsAccessible
                })
                .AsNoTracking()
                .ToListAsync();
        }

        // POST api/tasktype
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string typeName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(typeName))
                    return BadRequest("Название типа обязательно");

                string normalized = typeName.Trim().ToLower();
                bool exists = await _context.TaskTypes
                    .AnyAsync(t => t.TypeName.Trim().ToLower() == normalized);

                if (exists)
                    return Conflict("Тип задачи уже существует");

                _context.TaskTypes.Add(new TaskType
                {
                    TypeName = typeName.Trim(),
                    IsAccessible = true
                });

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "internal server error");
            }
        }

        // PUT api/tasktype/{id}
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] string newTypeName)
        {
            try { 
            if (string.IsNullOrWhiteSpace(newTypeName))
                return BadRequest("Название типа обязательно");

            var type = await _context.TaskTypes.FindAsync(id);
            if (type == null)
                return NotFound();

            string normalized = newTypeName.Trim().ToLower();
            bool exists = await _context.TaskTypes
                .AnyAsync(t => t.TypeId != id && t.TypeName.Trim().ToLower() == normalized);

            if (exists)
                return Conflict("Название типа задачи уже занято");

            type.TypeName = newTypeName.Trim();
            await _context.SaveChangesAsync();
            return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "internal server error");
            }
        }

        // PATCH api/tasktype/{id}
        [Authorize(Roles = "admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAccessibility(int id, [FromBody] bool isAccessible)
        {
            try { 
            var type = await _context.TaskTypes.FindAsync(id);
            if (type == null)
                return NotFound();

            type.IsAccessible = isAccessible;
            await _context.SaveChangesAsync();
            return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "internal server error");
            }
        }
    }

    public class TaskTypeDto
    {
        [JsonPropertyName("type_id")]
        public int TypeId { get; set; }

        [JsonPropertyName("type_name")]
        public string TypeName { get; set; }

        [JsonPropertyName("is_accessible")]
        public bool IsAccessible { get; set; }
    }
}
