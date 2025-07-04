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
        Console.WriteLine("[ClassInitialize] options created");

        _sharedContext = new AppDbContext(options);
        Console.WriteLine("[ClassInitialize] AppDbContext created");

        Console.WriteLine("[ClassInitialize] Step: accessing model...");
        var model = _sharedContext.Model;
        Console.WriteLine("[ClassInitialize] Step: model accessed");

        Console.WriteLine("[ClassInitialize] Step: opening connection...");
        _sharedContext.Database.OpenConnection();
        Console.WriteLine("[ClassInitialize] DB connection opened");
    }
    catch (Exception ex)
    {
        Console.WriteLine("[ClassInitialize] エラー: " + ex.Message);
        Console.WriteLine("[ClassInitialize] スタックトレース:\n" + ex.StackTrace);
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
    public async Task SelectAllAsync__ShouldSucceed()
    {
        // すべての部署を取得する
        var results = await _repository.SelectAllAync();
        foreach (var result in results)
        {
            Console.WriteLine(result.ToString());
        }
        Assert.AreEqual(3, results.Count());
    }
}