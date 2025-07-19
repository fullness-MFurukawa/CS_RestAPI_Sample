using System.ComponentModel.DataAnnotations;

namespace RestAPI_Sample.Presentation.ViewModels;
/// <summary>
/// UUID形式をチェックするカスタムバリデーション
/// </summary>
public class GuidFormatAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return true; // Required属性と併用することを前提

        return Guid.TryParse(value.ToString(), out _);
    }

    public override string FormatErrorMessage(string value)
    {
        return $"{value} はUUID形式で指定してください。";
    }
}