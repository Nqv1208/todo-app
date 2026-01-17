using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Common.Models;

namespace TodoApp.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    private ISender? _sender;
    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    // Helper để convert Result thành ActionResult
    protected ActionResult ToActionResult(Result result)
    {
        if (result.Succeeded)
            return Ok();
        
        return BadRequest(new { errors = result.Errors });
    }

    protected ActionResult<T> ToActionResult<T>(Result<T> result)
    {
        if (result.Succeeded)
            return Ok(result.Data);
        
        return BadRequest(new { errors = result.Errors });
    }

    // Lấy UserId từ JWT claims
    protected Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("userId");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return null;
        
        return userId;
    }
}
