using Microsoft.EntityFrameworkCore;
using RestAPI_Sample.Infrastructure.Entitties;
namespace RestAPI_Sample.Infrastructure.Contexts;
/// <summary>
/// アプリケーション用データベースコンテキスト（MySQL対応）
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="options"></param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options) { }

    /// <summary>
    /// 部署テーブル
    /// </summary>
    public DbSet<DepartmentEntity> Departments => Set<DepartmentEntity>();

    /// <summary>
    /// 従業員テーブル
    /// </summary>
    public DbSet<EmployeeEntity> Employees => Set<EmployeeEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*
        // Department: dept_uuid に一意制約を追加
        modelBuilder.Entity<DepartmentEntity>()
            .HasIndex(d => d.Uuid)
            .IsUnique();
        // Employee: emp_uuid に一意制約を追加
        modelBuilder.Entity<EmployeeEntity>()
            .HasIndex(e => e.Uuid)
            .IsUnique();
        // Employee と Department のリレーション
        modelBuilder.Entity<EmployeeEntity>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)  // 外部キーは dept_id (int)
            .HasPrincipalKey(d => d.Id)          // 主キーは dept_id (int)
            .HasConstraintName("employee_ibfk_1")
            .OnDelete(DeleteBehavior.SetNull);
        */
    }
}