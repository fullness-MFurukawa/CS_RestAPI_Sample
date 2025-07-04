using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI_Sample.Infrastructure.Entitties;
/// <summary>
/// department テーブルに対応するEntity Framework Coreのエンティティ
/// </summary>
[Table("department")]
public class DepartmentEntity
{
    [Key]
    [Column("dept_id")]
    public int Id { get; set; }

    [Required]
    [Column("dept_uuid", TypeName = "char(36)")]
    public string Uuid { get; set; } = default!;

    [Required]
    [Column("dept_name", TypeName = "varchar(20)")]
    public string Name { get; set; } = default!;

    public ICollection<EmployeeEntity> Employees { get; set; } = new List<EmployeeEntity>();
}