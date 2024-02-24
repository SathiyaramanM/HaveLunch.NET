using HaveLunch.Models;
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
        try
        {
            var dateTime = DateTime.Parse(date).ToUniversalTime();
            return Ok(await adminService.GetLunchAttendanceCount(dateTime));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetLunchAttendanceList(string date, int page = 1)
    {
        try
        {
            var dateTime = DateTime.Parse(date).ToUniversalTime();
            return Ok(await adminService.GetLunchAttendanceList(dateTime, page));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}