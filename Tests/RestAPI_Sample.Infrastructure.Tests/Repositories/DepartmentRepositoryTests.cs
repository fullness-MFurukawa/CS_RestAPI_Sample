using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RestAPI_Sample.Infrastructure.Adapters;
using RestAPI_Sample.Infrastructure.Contexts;
using RestAPI_Sample.Infrastructure.Repositories;

namespace RestAPI_Sample.Infrastructure.Tests.Repositories;
/// <summary>
/// ドメインオブジェクト:Department(部署)のCRUD操作インターフェイスの実装の単体テストドライバ
/// </summary>
[TestClass]
public class DepartmentRepositoryTests
{
    private static AppDbContext _sharedContext = null!;
    private IDbContextTransaction _transaction = null!;
    private DepartmentRepository _repository = null!;

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
        // ターゲット:DepartmentRepositryの生成 
        _repository = new DepartmentRepository(_sharedContext, new DepartmentEntityAdapter());
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

    [TestMethod("すべての部署を取得する")]
    public async Task SelectAllAsync_ShouldSucceed()
    {
        // すべての部署を取得する
        var results = await _repository.SelectAllAync();
        foreach (var result in results)
        {
            Console.WriteLine(result.ToString());
        }
        Assert.AreEqual(3, results.Count());
    }

    [TestMethod("部署IDにより1件の部署を取得する")]
    public async Task SelectByIdAsync_ShouldSucceed()
    {
        // 営業部のUUID
        var expectedId = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
        var department = await _repository.SelectByIdAsync(expectedId);
        Assert.IsNotNull(department);
        Assert.AreEqual(expectedId, department?.Id);
        Assert.AreEqual("営業部", department?.Name);
    }
    
    [TestMethod("存在しない部署IDの場合はnullを返す")]
    public async Task SelectByIdAsync_NotFoundId_ShouldReturnNull()
    {
        // 存在しないUUID
        var notFoundId = "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"; 
        var result = await _repository.SelectByIdAsync(notFoundId);
        Assert.IsNull(result);
    }
}