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
    [StringLength(36)]
    [Column("dept_uuid")]
    public string Uuid { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Column("dept_name")]
    public string Name { get; set; } = string.Empty;

    // 1対多の「多」側へのナビゲーション
    public ICollection<EmployeeEntity> Employees { get; set; } = new List<EmployeeEntity>();
    public override string ToString()
    {
        return $"Id={Id},Uuid={Uuid},Name={Name}";
    }

}