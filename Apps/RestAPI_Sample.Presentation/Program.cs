using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Controllerサービスを登録（これがないと MapControllers で例外）
builder.Services.AddControllers();

// アプリケーション構成要素の依存関係定義
RestAPI_Sample.Presentation.Configs
    .DependencyInjectionConfig.ConfigureDependencies(
        builder.Configuration, builder.Services);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>{
    c.EnableAnnotations(); // Swashbuckle のアノテーション属性
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "C# Web API編(ASP.NET Core) サンプルアプリケーション",
        Version = "1.0.0", // ✅ ← バージョンは "v1" などの文字列が必要
        Description = "Web APIサンプル"
    });
});

var app = builder.Build();

// ✅ Controllerのルートマッピングを追加
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // ✅ これを必ず指定する（Swagger JSONへのエンドポイント）
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "C# Web API編(ASP.NET Core) サンプルアプリケーション");

    });
}

app.UseHttpsRedirection();
/*
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();
*/

// ✅ Controller ルートのマッピング（AddControllersが前提）
app.MapControllers();

app.Run();
/*
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
*/
