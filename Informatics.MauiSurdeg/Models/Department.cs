using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informatics.MauiSurdeg.Models;
[Table("Department")]
public class Department
{
[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
[Column("DepartmentID")]
public int Id { get; set; }
[Required]
[Column("DeptName")]
[MaxLength(100)]
public string Name { get; set; }
[Required]
[Column("DeptBudget")]
public int Budget { get; set; }
// Navigation property (One Department -> Many Employees)
public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
