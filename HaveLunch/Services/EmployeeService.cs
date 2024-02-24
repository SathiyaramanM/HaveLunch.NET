using HaveLunch.Entities;
using HaveLunch.Models;
using HaveLunch.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HaveLunch.Services;

public interface IEmployeeService
{
    Task<EmployeeResponse> LoginAsync(EmployeeLoginModel model);
}

public class EmployeeService(AppDbContext appDbContext) : IEmployeeService
{
    public async Task<EmployeeResponse> LoginAsync(EmployeeLoginModel model)
    {
        var employee = await appDbContext.Employees.FirstOrDefaultAsync(x => x.Id == model.Id);
        if (employee == null)
        {
            employee = new Employee() { Id = model.Id, Name = model.Name };
            await appDbContext.AddAsync(employee);
                await appDbContext.SaveChangesAsync();
        }
        else
        {
            if (employee.Name != model.Name)
            {
                employee.Name = model.Name;
                await appDbContext.SaveChangesAsync();
            }
        }
        return new EmployeeResponse(employee.Id, employee.Name);
    }
}