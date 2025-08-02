using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace RestAPI_Sample.Presentation.Configs;

/// <summary>
/// JWT認証のサービス登録を行う拡張クラス
/// </summary>
public static class JwtAuthenticationExtensions
{
    /// <summary>
    /// JWT認証の構成をサービスに追加する拡張メソッド
    /// </summary>
    /// <param name="services">サービスコレクション</param>
    /// <param name="configuration">アプリケーション構成（appsettingsなど）</param>
    /// <returns>IServiceCollection（チェーン可能）</returns>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // appsettings.jsonの "JwtSettings" セクションを取得
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        // JWT認証の設定を追加
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,                      // Issuer（発行者）を検証
                ValidateAudience = true,                    // Audience（対象者）を検証
                ValidateLifetime = true,                    // 有効期限を検証
                ValidateIssuerSigningKey = true,            // 署名キーの検証
                ValidIssuer = issuer,                       // 有効なIssuer
                ValidAudience = audience,                   // 有効なAudience
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey))       // 秘密鍵を使用した署名検証
            };
        });
        return services;
    }
}