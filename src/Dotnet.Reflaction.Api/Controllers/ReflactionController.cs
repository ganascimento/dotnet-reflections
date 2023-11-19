using Dotnet.Reflaction.Api.Models;
using Dotnet.Reflaction.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Reflaction.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReflactionController : ControllerBase
{
    private readonly ICallerService _callerService;

    public ReflactionController(ICallerService callerService)
    {
        _callerService = callerService;
    }

    [HttpPost]
    public IActionResult Invoke([FromBody] CallerModel model)
    {
        var result = _callerService.Call(model);
        return Ok(result);
    }
}