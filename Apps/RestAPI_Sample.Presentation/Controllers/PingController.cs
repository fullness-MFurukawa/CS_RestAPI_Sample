using Microsoft.AspNetCore.Mvc;
using RestAPI_Sample.Infrastructure.Contexts;

namespace RestAPI_Sample.Presentation.Controllers;

[ApiController]
[Route("api/ping")]
public class PingController : ControllerBase
{
    private readonly AppDbContext _context;

    public PingController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Ping()
    {
        var connected = _context.Database.CanConnect();
        return Ok(new { DbConnected = connected });
    }
}