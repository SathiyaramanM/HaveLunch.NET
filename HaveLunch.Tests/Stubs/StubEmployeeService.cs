using HaveLunch.Entities;
using HaveLunch.Models;
using HaveLunch.Services;

public class StubEmployeeService : IEmployeeService
{
    public Task<Employee> LoginAsync(EmployeeLoginModel model)
    {
        return Task.FromResult(new Employee() {
            Id = model.Id,
            Name = model.Name
        });
    }
}