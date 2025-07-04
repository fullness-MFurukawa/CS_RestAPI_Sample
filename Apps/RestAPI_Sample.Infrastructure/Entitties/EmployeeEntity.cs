using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI_Sample.Infrastructure.Entitties;
/// <summary>
/// employee テーブルに対応するEntity Framework Coreのエンティティ
/// </summary>
[Table("employee")]
public class EmployeeEntity
{
    /// <summary>
    /// 主キー（自動採番）
    /// </summary>
    [Key]
    [Column("emp_id")]
    public int Id { get; set; }

    /// <summary>
    /// UUID（ユニーク制約付き）
    /// </summary>
    [Required]
    [Column("emp_uuid", TypeName = "char(36)")]
    public string Uuid { get; set; } = default!;

    /// <summary>
    /// 従業員名（最大20文字、NOT NULL）
    /// </summary>
    [Required]
    [Column("emp_name", TypeName = "varchar(20)")]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 外部キー: 部署UUID
    /// </summary>
    [Column("dept_uuid", TypeName = "char(36)")]
    public string? DepartmentUuid { get; set; }

    /// <summary>
    /// ナビゲーションプロパティ: 所属部署（null許容）
    /// </summary>
    [ForeignKey("DepartmentId")]
    public DepartmentEntity? Department { get; set; }
}