using Microsoft.AspNetCore.Mvc;
using ServerForToDoList.DBContext;
using ServerForToDoList.Repositories;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    // ороче, в program.cs задаетс€ контекст Ѕƒ, и он, как € пон€л автоматически встраиваетс€ в контроллы. Ќо что бы им пользоватьс€ его надо €вно объ€вить
    private readonly ToDoContext _context;

    public UserController(ToDoContext context)
    {
        _context = context;
    }


    [HttpGet("get/{id}")] // http://localhost:5131/api/user/get/number (number - это id)
    public async Task<IActionResult> GetUserAsync(int id)//!!!!!!!!!!!!!!Ќужно  все методы в контролах сделать асинхронными !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        if(id <= 0)
            return BadRequest("Id is invalid");
        //ѕолучение всех пользователей в консоль к примеру.
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
        return Ok($"User id: {id} succefuly returned"); // получение user-a
    }
    [HttpPost("register")] // http://localhost:5131/api/user/register
    public IActionResult RegisterUser([FromBody] List<UserRequest> requests) // регистраци€ пользовател€ (json отправл€ть в виде массива даже если один элемент)
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

        return Ok($"User: {request.lastName} {request.firstName} {request.surname} succefuly updated"); // обнавление user-a
    }
    [HttpDelete("delete")] // http://localhost:5131/api/user/delete
    public IActionResult delete_user([FromBody] UserRequest request)
    {
        if (request == null)
            return BadRequest("Error accepting data, data is null");

        return Ok($"User: {request.lastName} {request.firstName} {request.surname} succefuly deleted"); // удаление user-a
    }
}

public class UserRequest // json отправл€ть в соответствии с пор€дком полей в классе и своответствии с названи€ми полей в классе
{
    public int id { get; set; }
    public string? lastName { get; set; }
    public string firstName { get; set; }
    public string surname { get; set; }
    public string login { get; set; }
    public string password { get; set; }
    public string role { get; set; }
    public int? createdBy { get; set; }
}