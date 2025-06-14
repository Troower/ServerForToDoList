using Microsoft.AspNetCore.Mvc;
using ServerForToDoList.DBContext;
using ServerForToDoList.Repositories;
using System.Text;
using System.Text.Json.Serialization;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    //Êîðî÷å, â program.cs çàäàåòñÿ êîíòåêñò ÁÄ, è îí, êàê ÿ ïîíÿë àâòîìàòè÷åñêè âñòðàèâàåòñÿ â êîíòðîëëû. Íî ÷òî áû èì ïîëüçîâàòüñÿ åãî íàäî ÿâíî îáúÿâèòü
    private readonly ToDoContext _context;

    public UserController(ToDoContext context)
    {
        _context = context;
    }


    [HttpGet("get/{id}")] // http://localhost:5131/api/user/get/number (number - ýòî id)
    public async Task<IActionResult> GetUserAsync(int id)//!!!!!!!!!!!!!!Íóæíî  âñå ìåòîäû â êîíòðîëàõ ñäåëàòü àñèíõðîííûìè !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        if(id <= 0)
            return BadRequest("Id is invalid");
        //Ïîëó÷åíèå âñåõ ïîëüçîâàòåëåé â êîíñîëü ê ïðèìåðó.
        //var userList = await UserRepository.GetUsersAsync(_context);
        //var users = new StringBuilder();
        //if (userList != null)
        //{
        //    foreach (var user in userList)
        //    {
        //        users.AppendLine($"{user.FirstName} {user.LastName} {user.Surname}");
        //    }
        //}
        //Console.WriteLine(users.ToString());
        return Ok($"User id: {id} succefuly returned"); // ïîëó÷åíèå user-a
    }

    [HttpGet("getAll")] // http://localhost:5131/api/user/getAll 
    public IActionResult GetAllUser()
    {
        try 
        {
            // Ëîãèêà ïîëó÷åíèÿ ïîëüçîâàòåëåé
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest("An unknown error occurred");
        }
    }
    [HttpPost("register")] // http://localhost:5131/api/user/register
    public IActionResult RegisterUser([FromBody] List<UserRequest> requests) // ðåãèñòðàöèÿ ïîëüçîâàòåëÿ (json îòïðàâëÿòü â âèäå ìàññèâà äàæå åñëè îäèí ýëåìåíò)
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

        return Ok($"User: {request.lastName} {request.firstName} {request.surname} succefuly updated"); // îáíîâëåíèå user-a
    }
    [HttpDelete("delete")] // http://localhost:5131/api/user/delete
    public IActionResult delete_user([FromBody] UserRequest request)
    {
        if (request == null)
            return BadRequest("Error accepting data, data is null");

        return Ok($"User: {request.lastName} {request.firstName} {request.surname} succefuly deleted"); // óäàëåíèå user-a
    }
}

public class UserRequest // json îòïðàâëÿòü â ñîîòâåòñòâèè ñ ïîðäêîì ïîëåé â êëàññå
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