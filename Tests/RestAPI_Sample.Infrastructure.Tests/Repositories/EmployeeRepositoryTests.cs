using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Infrastructure.Adapters;
using RestAPI_Sample.Infrastructure.Contexts;
using RestAPI_Sample.Infrastructure.Repositories;

namespace RestAPI_Sample.Infrastructure.Tests.Repositories;
/// <summary>
/// ドメインオブジェクト:Employee(従業員)のCRUD操作インターフェイスの実装の単体テストドライバ
/// </summary>
[TestClass]
public class EmployeeRepositoryTests
{
    private static AppDbContext _sharedContext = null!;
    private IDbContextTransaction _transaction = null!;
    private EmployeeRepository _repository = null!;
    /// <summary>
    /// テストクラスの初期化処理
    /// </summary>
    /// <param name="context"></param>
    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        try
        {
            var connectionString = "Server=localhost;Port=3306;Database=restapi_sample;User Id=root;Password=root;";
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)))
                .Options;
            _sharedContext = new AppDbContext(options);
            var model = _sharedContext.Model;
            _sharedContext.Database.OpenConnection();
        }
        catch
        {
            throw;
        }
    }
    /// <summary>
    /// テストクラスの後処理
    /// </summary>
    [ClassCleanup]
    public static void ClassCleanup()
    {
        if (_sharedContext != null)
        {
            // データベース接続の解除
            _sharedContext.Database.CloseConnection();
            // アプリケーション用DbContextの破棄
            _sharedContext.Dispose();
        }
    }
    /// <summary>
    /// テストメソッド実行の前処理
    /// </summary>
    [TestInitialize]
    public void TestInitialize()
    {
        // トランザクションの開始
        _transaction = _sharedContext.Database.BeginTransaction();
        // ターゲット:EmployeeRepositryの生成 
        _repository = new EmployeeRepository(_sharedContext,
            new EmployeeEntityAdapter(new DepartmentEntityAdapter()));
    }
    /// <summary>
    /// テストメソッド実行後の後処理
    /// </summary>
    [TestCleanup]
    public void TestCleanup()
    {
        // トランザクションのロールバック
        _transaction.Rollback();
        // トランザクションの破棄
        _transaction.Dispose();
    }

    [TestMethod("指定された従業員Idに一致する従業員を取得する")]
    public async Task SelectByIdAsync_ShouldSucceed()
    {
        // 伊藤誠
        var existingId = "8acb02f4-b3a2-4ccf-998d-69f22a8b882f";
        var result = await _repository.SelectByIdAsync(existingId);
        Assert.IsNotNull(result);
        Assert.AreEqual(existingId, result!.Id);
        Assert.AreEqual("伊藤誠", result!.Name);
        Assert.AreEqual("4d3eabd8-9f8c-4a2b-9156-081a4e34b93a", result!.Department!.Id);
        Assert.AreEqual("総務部", result!.Department!.Name);
        Console.WriteLine(result);
    }

    [TestMethod("存在しない従業員Idに対してnullを返す")]
    public async Task SelectByIdAsync_NotFound_ShouldReturnNull()
    {
        // 存在しないUUID
        var notFoundId = Guid.NewGuid().ToString();
        var result = await _repository.SelectByIdAsync(notFoundId);
        // nullを返すことを検証
        Assert.IsNull(result);
        Console.WriteLine("存在しないIDに対してnullが返されました。");
    }

    [TestMethod("従業員を追加できる")]
    public async Task CreateAsync_ShouldSucceed()
    {
        // 登録するデータを用意する
        var employee = new Employee("テスト太郎");
        var department = new Department("3fa85f64-5717-4562-b3fc-2c963f66afa6", "営業部");
        employee.ChangeDepartment(department);
        Console.WriteLine(employee);
        // 従業員を登録する
        await _repository.CreateAsync(employee);
        // データベースから再取得して検証
        var inserted = await _repository.SelectByIdAsync(employee.Id);
        Assert.IsNotNull(inserted);
        Assert.AreEqual("テスト太郎", inserted!.Name);
        Assert.AreEqual("3fa85f64-5717-4562-b3fc-2c963f66afa6", inserted!.Department!.Id);
        Assert.AreEqual("営業部", inserted!.Department!.Name);
    }

    [TestMethod("指定された従業員Idの従業員を更新する")]
    public async Task UpdateByIdAsync_ShouldUpdate()
    {
        // 対象の従業員ID（山田彩）
        var targetId = "0a2bc6e9-02cc-4f6a-bf29-58e00e934c67";
        // 新しい氏名に変更する
        var updatedName = "斎藤彩";
        var employee = new Employee(targetId, updatedName);
        var department = new Department("4d3eabd8-9f8c-4a2b-9156-081a4e34b93a", "総務部");
        // 従業員を変更する
        var result = await _repository.UpdateByIdAsync(employee);
        Assert.IsTrue(result);

        // 更新されたかを確認する
        var updated = await _repository.SelectByIdAsync(targetId);
        Assert.AreEqual(updatedName, updated!.Name);
        Console.WriteLine(updated);
    }

    [TestMethod("存在しない従業員Idの従業員を更新する")]
    public async Task UpdateByIdAsync_InvalidId_ShouldReturnFalse()
    {
        // 存在しないUUID
        var invalidId = Guid.NewGuid().ToString();
        var employee = new Employee(invalidId, "架空太郎");
        var department = new Department("4d3eabd8-9f8c-4a2b-9156-081a4e34b93a", "総務部");
        employee.ChangeDepartment(department);
        var result = await _repository.UpdateByIdAsync(employee);
        Assert.IsFalse(result);
    }

    [TestMethod("指定された従業員Idの従業員を削除する")]
    public async Task DeleteByIdAsync_ShouldDelete()
    {
        // 存在する従業員のIdを用意する（山田彩）
        var employeeId = "0a2bc6e9-02cc-4f6a-bf29-58e00e934c67";
        var result = await _repository.DeleteByIdAsync(employeeId);
        Assert.IsTrue(result);
        // 削除されていることを確認する
        var deleted = await _repository.SelectByIdAsync(employeeId);
        Assert.IsNull(deleted);
    }

    [TestMethod("存在しない従業員Idの従業員を削除する")]
    public async Task DeleteByIdAsync_InvalidId_ShouldReturnFalse()
    {
        // 存在しない従業員Idを用意する
        var nonExistentId = Guid.NewGuid().ToString();
        var result = await _repository.DeleteByIdAsync(nonExistentId);
        Assert.IsFalse(result);
    }

    [TestMethod("指定されたキーワードに一致する従業員を取得する（ヒットあり）")]
    public async Task SelectByNameLikeAsync_Hit_ShouldReturnEmployees()
    {
        // テストデータを用意する
        var dept = new Department("3fa85f64-5717-4562-b3fc-2c963f66afa6", "営業部");
        var emp1 = new Employee(Guid.NewGuid().ToString(), "テスト一郎");
        var emp2 = new Employee(Guid.NewGuid().ToString(), "テスト二郎");
        emp1.ChangeDepartment(dept);
        emp2.ChangeDepartment(dept);
        await _repository.CreateAsync(emp1);
        await _repository.CreateAsync(emp2);
        // キーワード検索する
        var result = await _repository.SelectByNameLikeWithDepartmentAsync("テスト");
        Assert.IsNotNull(result);
        Assert.IsTrue(result!.Count >= 2);
        Assert.IsTrue(result.Any(e => e.Name == "テスト一郎"));
        Assert.IsTrue(result.Any(e => e.Name == "テスト二郎"));
    }

    [TestMethod("指定されたキーワードに一致する従業員を取得する（ヒットなし）")]
    public async Task SelectByNameLikeAsync_Miss_ShouldReturnEmpty()
    {
        var result = await _repository.SelectByNameLikeWithDepartmentAsync("未登録名");
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result!.Count);
    }
}