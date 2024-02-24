using HaveLunch.Models;
using HaveLunch.Services;
using Microsoft.AspNetCore.Mvc;

namespace HaveLunch.Controllers;

[ApiController]
[Route("attendance")]
public class EmployeeAttendanceController(IEmployeeAttendanceService employeeAttendanceService) : Controller
{
    [HttpGet("get")]
    public async Task<IActionResult> GetAttendanceDetail(int employeeId, string date)
    {
        try
        {
            var dateTime = DateTime.Parse(date).ToUniversalTime();
            return Ok(await employeeAttendanceService.GetEmployeeAttendanceDetail(employeeId, dateTime));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdateEmployeeAttendanceDetail(EmployeeAttendanceRequest request)
    {
        try
        {
            return Ok(await employeeAttendanceService.CreateOrUpdateEmployeeAttendance(request));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{employeeId:int}")]
    public async Task<IActionResult> GetEmployeeAttendanceHistory(int employeeId, int page = 1)
    {
        try
        {
            return Ok(await employeeAttendanceService.GetEmployeeAttendanceHistory(employeeId, page));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}