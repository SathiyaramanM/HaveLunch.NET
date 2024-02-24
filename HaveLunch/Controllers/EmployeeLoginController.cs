using HaveLunch.Models;
using HaveLunch.Services;
using Microsoft.AspNetCore.Mvc;

namespace HaveLunch.Controllers;

[ApiController]
[Route("login")]
public class EmployeeLoginController(IEmployeeService employeeService): Controller
{
    [HttpPost]
    public async Task<IActionResult> LoginAsync(EmployeeLoginModel model)
    {
        try
        {
            return Ok(await employeeService.LoginAsync(model));
        }
        catch (Exception e) 
        {
            return BadRequest(e.Message);
        }
    }
}