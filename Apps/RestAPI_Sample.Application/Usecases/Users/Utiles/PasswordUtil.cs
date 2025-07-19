using System.Security.Cryptography;

namespace RestAPI_Sample.Application.Usecases.Users.Utiles;

/// <summary>
/// パスワードのソルト生成・ハッシュ化・検証を行うユーティリティクラス
/// </summary>
public static class PasswordUtil
{
    /// <summary>
    /// ランダムなソルト（Salt）を生成する
    /// </summary>
    /// <param name="size">生成するソルトのバイト長（デフォルト:64バイト）</param>
    /// <returns>Base64文字列で表されたソルト</returns>
    public static string GenerateSalt(int size = 64)
    {
        // 安全な乱数を用いてソルトを生成
        var saltBytes = RandomNumberGenerator.GetBytes(size);
        // Base64文字列に変換して返す（DBなどに保存しやすいため）
        return Convert.ToBase64String(saltBytes);
    }

    /// <summary>
    /// パスワードとソルトからハッシュ値を生成する（PBKDF2 + SHA256）
    /// </summary>
    /// <param name="password">プレーンなパスワード</param>
    /// <param name="salt">Base64文字列で与えられたソルト</param>
    /// <param name="iterations">ストレッチングの繰り返し回数（デフォルト:10000）</param>
    /// <returns>Base64文字列で表されたハッシュ値</returns>
    public static string HashPassword(string password, string salt, int iterations = 10000)
    {
        // Base64形式のソルトをバイト配列に変換
        var saltBytes = Convert.FromBase64String(salt);

        // PBKDF2（Password-Based Key Derivation Function 2）を使ってハッシュを生成
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,              // 入力パスワード
            saltBytes,             // ソルト
            iterations,            // ストレッチ回数（計算負荷）
            HashAlgorithmName.SHA256 // 使用するハッシュアルゴリズム
        );

        // 256bit（32バイト）のハッシュを取得し、Base64で返す
        var hashBytes = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    /// 入力されたパスワードと保存済みハッシュ値が一致するか検証する
    /// </summary>
    /// <param name="password">ユーザー入力のパスワード</param>
    /// <param name="salt">保存されたソルト（Base64）</param>
    /// <param name="storedHash">保存されたハッシュ値（Base64）</param>
    /// <returns>true:一致（認証成功）、false:不一致（認証失敗）</returns>
    public static bool VerifyPassword(string password, string salt, string storedHash)
    {
        // 入力値から同じ条件でハッシュを再計算
        var hash = HashPassword(password, salt);
        // 保存済みのハッシュと比較
        return hash == storedHash;
    }
}