using System.ComponentModel.DataAnnotations.Schema;

namespace HaveLunch.Entities;

public class Employee
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
}