using Moq;
using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Domains.Repositories;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Application.Usecases;
using RestAPI_Sample.Application.Usecases.Employees.Interactors;

namespace RestAPI_Sample.Application.Tests.Usecases.Employees.Interactors;
/// <summary>
/// ユースケース:[従業員を削除する]を実現するインターフェイス実装の単体テストドライバ
/// Moqの利用
/// </summary>
[TestClass]
public class DeleteEmployeeInteractorTests
{
    private Mock<IEmployeeRepository> _mockEmpRepo = null!;
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;
    private DeleteEmployeeInteractor _interactor = null!;
    /// <summary>
    /// テストの前処理
    /// </summary>
    [TestInitialize]
    public void Setup()
    {
        // Repositoryのモックを生成する
        _mockEmpRepo = new Mock<IEmployeeRepository>();
        // UnitOfWorkのモックを生成する
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        // Usecaseインターフェイスの実装を生成する
        _interactor = new DeleteEmployeeInteractor(_mockEmpRepo.Object, _mockUnitOfWork.Object);
    }

    [TestMethod("指定したIdの従業員が取得できること")]
    public async Task GetEmployeeByIdAsync_ShouldReturnEmployee_WhenExists()
    {
        var id = "8acb02f4-b3a2-4ccf-998d-69f22a8b882f";
        var employee = new Employee(id, "伊藤誠");
        var department = new Department("3fa85f64-5717-4562-b3fc-2c963f66afa6", "営業部");
        employee.ChangeDepartment(department);
        // Repositoryのモックにテストデータを設定する
        _mockEmpRepo.Setup(r => r.SelectByIdAsync(id)).ReturnsAsync(employee);
        // メソッドを実行する
        var result = await _interactor.GetEmployeeByIdAsync(id);
        // 取得結果を検証する
        Assert.AreEqual(employee, result);
    }

    [TestMethod("指定したIDの従業員が存在しない場合、例外をスローすること")]
    public async Task GetEmployeeByIdAsync_ShouldThrow_WhenNotFound()
    {
        var id = "8acb02f4-b3a2-4ccf-998d-69f22a8b882a";
        // Repositoryのモックにテストデータを設定する
        _mockEmpRepo.Setup(r => r.SelectByIdAsync(id)).ReturnsAsync((Employee?)null);
        // メソッドを実行してNotFoundException例外を受け取る
        var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(
            () => _interactor.GetEmployeeByIdAsync(id));
        // 例外メッセージを検証する
        Assert.AreEqual("従業員Id:8acb02f4-b3a2-4ccf-998d-69f22a8b882aに一致する従業員は存在しません。",
            ex.Message);
    }

    [TestMethod("従業員が存在しない場合、NotFoundExceptionがスローされ、ロールバックされる")]
    public async Task DeleteEmployee_ShouldRollback_WhenEmployeeNotFound()
    {
        var id = "8acb02f4-b3a2-4ccf-998d-69f22a8b882a";
        // Repositoryのモックにテストデータを設定する
        _mockEmpRepo.Setup(r => r.DeleteByIdAsync(id)).ReturnsAsync(false);
        // メソッドを実行してNotFoundException例外を受け取る
        var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(
            () => _interactor.DeleteEmployee(id));
        // トランザクションが開始されたことを検証する
        _mockUnitOfWork.Verify(u => u.BeginAsync(), Times.Once);
        // トランザクションがロールバックされたことを検証する
        _mockUnitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
        // コミットメソッドが実行されなかったことを検証する
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        // エラーメッセージを検証する
        Assert.AreEqual(
            "従業員Id:8acb02f4-b3a2-4ccf-998d-69f22a8b882aに一致する従業員は存在しないため、削除はできません。",
            ex.Message);
    }

    [TestMethod("例外が発生した場合、ロールバックされる")]
    public async Task DeleteEmployee_ShouldRollback_WhenExceptionOccurs()
    {
        var id = "3fa85f64-5717-4562-b3fc-2c963f66afa1";
        // Repositoryのモックにテストデータを設定する
        _mockEmpRepo.Setup(r => r.DeleteByIdAsync(id)).ThrowsAsync(new Exception("DBエラー"));
        // メソッドを実行して例外がスローされたことを検証する
        await Assert.ThrowsExceptionAsync<Exception>(
            () => _interactor.DeleteEmployee(id));
        // トランザクションが開始されたことを検証する
        _mockUnitOfWork.Verify(u => u.BeginAsync(), Times.Once);
        // トランザクションがロールバックされたことを検証する
        _mockUnitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
        // コミットメソッドが実行されなかったことを検証する
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [TestMethod("従業員削除に成功した場合、トランザクションがコミットされる")]
    public async Task DeleteEmployee_ShouldCommit_WhenSuccess()
    {
        var id = "3fa85f64-5717-4562-b3fc-2c963f66afa1";
        // Repositoryのモックにテストデータを設定する
        _mockEmpRepo.Setup(r => r.DeleteByIdAsync(id)).ReturnsAsync(true);
        // メソッドを実行する
        await _interactor.DeleteEmployee(id);
        // トランザクションが開始されたことを検証する
        _mockUnitOfWork.Verify(u => u.BeginAsync(), Times.Once);
        // 削除メソッドが実行されたことを検証する
        _mockEmpRepo.Verify(r => r.DeleteByIdAsync(id), Times.Once);
        // トランザクションがコミットされたことを検証する
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        // トランザクションのロールバックが実行されなかったことを検証する
        _mockUnitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
    }
}
