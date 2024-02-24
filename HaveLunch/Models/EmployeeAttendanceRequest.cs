using HaveLunch.Enums;

namespace HaveLunch.Models;

public class EmployeeAttendanceRequest
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    
    public DateTime Date { get; set; }

    public AttendanceStatus Status { get; set; }
}