using RestAPI_Sample.Presentation.Configs;


var builder = WebApplication.CreateBuilder(args);

// アプリケーション全体の依存関係（DI）を一括登録
builder.Services.AddApplicationDependencies(builder.Configuration);
// JWT認証設定の追加（拡張メソッドで分離）
builder.Services.AddJwtAuthentication(builder.Configuration);
// Swaggerサービスを拡張メソッドで追加（JWT対応含む）
builder.Services.AddSwaggerWithJwt();
var app = builder.Build();

// 開発環境でのみ Swagger を有効化（UI含む）
app.UseSwaggerIfDevelopment(app.Environment);

// ミドルウェア構成
app.UseHttpsRedirection();
app.UseAuthentication();    // 認証ミドルウェア
app.UseAuthorization();     // 認可ミドルウェア
// ルーティング構成（コントローラーをエンドポイントにマッピング）
app.MapControllers();
// アプリケーション起動
app.Run();
