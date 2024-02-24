using HaveLunch.Entities;
using HaveLunch.Enums;
using HaveLunch.Models;
using HaveLunch.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HaveLunch.Services;

public interface IEmployeeAttendanceService
{
    Task<EmployeeAttendanceResponse> CreateOrUpdateEmployeeAttendance(EmployeeAttendanceRequest request);

    Task<EmployeeAttendanceResponse> GetEmployeeAttendanceDetail(int employeeId, DateTime date);

    Task<List<EmployeeAttendanceResponse>> GetEmployeeAttendanceHistory(int employeeId, int page = 1);
}

public class EmployeeAttendanceService(AppDbContext appDbContext) : IEmployeeAttendanceService
{
    public async Task<EmployeeAttendanceResponse> CreateOrUpdateEmployeeAttendance(EmployeeAttendanceRequest request)
    {
        if(request.Status == AttendanceStatus.NOT_SPECIFIED) 
        {
            throw new Exception("Invalid Attendance Status");
        }
        var employee = await appDbContext.Employees.FirstOrDefaultAsync(x => x.Id == request.EmployeeId) 
                        ?? throw new Exception("Employee Not Found!");
        var employeeAttendance = await appDbContext
                                    .EmployeeAttendances
                                    .Include(x => x.Employee)
                                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.EmployeeId == request.EmployeeId);
        if(employeeAttendance == null)
        {
            employeeAttendance = new EmployeeAttendance
            {
                Employee = employee,
                Date = request.Date.ToUniversalTime(),
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

    public async Task<EmployeeAttendanceResponse> GetEmployeeAttendanceDetail(int employeeId, DateTime date)
    {
        var employee = await appDbContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeId) 
                            ?? throw new Exception("Employee Not Found!");
        var employeeAttendance = await appDbContext
                                    .EmployeeAttendances
                                    .Include(x => x.Employee)
                                    .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.Date == date);
        if(employeeAttendance == null)
        {
            return new EmployeeAttendanceResponse()
            {
                Id = 0,
                EmployeeId = employeeId,
                EmployeeName = employee.Name,
                Date = date,
                Status = AttendanceStatus.NOT_SPECIFIED
            };
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