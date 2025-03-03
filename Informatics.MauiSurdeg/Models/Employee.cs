using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informatics.MauiSurdeg.Models;

[Table("Employee")]
public class Employee
{
[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
[Column("EmployeeID")]
public int Id { get; set; }
[Required]
[Column("EmpNo")]
[MaxLength(100)]
public string Number { get; set; }
[Required]
[Column("EmpName")]
[MaxLength(100)]
public string Name { get; set; }
[Required]
[Column("EmpSalary")]
public int Salary { get; set; }
[Column("DepartmentID")]
public int? DepartmentId { get; set; }
[ForeignKey("DepartmentId")]
public virtual Department Department { get; set; }
}
