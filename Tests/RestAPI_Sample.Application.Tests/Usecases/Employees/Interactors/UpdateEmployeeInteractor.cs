using System.Runtime.CompilerServices;
using Moq;
using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Domains.Repositories;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Application.Usecases;
using RestAPI_Sample.Application.Usecases.Employees.Interactors;

namespace RestAPI_Sample.Application.Tests.Usecases.Employees.Interactors;
/// <summary>
/// ユースケース:[従業員を変更する]を実現するインターフェイス実装の単体テストドライバ
/// Moqの利用
/// </summary>
[TestClass]
public class UpdateEmployeeInteractorTests
{
    private Mock<IDepartmentRepository> _mockDeptRepo = null!;
    private Mock<IEmployeeRepository> _mockEmpRepo = null!;
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;
    private UpdateEmployeeInteractor _interactor = null!;

    /// <summary>
    /// テストの初期化処理
    /// </summary>
    [TestInitialize]
    public void Setup()
    {
        // IDepartmentRepositoryインターフェイス実装のモックを生成する
        _mockDeptRepo = new Mock<IDepartmentRepository>();
        // IEmployeeRepositoryインターフェイス実装のモックを生成する
        _mockEmpRepo = new Mock<IEmployeeRepository>();
        // IUnitOfWorkインターフェイス実装のモックを生成する
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        // Usecaseインターフェイス実装のインスタンスを生成する
        _interactor = new UpdateEmployeeInteractor(
            _mockDeptRepo.Object,
            _mockEmpRepo.Object,
            _mockUnitOfWork.Object
        );
    }
    /// <summary>
    /// GetEmployeeByIdAsync
    /// </summary>
    [TestMethod("従業員が存在する場合、GetEmployeeByIdAsync()は該当従業員を返す")]
    public async Task GetEmployeeByIdAsync_WhenExists()
    {
        var department = new Department("3fa85f64-5717-4562-b3fc-2c963f66afa8", "人事部");
        var employee = new Employee("3fa85f64-5717-4562-b3fc-2c963f66afa1", "浜田三郎");
        employee.ChangeDepartment(department);
        // テストデータをRepositoryのモックに設定する
        _mockEmpRepo.Setup(r => r.SelectByIdAsync(employee.Id)).ReturnsAsync(employee);
        // メソッドを実行する
        var result = await _interactor.GetEmployeeByIdAsync(employee.Id);
        // 取得結果を検証する
        Assert.AreEqual(employee, result);
    }

    /// <summary>
    /// GetEmployeeByIdAsync
    /// </summary>
    [TestMethod("従業員が存在しない場合、GetEmployeeByIdAsync()はNotFoundExceptionをスローする")]
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

    [TestMethod("部署が存在する場合、UpdateEmployeeAsync()は従業員を変更できる")]
    public async Task UpdateEmployeeAsync_ShouldSucceed()
    {
        var department = new Department("3fa85f64-5717-4562-b3fc-2c963f66afa6", "営業部");
        var employee = new Employee("3fa85f64-5717-4562-b3fc-2c963f66afa1", "浜田三郎");
        employee.ChangeDepartment(department);
        // Repositoryのモックにテストデータを設定する
        _mockDeptRepo.Setup(r => r.SelectByIdAsync(employee.Department!.Id)).ReturnsAsync(department);
        _mockEmpRepo.Setup(r => r.SelectByIdAsync(employee.Id)).ReturnsAsync(employee);
        _mockEmpRepo.Setup(r => r.UpdateByIdAsync(employee));
        // メソッドを実行する
        await _interactor.UpdateEmployeeAsync(employee);
        //　トランザクションが開始されたことを検証する
        _mockUnitOfWork.Verify(u => u.BeginAsync(), Times.Once);
        // 変更メメソッドが実行されたことを検証する
        _mockEmpRepo.Verify(r => r.UpdateByIdAsync(employee), Times.Once);
        // トランザクションがコミットされたことを検証する
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [TestMethod("部署が存在しない場合、UpdateEmployeeAsync()はNotFoundExceptionをスローする")]
    public async Task UpdateEmployeeAsync__ShouldThrow_WhenNotFound_Case1()
    {
        var department = new Department("3fa85f64-5717-4562-b3fc-2c963f66afa8", "存在しない部署");
        var employee = new Employee("3fa85f64-5717-4562-b3fc-2c963f66afa1", "浜田三郎");
        employee.ChangeDepartment(department);
        // Repositoryのモックにテストデータを設定する
        _mockDeptRepo.Setup(r => r.SelectByIdAsync(employee.Department!.Id)).ReturnsAsync((Department?)null);
        _mockEmpRepo.Setup(r => r.SelectByIdAsync(employee.Id)).ReturnsAsync(employee);
        // メソッドを実行する
        var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(
            () => _interactor.UpdateEmployeeAsync(employee));
        // 例外メッセージを検証する
        Assert.AreEqual("部署Id:3fa85f64-5717-4562-b3fc-2c963f66afa8の部署は存在しないため、従業員の変更はできません。",
            ex.Message);
        // トランザクションが開始されていないことを検証する
        _mockUnitOfWork.Verify(u => u.BeginAsync(), Times.Never);
        // UpdateByIdAsync()メソッドが実行されていないことを検証する
        _mockEmpRepo.Verify(r => r.UpdateByIdAsync(It.IsAny<Employee>()), Times.Never);
        // ロールバックメソッドが実行されていないことを検証する
        _mockUnitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
    }

    [TestMethod("従業員が存在しない場合、UpdateEmployeeAsync()はNotFoundExceptionをスローする")]
    public async Task UpdateEmployeeAsync__ShouldThrow_WhenNotFound_Case2()
    {
        var department = new Department("3fa85f64-5717-4562-b3fc-2c963f66afa8", "営業部");
        var employee = new Employee("3fa85f64-5717-4562-b3fc-2c963f66afa1", "浜田三郎");
        employee.ChangeDepartment(department);
        // Repositoryのモックにテストデータを設定する
        _mockDeptRepo.Setup(r => r.SelectByIdAsync(employee.Department!.Id)).ReturnsAsync(department);
        _mockEmpRepo.Setup(r => r.SelectByIdAsync(employee.Id)).ReturnsAsync((Employee?)null);
        // メソッドを実行する
        var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(
            () => _interactor.UpdateEmployeeAsync(employee));
        // 例外メッセージを検証する
        Assert.AreEqual("従業員Id:3fa85f64-5717-4562-b3fc-2c963f66afa1の従業員は存在しないため、従業員の変更はできません。",
            ex.Message);
        // トランザクションが開始されてたことを検証する
        _mockUnitOfWork.Verify(u => u.BeginAsync(), Times.Once);
        // UpdateByIdAsync()メソッドが実行されたことを検証する
        _mockEmpRepo.Verify(r => r.UpdateByIdAsync(It.IsAny<Employee>()), Times.Once);
        // ロールバックメソッドが実行されたことを検証する
        _mockUnitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
    }

    [TestMethod("変更処理で例外が発生した場合、トランザクションをロールバックする")]
    public async Task UpdateEmployeeAsync_ShouldRollback()
    {
        var department = new Department("3fa85f64-5717-4562-b3fc-2c963f66afa8", "営業部");
        var employee = new Employee("3fa85f64-5717-4562-b3fc-2c963f66afa1", "浜田三郎");
        employee.ChangeDepartment(department);
        // テストデータをRepositoryのモックに設定する
        _mockDeptRepo.Setup(r => r.SelectByIdAsync(department.Id)).ReturnsAsync(department);
        _mockEmpRepo.Setup(r => r.UpdateByIdAsync(employee)).ThrowsAsync(new Exception("DBエラー"));

        // メソッドを実行してスローされた例外を受け取る
        await Assert.ThrowsExceptionAsync<Exception>(
            () => _interactor.UpdateEmployeeAsync(employee));
        // トランザクションが開始されたことを検証する
        _mockUnitOfWork.Verify(u => u.BeginAsync(), Times.Once);
        // トランザクションがロールバックされたことを検証する
        _mockUnitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
        // トランザクションがコミットされなかったことを検証する
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }
}