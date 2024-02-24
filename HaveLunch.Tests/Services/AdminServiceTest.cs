using HaveLunch.Entities;
using HaveLunch.Enums;
using HaveLunch.Models;
using HaveLunch.Persistence;
using HaveLunch.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace HaveLunch.Tests.Services;

public class AdminServiceTest
{
    private static readonly List<Employee> employees = [
        new() { Id = 1, Name = "Bruce" },
        new() { Id = 2, Name = "Alfred" }
    ];

    private static readonly List<EmployeeAttendance> employeeAttendances = [
        new() { Id = 1, EmployeeId = 1, Employee = employees[0], Date = new DateTime(2023, 5, 25), Status = AttendanceStatus.YES },
        new() { Id = 2, EmployeeId = 2, Employee = employees[1], Date = new DateTime(2023, 5, 25), Status = AttendanceStatus.YES },
        new() { Id = 3, EmployeeId = 1, Employee = employees[0], Date = new DateTime(2023, 5, 26), Status = AttendanceStatus.YES },
        new() { Id = 4, EmployeeId = 2, Employee = employees[1], Date = new DateTime(2023, 5, 26), Status = AttendanceStatus.NO },
        new() { Id = 5, EmployeeId = 1, Employee = employees[0], Date = new DateTime(2023, 5, 27), Status = AttendanceStatus.NO },
    ];

    private readonly Mock<AppDbContext> dbContext;

    public AdminServiceTest()
    {
        dbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        dbContext.Setup(x => x.Employees).ReturnsDbSet(employees);
        dbContext.Setup(x => x.EmployeeAttendances).ReturnsDbSet(employeeAttendances);
    }
    
    [Fact]
    public async Task GetLunchAttendanceCount_ShouldReturnNoOfEmployees_MarkedYesOnGivenDate()
    {
        var service = new AdminService(dbContext.Object);

        var result1 = await service.GetLunchAttendanceCount(new DateTime(2023, 5, 25));
        var result2 = await service.GetLunchAttendanceCount(new DateTime(2023, 5, 26));
        var result3 = await service.GetLunchAttendanceCount(new DateTime(2023, 5, 27));

        Assert.Equal(2, result1.Count);
        Assert.Equal(1, result2.Count);
        Assert.Equal(0, result3.Count);
    }

    [Fact]
    public async Task GetLunchAttendantsList_ShouldReturnAListOfEmployees_MarkedYesOnGivenDate()
    {
        var service = new AdminService(dbContext.Object);

        var result = await service.GetLunchAttendanceList(new DateTime(2023, 5, 25));

        Assert.Contains(new EmployeeResponse(1, "Bruce"), result);
    }
}