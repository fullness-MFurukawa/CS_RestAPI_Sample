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
        // ユニーク制約
        modelBuilder.Entity<DepartmentEntity>()
            .HasIndex(d => d.Uuid)
            .IsUnique();
        modelBuilder.Entity<EmployeeEntity>()
            .HasIndex(e => e.Uuid)
            .IsUnique();

        // リレーション設定（employee → department）
        modelBuilder.Entity<EmployeeEntity>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentUuid) 
            .HasConstraintName("fk_employee_department")
            .OnDelete(DeleteBehavior.SetNull);
    }
}