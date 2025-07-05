using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        _repository = new EmployeeRepository(_sharedContext, new EmployeeEntityAdapter());
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
        var notFoundId = "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"; 
        var result = await _repository.SelectByIdAsync(notFoundId);
        // nullを返すことを検証
        Assert.IsNull(result);
        Console.WriteLine("存在しないIDに対してnullが返されました。");
    }
}