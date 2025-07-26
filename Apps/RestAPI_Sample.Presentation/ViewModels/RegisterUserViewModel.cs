using System.ComponentModel.DataAnnotations;
namespace RestAPI_Sample.Presentation.ViewModels;
/// <summary>
/// ユーザー登録用 ViewModel（確認パスワード付き）
/// </summary>
public class RegisterUserViewModel
{
    [Required(ErrorMessage = "ユーザー名は必須です。")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "ユーザー名は3〜20文字で入力してください。")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "メールアドレスは必須です。")]
    [EmailAddress(ErrorMessage = "正しいメールアドレス形式を入力してください。")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "パスワードは必須です。")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "パスワードは6文字以上で入力してください。")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "確認用パスワードは必須です。")]
    [Compare("Password", ErrorMessage = "パスワードと確認用パスワードが一致しません。")]
    public string ConfirmPassword { get; set; } = string.Empty;
}