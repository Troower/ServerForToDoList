using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DeviceController : ControllerBase
{
    [HttpPost("register")] // http://localhost:5131/api/device/register
    public IActionResult RegisterDevice([FromBody] DeviceRequest request)
    {
        if (string.IsNullOrEmpty(request.Token))
            return BadRequest("Token is required");

        return Ok($"Device: {request.Device}, with token: {request.Token} succefuly registered"); // регистрация токена
    }
    [HttpPut("update-token")] // http://localhost:5131/api/device/update-token
    public IActionResult update_token([FromBody] DeviceRequest request)
    {
        if (string.IsNullOrEmpty(request.Token))
            return BadRequest("Token is required");

        return Ok($"Token: {request.Token}, device: {request.Device}, succefuly updated"); // обнавление токена
    }
    [HttpDelete("delete")] // http://localhost:5131/api/device/delete
    public IActionResult delete_token([FromBody] DeviceRequest request)
    {
        if (string.IsNullOrEmpty(request.Token))
            return BadRequest("Token is required");

        return Ok($"Token: {request.Token}, device: {request.Device}, succefuly deleted"); // удаление токена
    }
}

public class DeviceRequest
{
    public string Token { get; set; }
    public string Device { get; set; }
}