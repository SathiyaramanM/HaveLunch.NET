using HaveLunch.Entities;
using HaveLunch.Enums;
using HaveLunch.Models;
using HaveLunch.Persistence;
using HaveLunch.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace HaveLunch.Tests.Services;

public class EmployeeAttendanceServiceTest
{
    private static readonly List<Employee> employees = [
        new() { Id = 1, Name = "Bruce" },
        new() { Id = 2, Name = "Alfred" }
    ];

    private readonly Mock<AppDbContext> dbContext;

    public EmployeeAttendanceServiceTest()
    {
        dbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        dbContext.Setup(x => x.Employees).ReturnsDbSet(employees);
    }

    [Fact]
    public async Task GetEmployeeAttendanceDetail_ShouldReturnStatus_ForGivenEmployeeOnGivenDate()
    {
        List<EmployeeAttendance> employeeAttendances = [
            new() { Id = 1, Employee = employees[0], EmployeeId = 1, Date = new DateTime(2023, 5, 25), Status = AttendanceStatus.YES },
        ];
        dbContext.Setup(x => x.EmployeeAttendances).ReturnsDbSet(employeeAttendances);
        var service = new EmployeeAttendanceService(dbContext.Object);

        var result = await service.GetEmployeeAttendanceDetail(1, new DateTime(2023, 5, 25));

        Assert.Equal(employeeAttendances[0].Id, result.Id);
        Assert.Equal(employeeAttendances[0].EmployeeId, result.EmployeeId);
        Assert.Equal(employeeAttendances[0].Date, result.Date);
        Assert.Equal(employeeAttendances[0].Status, result.Status);
    }

    [Fact]
    public async Task GetEmployeeAttendanceDetail_ShouldReturnNotSpecifiedStatus_WhenAttendanceNotFound()
    {
        dbContext.Setup(x => x.EmployeeAttendances).ReturnsDbSet([]);
        var service = new EmployeeAttendanceService(dbContext.Object);

        var date = new DateTime(2023, 5, 25);
        var result = await service.GetEmployeeAttendanceDetail(employees[0].Id, date);

        Assert.Equal(0, result.Id);
        Assert.Equal(employees[0].Id, result.EmployeeId);
        Assert.Equal(employees[0].Name, result.EmployeeName);
        Assert.Equal(date, result.Date);
        Assert.Equal(AttendanceStatus.NOT_SPECIFIED, result.Status);
    }

    [Fact]
    public async Task GetEmployeeAttendanceDetail_ShouldThrowException_WhenEmployeeNotFound()
    {
        dbContext.Setup(x => x.EmployeeAttendances).ReturnsDbSet([]);
        var service = new EmployeeAttendanceService(dbContext.Object);

        await Assert.ThrowsAsync<Exception>(async () => {
            var result = await service.GetEmployeeAttendanceDetail(3, new DateTime(2023, 5, 25));
        });
    }

    [Theory]
    [InlineData(AttendanceStatus.YES)]
    [InlineData(AttendanceStatus.NO)]
    public async Task CreateOrUpdateEmployeeAttendance_ShouldMarkStatus_ForGivenEmployeeOnSelectDate(AttendanceStatus status)
    {
        List<EmployeeAttendance> employeeAttendances = [
            new() { Id = 1, Employee = employees[0], EmployeeId = 1, Date = new DateTime(2023, 5, 25), Status = AttendanceStatus.YES },
        ];
        dbContext.Setup(x => x.EmployeeAttendances).ReturnsDbSet(employeeAttendances);
        var service = new EmployeeAttendanceService(dbContext.Object);

        var model = new EmployeeAttendanceRequest()
        {
            Id = employeeAttendances[0].Id,
            EmployeeId = employeeAttendances[0].EmployeeId,
            Date = employeeAttendances[0].Date,
            Status = status
        };

        var result = await service.CreateOrUpdateEmployeeAttendance(model);

        Assert.Equal(employeeAttendances[0].Id, result.Id);
        Assert.Equal(employeeAttendances[0].EmployeeId, result.EmployeeId);
        Assert.Equal(employeeAttendances[0].Date, result.Date);
        Assert.Equal(status, result.Status);
    }

    [Fact]
    public async Task CreateOrUpdateEmployeeAttendance_ShouldThrowException_WhenMarkInvalidStatus()
    {
        List<EmployeeAttendance> employeeAttendances = [
            new() { Id = 1, Employee = employees[0], EmployeeId = 1, Date = new DateTime(2023, 5, 25), Status = AttendanceStatus.YES },
        ];
        dbContext.Setup(x => x.EmployeeAttendances).ReturnsDbSet(employeeAttendances);
        var service = new EmployeeAttendanceService(dbContext.Object);

        var model = new EmployeeAttendanceRequest()
        {
            Id = employeeAttendances[0].Id,
            EmployeeId = employeeAttendances[0].EmployeeId,
            Date = employeeAttendances[0].Date,
            Status = AttendanceStatus.NOT_SPECIFIED
        };

        await Assert.ThrowsAsync<Exception>(async () => {
            var result = await service.CreateOrUpdateEmployeeAttendance(model);
        });
    }
}