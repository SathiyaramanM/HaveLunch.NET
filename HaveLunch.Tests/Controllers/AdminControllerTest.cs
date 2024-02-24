using HaveLunch.Controllers;
using HaveLunch.Models;
using HaveLunch.Tests.Stubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaveLunch.Tests.Controllers;

public class AdminControllerTest
{
    [Fact]
    public async Task GetLunchAttendanceCount_ShouldReturnOk_ForGivenDateInCorrectFormat()
    {
        var service = new StubAdminService();
        var controller = new AdminController(service);

        var result1 = await controller.GetLunchAttendanceCount("25/05/2023");
        var result2 = await controller.GetLunchAttendanceCount("26/05/2023");
        var result3 = await controller.GetLunchAttendanceCount("27/05/2023");

        var returnValue1 = Assert.IsAssignableFrom<OkObjectResult>(result1).Value as AdminCountResponse;
        var returnValue2 = Assert.IsAssignableFrom<OkObjectResult>(result2).Value as AdminCountResponse;
        var returnValue3 = Assert.IsAssignableFrom<OkObjectResult>(result3).Value as AdminCountResponse;

        Assert.Equal(2, returnValue1.Count);
        Assert.Equal(1, returnValue2.Count);
        Assert.Equal(0, returnValue3.Count);
    }

    [Fact]
    public async Task GetLunchAttendanceCount_ShouldReturnBadRequest_ForGivenDateInInvalidFormat()
    {
        var service = new StubAdminService();
        var controller = new AdminController(service);

        var result = await controller.GetLunchAttendanceCount("2505/2023");

        var returnValue = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        Assert.True(returnValue.StatusCode == StatusCodes.Status400BadRequest);
    }
}