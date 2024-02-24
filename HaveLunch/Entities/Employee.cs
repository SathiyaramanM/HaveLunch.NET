using System.ComponentModel.DataAnnotations.Schema;

namespace HaveLunch.Entities;

public class Employee : IEquatable<Employee>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }

    public bool Equals(Employee other)
    {
        if(other == null)
            return false;
        return Id == other.Id && Name == other.Name;
    }
}