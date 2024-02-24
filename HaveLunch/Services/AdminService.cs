using HaveLunch.Models;
using HaveLunch.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HaveLunch.Services;

public interface IAdminService
{
    Task<AdminCountResponse> GetLunchAttendanceCount(DateTime date);
}

public class AdminService(AppDbContext appDbContext) : IAdminService
{
    public async Task<AdminCountResponse> GetLunchAttendanceCount(DateTime date)
    {
        var employeesCount = await appDbContext.EmployeeAttendances.CountAsync(x => x.Date == date && x.Status == Enums.AttendanceStatus.YES);
        return new AdminCountResponse(employeesCount);
    }
}