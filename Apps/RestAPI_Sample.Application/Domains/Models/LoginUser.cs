namespace RestAPI_Sample.Application.Domains.Models;

/// <summary>
/// ログイン処理に利用する一時的なユーザードメインモデル
/// </summary>
public class LoginUser
{
    /// <summary>
    /// ユーザー名またはメールアドレス
    /// </summary>
    public string UsernameOrEmail { get; }
    /// <summary>
    /// パスワード
    /// </summary>
    public string Password { get; }

    public LoginUser(string usernameOrEmail, string password)
    {
        UsernameOrEmail = usernameOrEmail;
        Password = password;
    }
}