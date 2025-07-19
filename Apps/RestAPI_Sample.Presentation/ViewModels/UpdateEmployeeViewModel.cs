using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI_Sample.Presentation.ViewModels;

/// <summary>
/// 従業員変更 ViewModelクラス
/// </summary>
public class UpdateEmployeeViewModel
{
    // 入力された従業員Id
    [Required(ErrorMessage = "従業員Idは必須です。")]
    [GuidFormat(ErrorMessage = "従業員IdはUUID形式で指定してください。")]
    public string Id { get; set; } = string.Empty;

    // 入力された従業員名
    [Required(ErrorMessage = "従業員名は必須です。")]
    [StringLength(30 , ErrorMessage ="従業員名は{0}文字以内です。")]
    public string Name { get; set; } = string.Empty;

    // 選択された部署Id
    [Required(ErrorMessage = "従業員名は必須です。")]
    [Remote("/api/employees/update/VerifyDepartmentId")]
    public string DepartmentId { get; set; } = string.Empty;
}