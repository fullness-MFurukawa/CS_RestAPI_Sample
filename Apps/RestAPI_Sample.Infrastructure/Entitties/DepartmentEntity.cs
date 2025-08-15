using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RestAPI_Sample.Infrastructure.Entitties;
/// <summary>
/// department テーブルに対応するEntity Framework Coreのエンティティ
/// </summary>
[Table("department")]// ターゲットテーブル
public class DepartmentEntity
{
    [Key] // 主キーをマッピング
    [Column("dept_id")] // マッピングする列名
    public int Id { get; set; }

    [Required] // NOT NUll
    [StringLength(36)] // データ長は36文字
    [Column("dept_uuid")]// マッピングする列名
    public string Uuid { get; set; } = string.Empty;

    [Required] // NOT NULL
    [StringLength(30)]// データ長は30文字
    [Column("dept_name")]// マッピングする列名
    public string Name { get; set; } = string.Empty;

    // 1対多の「多」側へのナビゲーションプロパティ
    // 部署に配属された従業員
    public ICollection<EmployeeEntity> Employees { get; set; } = new List<EmployeeEntity>();
    public override string ToString()
    {
        return $"Id={Id},Uuid={Uuid},Name={Name}";
    }
}