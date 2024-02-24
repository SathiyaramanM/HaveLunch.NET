using HaveLunch.Enums;

namespace HaveLunch.Models;

public class EmployeeAttendanceResponse
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }

    public DateTime Date { get; set; }

    public AttendanceStatus Status { get; set; }
}
