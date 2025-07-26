using Microsoft.AspNetCore.Identity;
using RestAPI_Sample.Application.Security;

namespace RestAPI_Sample.Infrastructure.Security;

/// <summary>
/// ASP.NET Core IdentityのPasswordHasherを利用したパスワードハッシュ化インターフェイス実装クラス
/// </summary>
public class AspNetPasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _passwordHasher = new();

    /// <summary>
    /// 平文のパスワードをハッシュ化します。
    /// </summary>
    /// <param name="password">ハッシュ化する平文パスワード。</param>
    /// <returns>ハッシュ化されたパスワード文字列。</returns>
    public string Hash(string password)
    {
        return _passwordHasher.HashPassword(null, password);
    }

    /// <summary>
    /// 平文パスワードとハッシュ済みパスワードを照合します。
    /// </summary>
    /// <param name="password">ユーザーが入力した平文パスワード。</param>
    /// <param name="hashed">保存されているハッシュ化済みパスワード。</param>
    /// <returns>一致する場合は <c>true</c>、一致しない場合は <c>false</c>。</returns>
    public bool Verify(string password, string hashed)
    {
        var result = _passwordHasher.VerifyHashedPassword(null, hashed, password);
        return result == PasswordVerificationResult.Success ||
               result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}