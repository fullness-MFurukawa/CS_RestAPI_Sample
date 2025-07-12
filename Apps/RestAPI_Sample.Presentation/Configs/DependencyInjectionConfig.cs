using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Configuration;
using RestAPI_Sample.Infrastructure.Contexts;
using RestAPI_Sample.Infrastructure.Adapters;
using RestAPI_Sample.Infrastructure.Repositories;
using RestAPI_Sample.Infrastructure.Shared;
using RestAPI_Sample.Application.Domains.Repositories;
using RestAPI_Sample.Application.Usecases;
using RestAPI_Sample.Application.Usecases.Employees.Interfaces;
using RestAPI_Sample.Application.Usecases.Employees.Interactors;

namespace RestAPI_Sample.Presentation.Configs;
/// <summary>
/// 依存関係定義クラス
/// </summary>
public static class DependencyInjectionConfig
{
    /// <summary>
    /// 依存関係定義メソッド
    /// </summary>
    /// <param name="config">アプリケーションの構成設定インターフェイス</param>
    /// <param name="services">依存性注入インターフェイス</param>
    public static void ConfigureDependencies(
        IConfiguration config, IServiceCollection services)
    {
        // インフラストラクチャ層の依存関係定義
        AddInfrastructureServices(config, services);
        // アプリケーション層の依存関係定義
        AddApplicationServices(services);
    }

    /// <summary>
    /// インフラストラクチャ層の依存関係定義
    /// </summary>
    /// <param name="config">アプリケーションの構成設定インターフェイス</param>
    /// <param name="services">依存性注入インターフェイス</param>
    private static void AddInfrastructureServices(
        IConfiguration config, IServiceCollection services)
    {
        // AppDbContextを依存性注入に登録
        var connectstr = config.GetConnectionString("MySqlConnection");
        services.AddDbContext<AppDbContext>(options =>
        {
            // デバッグレベルのログをコンソール出力する
            options.LogTo(Console.WriteLine, LogLevel.Debug);
            // MySQLに接続する
            options.UseMySql(connectstr, ServerVersion.AutoDetect(connectstr));
        });
        // Adapterを依存性注入に登録
        services.AddScoped<DepartmentEntityAdapter>();
        services.AddScoped<EmployeeEntityAdapter>();
        // Repositoryインターフェイスの実装を依存性注入に登録
        services.AddScoped<IDepartmentRepository , DepartmentRepository>();
        services.AddScoped<IEmployeeRepository , EmployeeRepository>();
        // IUnitOfWorkインターフェイスの実装を依存性注入に登録
        services.AddScoped<IUnitOfWork , UnitOfWork>(); 
    }

    /// <summary>
    /// アプリケーション層の依存関係定義
    /// </summary>
    /// <param name="services">依存性注入インターフェイス</param>
    private static void AddApplicationServices(IServiceCollection services)
    {
        // ユースケース実現インターフェイスとその実装を登録
        services.AddScoped<ISearchEmployeesByKeywordUseCase,SearchEmployeesByKeywordInteractor>();

    }
}