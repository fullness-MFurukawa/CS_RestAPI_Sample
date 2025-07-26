namespace RestAPI_Sample.Application.Security;
/// <summary>
/// パスワードのハッシュ化とパスワード認証インターフェイス
/// ユーザー登録やログイン認証におけるパスワードのセキュリティ処理を標準化する
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// 平文のパスワードを安全な形式でハッシュ化する
    /// 内部的にソルトなどを組み合わせてる
    /// </summary>
    /// <param name="password">ハッシュ化対象の平文パスワード</param>
    /// <returns>ハッシュ化されたパスワード文字列</returns>
    string Hash(string password);
    /// <summary>
    /// 平文のパスワードとハッシュ済みのパスワードを比較して、一致するかどうかを検証する
    /// ハッシュに使われたソルトやアルゴリズムを内部で自動的に解釈して検証する
    /// </summary>
    /// <param name="password">ユーザーが入力した平文のパスワード</param>
    /// <param name="hashedPassword">ハッシュ化済みのパスワード</param>
    /// <returns>一致する場合:true、一致しない場合:false</returns>
    bool Verify(string password, string hashed);
}