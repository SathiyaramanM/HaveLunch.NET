using HaveLunch.Services;
using Microsoft.AspNetCore.Mvc;

namespace HaveLunch.Controllers;

[ApiController]
[Route("admin")]
public class AdminController(IAdminService adminService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetLunchAttendanceCount(string date = "")
    {
        var dateTimeOffset = DateTimeOffset.Parse(date);
        return Ok(await adminService.GetLunchAttendanceCount(dateTimeOffset));
    }
}