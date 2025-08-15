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

    /// <summary>
    /// ユーザーテーブル
    /// </summary>
    public DbSet<UserEntity> Users => Set<UserEntity>();

    /// <summary>
    /// モデル構成（エンティティとDBのマッピング設定）を行う
    /// 一意制約・リレーション・制約名・削除時動作などを定義
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Department: dept_uuid に一意制約を追加
        modelBuilder.Entity<DepartmentEntity>()
            .HasIndex(d => d.Uuid)
            .IsUnique();
        // Employee: emp_uuid に一意制約を追加
        modelBuilder.Entity<EmployeeEntity>()
            .HasIndex(e => e.Uuid)
            .IsUnique();
        // EmployeeとDepartmentのリレーション
        modelBuilder.Entity<EmployeeEntity>()
            .HasOne(e => e.Department)           // 従業員は1つの部署に所属
            .WithMany(d => d.Employees)          // 部署は複数の従業員を持つ
            .HasForeignKey(e => e.DepartmentId)  // 外部キーは dept_id (int)
            .HasPrincipalKey(d => d.Id)          // 主キーは dept_id (int)
            .HasConstraintName("employee_ibfk_1")// 外部キー制約名を明示
            .OnDelete(DeleteBehavior.SetNull);   // 部署削除時、外部キーをNULLに設定

        // ユーザーエンティティの制約（ユニークインデックスなど）を定義可能
        modelBuilder.Entity<UserEntity>()
            .Property(u => u.UserUuid)
            .HasColumnType("char(36)");
        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.UserUuid)
            .IsUnique();

        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}