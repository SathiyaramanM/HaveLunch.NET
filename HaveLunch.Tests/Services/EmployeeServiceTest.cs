using HaveLunch.Entities;
using HaveLunch.Models;
using HaveLunch.Persistence;
using HaveLunch.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace HaveLunch.Tests.Services;

public class EmployeeServiceTest
{
    [Fact]
    public async Task LoginAsync_ShouldReturnSameLoginDetails()
    {
        var sampleData = new List<Employee>() { new() { Id = 1, Name = "Bruce" } };
        var dbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        dbContext.Setup(x => x.Employees).ReturnsDbSet(sampleData);
        var service = new EmployeeService(dbContext.Object);

        var model = new EmployeeLoginModel
        {
            Id = 1,
            Name = "Bruce"
        };
        var result = await service.LoginAsync(model);

        Assert.Equal(result.Id, model.Id);
        Assert.Equal(result.Name, model.Name);
    }

    [Fact]
    public async Task LoginAsync_ShouldCreateLoginIfNotExist()
    {
        var dbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        dbContext.Setup(x => x.Employees).ReturnsDbSet([]);
        var service = new EmployeeService(dbContext.Object);

        var model = new EmployeeLoginModel
        {
            Id = 1,
            Name = "Bruce"
        };
        var result = await service.LoginAsync(model);

        Assert.Equal(result.Id, model.Id);
        Assert.Equal(result.Name, model.Name);
    }

    [Fact]
    public async Task LoginAsync_ShouldUpdateDetailsIfAlreadyExist()
    {
        var sampleData = new List<Employee>() { new() { Id = 1, Name = "Bruce" } };
        var dbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        dbContext.Setup(x => x.Employees).ReturnsDbSet(sampleData);
        var service = new EmployeeService(dbContext.Object);

        var model = new EmployeeLoginModel
        {
            Id = 1,
            Name = "Alfred"
        };
        var result = await service.LoginAsync(model);

        Assert.Equal(result.Id, model.Id);
        Assert.Equal(result.Name, model.Name);
    }
}