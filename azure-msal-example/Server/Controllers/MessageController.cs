using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Security.Claims;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly ILogger<MessageController> _logger;

    // The required scope for the API (defined in Azure AD app registration)
    private static readonly string[] RequiredScopes = { "Message.Read" };

    public MessageController(ILogger<MessageController> logger)
    {
        _logger = logger;
    }

    [HttpPost("echo")]
    public IActionResult EchoMessage([FromBody] MessageRequest request)
    {
        // Verify that the required scope is present in the token
        HttpContext.VerifyUserHasAnyAcceptedScope(RequiredScopes);

        if (request == null || string.IsNullOrEmpty(request.Message))
        {
            return BadRequest("Message cannot be empty");
        }

        // Get user information from the token claims
        var userClaims = HttpContext.User;
        var userName = userClaims.FindFirst(ClaimTypes.Name)?.Value ?? 
                      userClaims.FindFirst("preferred_username")?.Value ?? 
                      "Unknown User";

        _logger.LogInformation("Received message from user {UserName}: {Message}", userName, request.Message);

        var response = new MessageResponse
        {
            OriginalMessage = request.Message,
            EchoMessage = $"Echo: {request.Message}",
            ReceivedAt = DateTime.UtcNow,
            UserName = userName
        };

        return Ok(response);
    }

    [HttpGet("protected")]
    public IActionResult GetProtectedData()
    {
        // Verify that the required scope is present in the token
        HttpContext.VerifyUserHasAnyAcceptedScope(RequiredScopes);

        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value ?? 
                      HttpContext.User.FindFirst("preferred_username")?.Value ?? 
                      "Unknown User";

        return Ok(new { 
            Message = "This is protected data", 
            User = userName,
            Timestamp = DateTime.UtcNow 
        });
    }
}

public class MessageRequest
{
    public string Message { get; set; } = string.Empty;
}

public class MessageResponse
{
    public string OriginalMessage { get; set; } = string.Empty;
    public string EchoMessage { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; }
    public string UserName { get; set; } = string.Empty;
}