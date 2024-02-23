using HaveLunch.Entities;
using HaveLunch.Models;
using HaveLunch.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HaveLunch.Services;

public interface IEmployeeAttendanceService
{
    Task<EmployeeAttendanceResponse> CreateOrUpdateEmployeeAttendance(EmployeeAttendanceRequest request);

    Task<EmployeeAttendanceResponse> GetEmployeeAttendanceDetail(int employeeId, DateTimeOffset date);

    Task<List<EmployeeAttendanceResponse>> GetEmployeeAttendanceHistory(int employeeId, int page = 1);
}

public class EmployeeAttendanceService(AppDbContext appDbContext) : IEmployeeAttendanceService
{
    public async Task<EmployeeAttendanceResponse> CreateOrUpdateEmployeeAttendance(EmployeeAttendanceRequest request)
    {
        var employee = await appDbContext.Employees.FirstOrDefaultAsync(x => x.Id == request.EmployeeId);
        if(employee == null)
        {
            throw new Exception("Employee Not Found!");
        }
        var employeeAttendance = await appDbContext
                                    .EmployeeAttendances
                                    .Include(x => x.Employee)
                                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.EmployeeId == request.EmployeeId);
        if(employeeAttendance == null)
        {
            employeeAttendance = new EmployeeAttendance
            {
                Employee = employee,
                Date = new DateTimeOffset(request.Date.ToDateTime(TimeOnly.MinValue)),
                Status = request.Status
            };
            await appDbContext.EmployeeAttendances.AddAsync(employeeAttendance);
            await appDbContext.SaveChangesAsync();
        }
        else
        {
            employeeAttendance.Status = request.Status;
            await appDbContext.SaveChangesAsync();
        }
        return new EmployeeAttendanceResponse()
        {
            Id = employeeAttendance.Id,
            EmployeeId = employeeAttendance.EmployeeId,
            EmployeeName = employeeAttendance.Employee.Name,
            Date = employeeAttendance.Date,
            Status = employeeAttendance.Status
        };
    }

    public async Task<EmployeeAttendanceResponse> GetEmployeeAttendanceDetail(int employeeId, DateTimeOffset date)
    {
        var employeeAttendance = await appDbContext
                                    .EmployeeAttendances
                                    .Include(x => x.Employee)
                                    .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.Date == date) 
                                    ?? throw new Exception("Employee Attendance Not Found for the given date");
        return new EmployeeAttendanceResponse()
        {
            Id = employeeAttendance.Id,
            EmployeeId = employeeAttendance.EmployeeId,
            EmployeeName = employeeAttendance.Employee.Name,
            Date = employeeAttendance.Date,
            Status = employeeAttendance.Status
        };
    }

    public async Task<List<EmployeeAttendanceResponse>> GetEmployeeAttendanceHistory(int employeeId, int page = 1)
    {
        const int pageSize = 7;
        var employees = await appDbContext
                            .EmployeeAttendances
                            .Where(x => x.EmployeeId == employeeId)
                            .OrderByDescending(x => x.Date)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Select(x => new EmployeeAttendanceResponse())
                            .ToListAsync();
        return employees;
    }
}