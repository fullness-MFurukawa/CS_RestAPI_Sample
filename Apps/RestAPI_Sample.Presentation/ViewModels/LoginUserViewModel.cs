using System.ComponentModel.DataAnnotations;

namespace RestAPI_Sample.Presentation.ViewModels;

/// <summary>
/// ログイン用の入力情報を保持するViewModel
/// </summary>
public class LoginUserViewModel
{
    /// <summary>
    /// ログインID（ユーザー名またはメールアドレス）
    /// </summary>
    [Required(ErrorMessage = "ユーザー名またはメールアドレスは必須です。")]
    [StringLength(100, ErrorMessage = "ユーザー名またはメールアドレスは100文字以内で入力してください。")]
    public string UsernameOrEmail { get; set; } = string.Empty;

    /// <summary>
    /// パスワード
    /// </summary>
    [Required(ErrorMessage = "パスワードは必須です。")]
    [StringLength(100, ErrorMessage = "パスワードは100文字以内で入力してください。")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}