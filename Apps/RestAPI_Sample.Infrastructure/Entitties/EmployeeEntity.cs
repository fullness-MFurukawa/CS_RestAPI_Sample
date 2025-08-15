using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RestAPI_Sample.Infrastructure.Entitties;
/// <summary>
/// employee テーブルに対応するEntity Framework Coreのエンティティ
/// </summary>
[Table("employee")]// ターゲットテーブル
public class EmployeeEntity
{
    [Key] // 主キーをマッピング
    [Column("emp_id")]// マッピングする列名
    public int Id { get; set; }

    [Required]// NOT NULL
    [StringLength(36)]// データ長は36文字
    [Column("emp_uuid")]// マッピングする列名
    public string Uuid { get; set; } = string.Empty;

    [Required]// NOT NULL
    [StringLength(30)]// データ長は30文字
    [Column("emp_name")]// マッピングする列名
    public string Name { get; set; } = string.Empty;

    [Column("dept_id")] // マッピングする列名
    public int? DepartmentId { get; set; }

    // Departmentエンティティへのナビゲーションプロパティ
    // DepartmentIdプロパティの値と外部キー関係にある
    // null許容にし、従業員が部署に所属していないケースも許可する
    [ForeignKey("DepartmentId")]
    public DepartmentEntity? Department { get; set; }

    public override string ToString()
    {
        return $"Id={Id},Uuid={Uuid},Name={Name},DepartmentId={DepartmentId},Department={Department}";
    }
}