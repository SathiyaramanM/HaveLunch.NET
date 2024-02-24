using System.ComponentModel.DataAnnotations.Schema;
using HaveLunch.Enums;

namespace HaveLunch.Entities;

public class EmployeeAttendance
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public DateTime Date { get; set; }

    public AttendanceStatus Status { get; set; }
}