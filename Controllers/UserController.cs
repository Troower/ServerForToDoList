using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpGet("get/{id}")] // http://localhost:5131/api/user/get/number (number - это id)
    public IActionResult GetUserByID(int id)
    {
        if(id <= 0)
            return BadRequest("Id is invalid");

        return Ok($"User id: {id} succefuly returned"); // получение user-a
    }
    [HttpGet("getAll")] // http://localhost:5131/api/user/getAll 
    public IActionResult GetAllUser()
    {
        try 
        {
            // Логика получения пользователей
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest("An unknown error occurred");
        }
    }
    [HttpPost("register")] // http://localhost:5131/api/user/register
    public IActionResult RegisterUser([FromBody] List<UserRequest> requests) // регистрация пользователя (json отправлять в виде массива даже если один элемент)
    {
        if (requests == null || !requests.Any())
            return BadRequest("No users provided");

        
        var response = new List<string>();
        foreach (var request in requests)
        {
            response.Add($"User {request.lastName} {request.firstName} registered");
        }

        return Ok(response);
    }
    [HttpPut("update")] // http://localhost:5131/api/user/update
    public IActionResult update_user([FromBody] UserRequest request)
    {
        if (request == null)
            return BadRequest("Error accepting data, data is null");

        return Ok($"User: {request.lastName} {request.firstName} {request.surname} succefuly updated"); // обновление user-a
    }
    [HttpDelete("delete")] // http://localhost:5131/api/user/delete
    public IActionResult delete_user([FromBody] UserRequest request)
    {
        if (request == null)
            return BadRequest("Error accepting data, data is null");

        return Ok($"User: {request.lastName} {request.firstName} {request.surname} succefuly deleted"); // удаление user-a
    }
}

public class UserRequest // json отправлять в соответствии с пордком полей в классе
{
    [JsonPropertyName("user_id")]
    public int id { get; set; }
    [JsonPropertyName("last_name")]
    public string? lastName { get; set; }
    [JsonPropertyName("first_name")]
    public string firstName { get; set; }
    [JsonPropertyName("surname")]
    public string surname { get; set; }
    [JsonPropertyName("login")]
    public string login { get; set; }
    [JsonPropertyName("password_hash")]
    public string password { get; set; }
    [JsonPropertyName("role")]
    public string role { get; set; }
    [JsonPropertyName("created_by")]
    public int? createdBy { get; set; }
}