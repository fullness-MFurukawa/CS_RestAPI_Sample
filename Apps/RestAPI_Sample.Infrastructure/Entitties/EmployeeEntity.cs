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
    [StringLength(36)]
    [Column("emp_uuid")]
    public string Uuid { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Column("emp_name")]
    public string Name { get; set; } = string.Empty;

    [Column("dept_id")]
    public int? DepartmentId { get; set; } // NULL許容に変更

    [ForeignKey("DepartmentId")]
    public DepartmentEntity? Department { get; set; }
}