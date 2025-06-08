using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpPost("register")] // http://localhost:5131/api/user/register
    public IActionResult RegisterUser([FromBody] UserRequest request)
    {
        if (request == null)
            return BadRequest("Error accepting data, data is null");

        return Ok($"User: {request.lastName} {request.firstName} {request.surnameName} succefuly registered"); // регистрация user-a
    }
    [HttpPut("update")] // http://localhost:5131/api/user/update
    public IActionResult update_user([FromBody] UserRequest request)
    {
        if (request == null)
            return BadRequest("Error accepting data, data is null");

        return Ok($"User: {request.lastName} {request.firstName} {request.surnameName} succefuly updated"); // обнавление user-a
    }
    [HttpDelete("delete")] // http://localhost:5131/api/user/delete
    public IActionResult delete_user([FromBody] UserRequest request)
    {
        if (request == null)
            return BadRequest("Error accepting data, data is null");

        return Ok($"User: {request.lastName} {request.firstName} {request.surnameName} succefuly deleted"); // удаление user-a
    }
}

public class UserRequest
{
    public int id { get; set; }
    public string lastName { get; set; }
    public string firstName { get; set; }
    public string surnameName { get; set; }
    public string login { get; set; }
    public string password { get; set; }
    public string role { get; set; }
    public int createdBy { get; set; }
}