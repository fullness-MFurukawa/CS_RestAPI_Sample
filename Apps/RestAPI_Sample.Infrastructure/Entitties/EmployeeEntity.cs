using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI_Sample.Infrastructure.Entitties;
/// <summary>
/// employee テーブルに対応するEntity Framework Coreのエンティティ
/// </summary>
[Table("employee")]
public class EmployeeEntity
{
    [Key]
    [Column("emp_id")]
    public int Id { get; set; }

    [Required]
    [Column("emp_uuid", TypeName = "char(36)")]
    public string Uuid { get; set; } = default!;

    [Required]
    [Column("emp_name", TypeName = "varchar(20)")]
    public string Name { get; set; } = default!;

    [Column("dept_uuid", TypeName = "char(36)")]
    public string? DepartmentUuid { get; set; }

    [ForeignKey("DepartmentUuid")]
    public DepartmentEntity? Department { get; set; }
}