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
}
