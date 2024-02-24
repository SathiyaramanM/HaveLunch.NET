using HaveLunch.Models;
using HaveLunch.Services;

namespace HaveLunch.Tests.Stubs;

public class StubAdminService : IAdminService
{
    public Task<AdminCountResponse> GetLunchAttendanceCount(DateTime date)
    {
        if (date == new DateTime(2023, 5, 25).ToUniversalTime())
        {
            return Task.FromResult(new AdminCountResponse(2));
        }
        else if (date == new DateTime(2023, 5, 26).ToUniversalTime())
        {
            return Task.FromResult(new AdminCountResponse(1));
        }
        else
        {
            return Task.FromResult(new AdminCountResponse(0));
        }
    }

    public Task<List<EmployeeResponse>> GetLunchAttendanceList(DateTime date, int page = 1)
    {
        if(date == new DateTime(2023, 5, 25).ToUniversalTime())
        {
            return Task.FromResult<List<EmployeeResponse>>([
                new EmployeeResponse(1, "Bruce"), 
                new EmployeeResponse(2, "Alfred")
            ]);
        }
        else if(date == new DateTime(2023, 5, 26).ToUniversalTime())
        {
            return Task.FromResult<List<EmployeeResponse>>([
                new EmployeeResponse(1, "Bruce")
            ]);
        }
        else
        {
            return Task.FromResult<List<EmployeeResponse>>([]);
        }
    }
}
