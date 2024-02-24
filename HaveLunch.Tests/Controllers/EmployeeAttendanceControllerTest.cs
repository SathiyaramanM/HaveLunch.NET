using HaveLunch.Controllers;
using HaveLunch.Enums;
using HaveLunch.Models;
using HaveLunch.Tests.Stubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaveLunch.Tests;

public class EmployeeAttendanceControllerTest
{
    [Fact]
    public async Task GetAttendanceDetail_ShouldReturnOk_ForValidEmployeeId()
    {
        var service = new StubEmployeeAttendanceService();
        var controller = new EmployeeAttendanceController(service);

        var result = await controller.GetAttendanceDetail(1, "25/05/2023");

        var returnValue = Assert.IsAssignableFrom<OkObjectResult>(result).Value as EmployeeAttendanceResponse;
        Assert.Equal(1, returnValue.Id);
        Assert.Equal(1, returnValue.EmployeeId);
        Assert.Equal("Bruce", returnValue.EmployeeName);
        Assert.Equal(new DateTime(2023, 05, 25).ToUniversalTime(), returnValue.Date);
        Assert.Equal(AttendanceStatus.YES, returnValue.Status);
    }
    
    [Fact]
    public async Task GetAttendanceDetail_ShouldReturnBadRequest_ForInvalidEmployeeId_WhenGettingException()
    {
        var service = new StubEmployeeAttendanceService();
        var controller = new EmployeeAttendanceController(service);

        var result = await controller.GetAttendanceDetail(5, "25/05/2023");

        var returnValue = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, returnValue.StatusCode);
    }
    
    [Fact]
    public async Task GetEmployeeAttendanceHistory_ShouldReturnOk_ForValidEmployeeId()
    {
        var service = new StubEmployeeAttendanceService();
        var controller = new EmployeeAttendanceController(service);

        var result = await controller.GetEmployeeAttendanceHistory(1);

        var returnValue = Assert.IsAssignableFrom<OkObjectResult>(result).Value as List<EmployeeAttendanceResponse>;
        Assert.True(returnValue.All(x => x.EmployeeId == 1));
    }

    [Fact]
    public async Task GetEmployeeAttendanceHistory_ShouldReturnBadRequest_ForInvalidEmployeeId_WhenGettingException()
    {
        var service = new StubEmployeeAttendanceService();
        var controller = new EmployeeAttendanceController(service);

        var result = await controller.GetEmployeeAttendanceHistory(5);

        var returnValue = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, returnValue.StatusCode);
    }
    
    [Fact]
    public async Task CreateOrUpdateEmployeeAttendance_ShouldReturnOk_ForValidEmployeeId()
    {
        var service = new StubEmployeeAttendanceService();
        var controller = new EmployeeAttendanceController(service);
        var dateTime = DateTime.UtcNow;

        var request = new EmployeeAttendanceRequest()
        {
            Id = 1,
            EmployeeId = 1,
            Date = dateTime,
            Status = AttendanceStatus.YES
        };
        var result = await controller.CreateOrUpdateEmployeeAttendanceDetail(request);

        var returnValue = Assert.IsAssignableFrom<OkObjectResult>(result).Value as EmployeeAttendanceResponse;
        Assert.Equal(1, returnValue.Id);
        Assert.Equal(1, returnValue.EmployeeId);
        Assert.Equal("Bruce", returnValue.EmployeeName);
        Assert.Equal(dateTime, returnValue.Date);
        Assert.Equal(AttendanceStatus.YES, returnValue.Status);
    }
    
    [Fact]
    public async Task CreateOrUpdateEmployeeAttendance_ShouldReturnBadRequest_ForInvalidAttendanceStatus_WhenGettingException()
    {
        var service = new StubEmployeeAttendanceService();
        var controller = new EmployeeAttendanceController(service);
        var dateTime = DateTime.UtcNow;

        var request = new EmployeeAttendanceRequest()
        {
            Id = 1,
            EmployeeId = 1,
            Date = dateTime,
            Status = AttendanceStatus.NOT_SPECIFIED
        };
        var result = await controller.CreateOrUpdateEmployeeAttendanceDetail(request);

        var returnValue = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, returnValue.StatusCode);
    }
}