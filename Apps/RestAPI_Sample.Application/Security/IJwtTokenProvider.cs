namespace RestAPI_Sample.Application.Security;

/// <summary>
/// JWTトークンの生成およびユーザー情報の埋め込みを行うインターフェイス
/// </summary>
public interface IJwtTokenProvider
{
    /// <summary>
    /// 指定されたユーザー情報からJWTトークンを生成する
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <param name="username">ユーザー名</param>
    /// <returns>生成されたJWTトークン文字列</returns>
    string GenerateToken(string userId, string username);
}
