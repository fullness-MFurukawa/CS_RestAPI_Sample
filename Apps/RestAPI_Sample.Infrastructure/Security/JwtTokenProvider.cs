using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestAPI_Sample.Application.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace RestAPI_Sample.Infrastructure.Security;
/// <summary>
/// JWTトークンを生成する実装クラス
/// アプリケーション設定ファイルからIssuer・Audience・SecretKeyなどを読み込み、
/// ユーザー情報を元に署名付きトークンを生成
/// </summary>
public class JwtTokenProvider : IJwtTokenProvider
{
    private readonly IConfiguration _config;
    /// <summary>
    /// コンストラクタ：設定情報を注入する
    /// </summary>
    /// <param name="config">アプリケーション設定情報（appsettings.jsonなど）</param>
    public JwtTokenProvider(IConfiguration config)
    {
        _config = config;
    }
    /// <summary>
    /// 指定されたユーザーIDとユーザー名に基づいてJWTトークンを生成する
    /// </summary>
    /// <param name="userId">ユーザーId（サブジェクト）</param>
    /// <param name="username">ユーザー名（表示用）</param>
    /// <returns>生成されたJWTトークン文字列</returns>
    public string GenerateToken(string userId, string username)
    {
        
        // 設定ファイルからJWT関連情報を取得
        var jwtSettings = _config.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expiresInMinutes = int.Parse(jwtSettings["ExpiresInMinutes"] ?? "60");

        // 秘密鍵を使ってトークン署名用のセキュリティキーを作成
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // トークンに含めるクレーム（ユーザーに関する情報）を定義
        var claims = new[]
        {
            // サブジェクト（ユーザーId）
            new Claim(JwtRegisteredClaimNames.Sub, userId),             
            // 表示名（ユーザー名など）  
            new Claim(JwtRegisteredClaimNames.UniqueName, username),      
            // トークンの一意Id
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
        };

        // JWTトークンの本体を生成
        var token = new JwtSecurityToken(
            issuer,                        // 発行者
            audience,                      // 対象（クライアント）
            claims,                        // クレーム（中身）
            expires: DateTime.UtcNow.AddMinutes(expiresInMinutes), // 有効期限
            signingCredentials: creds      // 署名情報
        );
        // トークンを文字列として返却
        return new JwtSecurityTokenHandler().WriteToken(token);
        
    }
}