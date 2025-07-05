using Microsoft.EntityFrameworkCore;
using RestAPI_Sample.Infrastructure.Contexts;

namespace RestAPI_Sample.Infrastructure.Tests.Contexts;

/// <summary>
/// アプリケーション用DbContextの単体テストドライバ
/// </summary>
[TestClass]
public class AddDbContextTests
{
    private static TestContext? _testContext;

    [ClassInitialize]
    public static void ClassInit(TestContext context)
    {
        _testContext = context;
    }

    [TestMethod("データベース接続ができる")]
    public void DbConnect_ShouldSucceed()
    {
        var connectionString = "Server=localhost;Port=3306;Database=restapi_sample;User Id=root;Password=root;";
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)),
                mySqlOptions => mySqlOptions.EnableRetryOnFailure())
            .Options;

        var context = new AppDbContext(options);
        try
        {
            context.Database.OpenConnection();
            _testContext?.WriteLine("✅ DB接続成功しました。");
            Assert.IsTrue(true);
        }
        catch (Exception ex)
        {
            _testContext?.WriteLine($"❌ 例外が発生しました: {ex.Message}");
            _testContext?.WriteLine($"スタックトレース:\n{ex.StackTrace}");
            Assert.Fail("接続に失敗しました。");
        }
        finally
        {
            context.Database.CloseConnection();
        }
    }
    
}