using HaveLunch.Entities;
using Microsoft.EntityFrameworkCore;

namespace HaveLunch.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }
    
    public DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
}