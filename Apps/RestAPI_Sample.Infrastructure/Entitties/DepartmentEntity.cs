using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI_Sample.Infrastructure.Entitties;
/// <summary>
/// department テーブルに対応するEntity Framework Coreのエンティティ
/// </summary>
[Table("department")]
public class DepartmentEntity
{
    /// <summary>
    /// 主キー（自動採番）
    /// </summary>
    [Key]
    [Column("dept_id")]
    public int Id { get; set; }

    /// <summary>
    /// UUID（ユニーク制約付き）
    /// </summary>
    [Required]
    [Column("dept_uuid", TypeName = "char(36)")]
    public string Uuid { get; set; } = default!;

    /// <summary>
    /// 部署名（最大20文字、NOT NULL）
    /// </summary>
    [Required]
    [Column("dept_name", TypeName = "varchar(20)")]
    public string Name { get; set; } = default!;

    // 逆参照プロパティ
    public ICollection<EmployeeEntity>? Employees { get; set; }

    public override string ToString()
    {
        return $"Id={Id} , Uuid={Uuid} , Name={Name}";
    }
}