using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestAPI_Sample.Infrastructure.Contexts;
using RestAPI_Sample.Infrastructure.Adapters;
using RestAPI_Sample.Infrastructure.Repositories;
using RestAPI_Sample.Infrastructure.Shared;
using RestAPI_Sample.Application.Domains.Repositories;
using RestAPI_Sample.Application.Usecases;
using RestAPI_Sample.Application.Usecases.Employees.Interfaces;
using RestAPI_Sample.Application.Usecases.Employees.Interactors;
using RestAPI_Sample.Application.Usecases.Users.interfaces;
using RestAPI_Sample.Application.Usecases.Users.Interactors;
using RestAPI_Sample.Presentation.ViewModels;
using RestAPI_Sample.Application.Security;
using RestAPI_Sample.Infrastructure.Security;

namespace RestAPI_Sample.Presentation.Configs;

/// <summary>
/// DI設定（インフラ・アプリケーション・プレゼンテーション層）をまとめて追加する拡張クラス
/// </summary>
public static class ApplicationDependencyExtensions
{

    /// <summary>
    /// アプリ全体の依存関係（DI）を一括追加する拡張メソッド
    /// </summary>
    /// <param name="services">サービスコレクション</param>
    /// <param name="config">構成情報</param>
    /// <returns>IServiceCollection（チェーン可能）</returns>
    public static IServiceCollection AddApplicationDependencies(
        this IServiceCollection services, IConfiguration config)
    {
        // 各層の依存関係を追加
        services.AddInfrastructureDependencies(config);
        services.AddApplicationLayerDependencies();
        services.AddPresentationLayerDependencies();
        return services;
    }

    /// <summary>
    /// インフラストラクチャ層の依存関係を追加
    /// </summary>
    private static IServiceCollection AddInfrastructureDependencies(
        this IServiceCollection services, IConfiguration config)
    {
        // DbContext の登録
        var connectstr = config.GetConnectionString("MySqlConnection");
        services.AddDbContext<AppDbContext>(options =>
        {
            options.LogTo(Console.WriteLine, LogLevel.Debug);
            options.UseMySql(connectstr, ServerVersion.AutoDetect(connectstr));
        });

        // Adapter の登録
        services.AddScoped<DepartmentEntityAdapter>();
        services.AddScoped<EmployeeEntityAdapter>();
        services.AddScoped<UserEntityAdapter>();
        // Repository の登録
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        // Unit of Work の登録
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        // セキュリティ関連
        services.AddScoped<IPasswordHasher, AspNetPasswordHasher>();
        services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
        return services;
    }

    /// <summary>
    /// アプリケーション層の依存関係を追加
    /// </summary>
    private static IServiceCollection AddApplicationLayerDependencies(this IServiceCollection services)
    {
        // ユースケース（UseCase）の実装を登録
        services.AddScoped<ISearchEmployeesByKeywordUseCase, SearchEmployeesByKeywordInteractor>();
        services.AddScoped<IRegisterEmployeeUseCase, RegisterEmployeeInteractor>();
        services.AddScoped<IUpdateEmployeeUsecase, UpdateEmployeeInteractor>();
        services.AddScoped<IDeleteEmployeeUsecase, DeleteEmployeeInteractor>();
        services.AddScoped<IRegisterUserUsecase, RegisterUserInteractor>();
        services.AddScoped<ILoginUserUsecase, LoginUserInteractor>();

        return services;
    }

    /// <summary>
    /// プレゼンテーション層の依存関係を追加
    /// </summary>
    private static IServiceCollection AddPresentationLayerDependencies(this IServiceCollection services)
    {
        // MVCコントローラーとViewModelアダプタの登録
        services.AddControllers();
        services.AddScoped<RegisterEmployeeViewModelAdapter>();
        services.AddScoped<UpdateEmployeeViewModelAdapter>();
        services.AddScoped<RegisterUserViewModelAdapter>();
        services.AddScoped<LoginUserViewModelAdapter>();

        return services;
    }
}