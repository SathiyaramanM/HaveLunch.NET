using HaveLunch.Enums;
using HaveLunch.Models;
using HaveLunch.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HaveLunch.Services;

public interface IAdminService
{
    Task<AdminCountResponse> GetLunchAttendanceCount(DateTime date);

    Task<List<EmployeeResponse>> GetLunchAttendanceList(DateTime date, int page = 1);
}

public class AdminService(AppDbContext appDbContext) : IAdminService
{
    public async Task<AdminCountResponse> GetLunchAttendanceCount(DateTime date)
    {
        var employeesCount = await appDbContext.EmployeeAttendances.CountAsync(x => x.Date == date && x.Status == AttendanceStatus.YES);
        return new AdminCountResponse(employeesCount);
    }

    public async Task<List<EmployeeResponse>> GetLunchAttendanceList(DateTime date, int page = 1)
    {
        const int pageSize = 7;
        var employees = await appDbContext
                            .EmployeeAttendances
                            .Where(x => x.Date == date && x.Status == AttendanceStatus.YES)
                            .OrderBy(x => x.EmployeeId)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(x => x.Employee)
                            .Select(x => new EmployeeResponse(x.EmployeeId, x.Employee.Name))
                            .ToListAsync();
        return employees;
    }
}