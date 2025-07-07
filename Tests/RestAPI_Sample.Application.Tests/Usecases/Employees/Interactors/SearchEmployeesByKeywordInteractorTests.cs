using Moq;
using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Domains.Repositories;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Application.Usecases.Employees.Interactors;

namespace RestAPI_Sample.Application.Tests.Usecases.Employees.Interactors;
/// <summary>
/// ユースケース:[従業員をキーワード検索する]を実現するインターフェイス実装の単体テストドライバ
/// Moqの利用
/// </summary>
[TestClass]
public class SearchEmployeesByKeywordInteractorTests
{
    private Mock<IEmployeeRepository> _mockRepository = null!;
    private SearchEmployeesByKeywordInteractor _interactor = null!;
    /// <summary>
    /// テストの前処理
    /// </summary>
    [TestInitialize]
    public void Setup()
    {
        // Moqを利用してIEmployeeRepositoryを生成
        _mockRepository = new Mock<IEmployeeRepository>();
        // ユースケースインターフェイス実装の生成
        _interactor = new SearchEmployeesByKeywordInteractor(_mockRepository.Object);
    }
    [TestMethod("有効なキーワードで従業員が返る")]
    public async Task ExecuteAsync_ShouldReturnEmployees()
    {
        var keyword = "田"; // 田が含まれる名前を検索する
        var emp1 = new Employee("b23f8fda-a314-4a9c-84d5-0c621ee25d32", "田中太郎");
        emp1.ChangeDepartment(new Department("3fa85f64-5717-4562-b3fc-2c963f66afa6", "営業部"));
        var emp2 = new Employee("b23f8fda-a314-4a9c-84d5-0c621ee25d33", "田村花子");
        emp2.ChangeDepartment(new Department("4d3eabd8-9f8c-4a2b-9156-081a4e34b93a", "総務部"));
        var emp3 = new Employee("b23f8fda-a314-4a9c-84d5-0c621ee25d34", "山田一郎");
        emp3.ChangeDepartment(new Department("7b12ed56-ffeb-4b94-8c53-23e14f62b15c", "開発部"));
        var expected = new List<Employee> { emp1, emp2, emp3 };
        // モックのRepositoryにテストデータを設定する
        _mockRepository
        .Setup(r => r.SelectByNameLikeAsync(keyword))
        .ReturnsAsync(expected);
        // キーワード検索する
        var result = await _interactor.ExecuteAsync(keyword);
        // ユースケース実現結果を評価する
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("田中太郎", result[0].Name);
        Assert.AreEqual("田村花子", result[1].Name);
        Assert.AreEqual("山田一郎", result[2].Name);
    }

    [TestMethod("該当データがない場合にNotFoundExceptionがスローされる")]
    [ExpectedException(typeof(NotFoundException))]
    public async Task ExecuteAsync_ShouldThrowNotFoundException()
    {
        var keyword = "存在しない名前";
        // 空リストをモックのRepositoryに設定
        _mockRepository
            .Setup(r => r.SelectByNameLikeAsync(keyword))
            .ReturnsAsync(new List<Employee>()); 
        // キーワード検索する
        await _interactor.ExecuteAsync(keyword);
    }
}