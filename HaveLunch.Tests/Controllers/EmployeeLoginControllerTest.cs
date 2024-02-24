using HaveLunch.Controllers;
using HaveLunch.Entities;
using HaveLunch.Models;
using Microsoft.AspNetCore.Mvc;

namespace HaveLunch.Tests.Controllers;

public class EmployeeLoginControllerTest
{
    [Fact]
    public async Task LoginAsync_ShouldReturnOk_ForAllValidIdAndNameValues()
    {
        var service = new StubEmployeeService();
        var controller = new EmployeeLoginController(service);

        var model = new EmployeeLoginModel
        {
            Id = 1,
            Name = "Bruce"
        };

        var result = await controller.LoginAsync(model);

        var returnValue = Assert.IsAssignableFrom<OkObjectResult>(result).Value as EmployeeResponse;
        Assert.Equal(model.Id, returnValue.Id);
        Assert.Equal(model.Name, returnValue.Name);
    }
}