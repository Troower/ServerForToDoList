using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using ServerForToDoList.Controllers;
using ServerForToDoList.DBContext;
using ServerForToDoList.Model;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

[ApiController]
[Route("api/device")]
public class DeviceController : ControllerBase
{
    private readonly ToDoContext _context;
    private readonly DeviceTokenService _deviceService;
    public DeviceController(ToDoContext context, DeviceTokenService deviceService)
    {
        _context = context;
        _deviceService = deviceService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterDevice([FromBody] DeviceRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Token))
                return BadRequest("Token is required");
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _deviceService.RegisterOrUpdateTokenAsync(request, int.Parse(userIdClaim));
            return Ok($"{result}: Device {request.Device}, token {request.Token}");
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(409, "Your FCM Token already registered");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Произошла внутреняя ошибка сервера");
        }
    }
    
    [HttpDelete("delete-by-user")] // http://localhost:5131/api/device/delete-by-user
    public async Task<IActionResult> delete_token([FromBody] int id)
    {
        try
        {
            var deletingTokens = await _context.UserDeviceTokens.Where(w => w.UserId == id).Select(t => t).ToListAsync();

            foreach (var delete in deletingTokens)
            {
                _context.UserDeviceTokens.Remove(delete);
            }
            _context.SaveChanges();
            return Ok($"All user tokens succefuly deleted"); // удаление токена
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Произошла внутренняя ошибка сервера");
        }
    }
    [HttpPut("delete-this-token")] // http://localhost:5131/api/device/delete-this-token
    public async Task<IActionResult> delete_this_token([FromBody] DeviceRequest request)
    {
        try
        {
            if(string.IsNullOrEmpty(request.Token))
                return BadRequest("Token is required");

            var deletingToken = _context.UserDeviceTokens.Where(w => w.DeviceToken == request.Token).Select(t => t);
            if (deletingToken.Count() != 0)
            {
                foreach (var delete in deletingToken)
                {
                    _context.UserDeviceTokens.Remove(delete);
                }
                _context.SaveChanges();
                return Ok($"Token succefuly deleted"); // удаление токена
            }
            else
                return Ok("already up-to-date");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "internal server error");
        }
    }

}

public class DeviceRequest
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [JsonPropertyName("device")]
    public string Device { get; set; }
}