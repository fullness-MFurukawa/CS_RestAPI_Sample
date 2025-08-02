using Microsoft.OpenApi.Models;

namespace RestAPI_Sample.Presentation.Configs;

/// <summary>
/// Swaggerのサービス登録拡張クラス
/// </summary>
public static class SwaggerServiceExtensions
{
    /// <summary>
    /// SwaggerおよびJWT認証連携の設定をDIコンテナに追加する拡張メソッド
    /// </summary>
    /// <param name="services">DIコンテナ</param>
    /// <returns>IServiceCollection（チェーン可能）</returns>
    public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
    {
        // Swaggerの基本設定とドキュメント定義
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "APS.NET Core C# REST API Sample",  // APIドキュメントのタイトル
                    Version = "Version 1.0.0"             // バージョン
                });

                // JWT認証用のセキュリティスキームの定義
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT 認証トークンを 'Bearer {token}' の形式で入力してください",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                // セキュリティ要件の追加（全てのエンドポイントにBearer認証を適用）
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()  // 認可スコープが必要な場合はここに追加
                    }
                });
            });
        return services;
    }

    /// <summary>
    /// 開発環境の場合のみ SwaggerUI を有効にする拡張メソッド
    /// </summary>
    /// <param name="app">アプリケーションビルダー</param>
    /// <param name="env">ホスティング環境</param>
    /// <returns>IApplicationBuilder（チェーン可能）</returns>
    public static IApplicationBuilder UseSwaggerIfDevelopment(this IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            // Swagger生成およびUI表示設定
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestAPI Sample v1");
            });
        }
        return app;
    }
}