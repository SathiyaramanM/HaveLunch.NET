using HaveLunch.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HaveLunch.Services;

public interface IAdminService
{
    Task<AdminCountResponse> GetLunchAttendanceCount(DateTimeOffset date);
}

public class AdminService(AppDbContext appDbContext) : IAdminService
{
    public async Task<AdminCountResponse> GetLunchAttendanceCount(DateTimeOffset date)
    {
        var employeesCount = await appDbContext.EmployeeAttendances.CountAsync(x => x.Date == date);
        return new AdminCountResponse(employeesCount);
    }
}