using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DeviceController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult RegisterDevice([FromBody] DeviceRegistrationRequest request)
    {
        if (string.IsNullOrEmpty(request.Token))
            return BadRequest("Token is required");

        return Ok($"Device: {request.Device}, with token: {request.Token} succefuly registered"); // регистрация токена
    }
    [HttpPost("update-token")]
    public IActionResult update_token([FromBody] DeviceRegistrationRequest request)
    {
        if (string.IsNullOrEmpty(request.Token))
            return BadRequest("Token is required");

        return Ok($"Token: {request.Token}, device: {request.Device}, succefuly updated"); // обнавление токена
    }
}

public class DeviceRegistrationRequest
{
    public string Token { get; set; }
    public string Device { get; set; }
}