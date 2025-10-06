using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using ServerForToDoList.DBContext;
using ServerForToDoList.Extensions;
using ServerForToDoList.Model;
using ServerForToDoList.Repositories;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ToDoContext _context;
    private readonly FcmNotificationService _notificationService;

    public UserController(ToDoContext context, FcmNotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    [HttpGet("get/{id}")] // http://localhost:5131/api/user/get/1
    [Authorize]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = await _context.Users
                .Where(u => u.UserId == id )
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { Message = $"Пользователь с ID {id} не найден или у вас нет доступа" });
            }

            var userResponse = new UserRequest
            {
                id = user.UserId,
                lastName = user.LastName,
                firstName = user.FirstName,
                surname = user.Surname,
                login = user.Login,
                role = user.Role,
                createdBy = user.CreatedBy,
                deletedAt = user.DeletedAt
            };

            return Ok(userResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "internal server error" });
        }
    }

    [HttpGet("getAllByCreator")] 
    [Authorize(Roles = "admin,manager")]
    public async Task<IActionResult> GetAllUsersByCreator()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var users = await UserRepository.GetAllUserByIdCreatedAsync(_context, int.Parse(userIdClaim.ToString()));
            var usersResponse = users.Select(UserExtensions.ConvertToUserRequest).ToList();
            return Ok(usersResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "internal server error" });
        }
    }

    [HttpGet("UsersForManage")]
    [Authorize(Roles = "admin,manager")]
    public async Task<IActionResult> GetUsersForManage()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var users = await UserRepository.GetAllUserForManageAsync(_context, int.Parse(userIdClaim.ToString()));
            var usersResponse = users.Select(UserExtensions.ConvertToUserRequest).ToList();
            return Ok(usersResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "internal server error" });
        }
    }


    [HttpPost("register")] // http://localhost:5131/api/user/register
    [Authorize(Roles = "admin,manager")]
    public async Task<IActionResult> CreateUser([FromBody] UserRequest user)
    {
        try
        {
            if (await _context.Users.AnyAsync(u => u.Login == user.login))
            {
                return Conflict(new { Message = "User with this login already exists" });
            }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var newUser = new User
            {
                UserId = user.id,
                LastName = user.lastName,
                FirstName = user.firstName,
                Surname = user.surname,
                Login = user.login,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.password),
                Role = user.role,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = int.Parse(userIdClaim.ToString())
            };

            await UserRepository.AddUserAsync(_context, newUser);
            var answer = new
            {
                surname = newUser.Surname,
                first_name = newUser.FirstName,
                message = "Пользователь успешно создан"
            };


            return Created($"/api/user/get/{newUser.UserId}", answer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "internal server error" });
        }
    }


    [HttpPut("update")] // http://localhost:5131/api/user/update
    [Authorize(Roles = "admin,manager")]
    public async Task<IActionResult> update_user([FromBody] UserRequest request)
    {

        try
        {
            var user = await _context.Users
                .Where(u => u.UserId == request.id)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { Message = $"User with id {request.id} not found" });
                        
            user.FirstName = request.firstName;
            user.Surname = request.surname;
            user.Login = request.login;
            user.LastName = request.lastName;
            if(request.password != string.Empty)
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.password);
            }
            user.Role = request.role;
            
            await UserRepository.UpdateUserAsync(_context, user);
            var answer = new
            {
                surname = user.Surname,
                first_name = user.FirstName
            };
            return Ok(answer); // update user

        }
        catch (Exception)
        {
            return StatusCode(500, new { Message = "internal server error" });
        }

    }
    [HttpPut("softDelete")] // http://localhost:5131/api/user/softDelete
    [Authorize(Roles = "admin,manager")]
    public async Task<IActionResult> delete_user([FromBody] UserRequest request)
    {
        try
        {
            if (request.id > 0)
            {
                await UserRepository.SoftDeleteUserAsync(_context, request.id);
                var user = await _context.Users
                .Where(u => u.UserId == request.id)
                .FirstOrDefaultAsync();

                if (user != null)
                {
                    var deviceToken = await _context.UserDeviceTokens
                         .Where(u => u.UserId == user.UserId)
                         .Select(u => u)
                         .ToListAsync();
                    foreach (var token in deviceToken)
                    {
                        await _notificationService.SendNotificationAsync(token.DeviceToken, "Удаление аккаунта", $"Ваш аккаунт был удалён, если это ошибка обратитесь к администратору");
                        _context.UserDeviceTokens.Remove(token);
                    }

                    _context.SaveChanges();
                    
                    var mes = new
                    {
                        surname = user.Surname,
                        first_name = user.FirstName
                    };
                    return Ok(mes); // delete user
                }
            }

            return BadRequest(new { Message = "Error receiving data" });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Message = "internal server error" });
        }
    }
}

public class UserRequest // json 
{
    [JsonPropertyName("user_id")]
    public int id { get; set; }
    [JsonPropertyName("last_name")]
    public string? lastName { get; set; }
    [JsonPropertyName("first_name")]
    public string? firstName { get; set; }
    [JsonPropertyName("surname")]
    public string? surname { get; set; }
    [JsonPropertyName("login")]
    public string? login { get; set; }
    [JsonPropertyName("password_hash")]
    public string? password { get; set; }
    [JsonPropertyName("role")]
    public string? role { get; set; }
    [JsonPropertyName("created_by")]
    public int? createdBy { get; set; }
    [JsonPropertyName("deleted_at")]
    public DateTime? deletedAt { get; set; }
}