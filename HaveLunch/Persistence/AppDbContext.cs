using HaveLunch.Entities;
using Microsoft.EntityFrameworkCore;

namespace HaveLunch.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public virtual DbSet<Employee> Employees { get; set; }
    
    public virtual DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
}