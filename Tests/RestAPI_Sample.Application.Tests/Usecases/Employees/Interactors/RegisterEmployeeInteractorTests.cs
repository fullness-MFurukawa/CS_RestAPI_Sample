using Moq;
using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Domains.Repositories;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Application.Usecases;
using RestAPI_Sample.Application.Usecases.Employees.Interactors;

namespace RestAPI_Sample.Application.Tests.Usecases.Employees.Interactors;

/// <summary>
/// ユースケース:[従業員を登録する]を実現するインターフェイス実装の単体テストドライバ
/// Moqの利用
/// </summary>
[TestClass]
public class RegisterEmployeeInteractorTests
{
    private Mock<IDepartmentRepository> _mockDeptRepo = null!;
    private Mock<IEmployeeRepository> _mockEmpRepo = null!;
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;
    private RegisterEmployeeInteractor _interactor = null!;

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
        _interactor = new RegisterEmployeeInteractor(
            _mockDeptRepo.Object,
            _mockEmpRepo.Object,
            _mockUnitOfWork.Object
        );
    }
    /// <summary>
    /// GetDepartmentsAsync()がすべての部署を取得して返すことを検証する
    /// </summary>
    [TestMethod("GetDepartmentsAsync()は部署一覧を取得して返す")]
    public async Task GetDepartmentsAsync_ShouldReturnDepartments()
    {
        var departments = new List<Department>
        {
            new Department("3fa85f64-5717-4562-b3fc-2c963f66afa6", "総務部"),
            new Department("4d3eabd8-9f8c-4a2b-9156-081a4e34b93a", "開発部"),
            new Department("7b12ed56-ffeb-4b94-8c53-23e14f62b15c", "営業部")
        };
        // モックのSelectAllAync()メソッドに返すデータを設定する
        _mockDeptRepo.Setup(repo => repo.SelectAllAync()).ReturnsAsync(departments);
        // メソッドを実行する
        var result = await _interactor.GetDepartmentsAsync();
        // 結果が期待通りかを検証する
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("総務部", result[0].Name);
        Assert.AreEqual("開発部", result[1].Name);
        Assert.AreEqual("営業部", result[2].Name);
    }

    [TestMethod("部署が存在する場合、GetDepartmentByIdAsync()は該当部署を返す")]
    public async Task GetDepartmentByIdAsync_WhenExists()
    {
        var id = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
        var expected = new Department(id, "営業部");
        // テストデータをRepositoryのモックに設定する
        _mockDeptRepo.Setup(r => r.SelectByIdAsync(id)).ReturnsAsync(expected);
        // メソッドを実行する
        var result = await _interactor.GetDepartmentByIdAsync(id);
        // 実行結果を検証する
        Assert.IsNotNull(result);
        Assert.AreEqual(expected.Id, result.Id);
        Assert.AreEqual(expected.Name, result.Name);
    }

    [TestMethod("部署が存在しない場合、GetDepartmentByIdAsync()はNotFoundExceptionをスローする")]
    public async Task GetDepartmentByIdAsync_WhenNotExists()
    {
        var id = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
        // テストデータをRepositoryのモックに設定する
        _mockDeptRepo.Setup(r => r.SelectByIdAsync(id)).ReturnsAsync((Department?)null);
        // NotFoundExceptionがスローされることを検証する
        var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(
            () => _interactor.GetDepartmentByIdAsync(id));
        Assert.AreEqual("部署Id:3fa85f64-5717-4562-b3fc-2c963f66afa6に一致する部署は存在しません。",
                 ex.Message);
    }

    [TestMethod("部署が存在する場合、従業員を登録できる")]
    public async Task RegisterEmployeeAsync_ShouldSucceed()
    {
        var department = new Department("3fa85f64-5717-4562-b3fc-2c963f66afa6", "営業部");
        var employee = new Employee("佐々木一郎");
        employee.ChangeDepartment(department);
        // テストデータをRepositoryのモックに設定する
        _mockDeptRepo.Setup(r => r.SelectByIdAsync(department.Id)).ReturnsAsync(department);
        // メソッドを実行する
        await _interactor.RegisterEmployeeAsync(employee);
        //　トランザクションが開始されたことを検証する
        _mockUnitOfWork.Verify(u => u.BeginAsync(), Times.Once);
        // 登録メメソッドが実行されたことを検証する
        _mockEmpRepo.Verify(r => r.CreateAsync(employee), Times.Once);
        // トランザクションがコミットされたことを検証する
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [TestMethod("部署が存在しない場合、NotFoundExceptionをスローする")]
    public async Task RegisterEmployeeAsync_WhenDepartmentNotFound()
    {
        var department = new Department("3fa85f64-5717-4562-b3fc-2c963f66afa8", "存在しない部署");
        var employee = new Employee("3fa85f64-5717-4562-b3fc-2c963f66afa1", "村上次郎");
        employee.ChangeDepartment(department);
        // テストデータをRepositoryのモックに設定する
        _mockDeptRepo.Setup(r => r.SelectByIdAsync(department.Id)).ReturnsAsync((Department?)null);
        // メソッドを実行してスローされたNotFoundExceptionを受け取る
        var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(
            () => _interactor.RegisterEmployeeAsync(employee));

        // RepositoryのCreateAsync()メソッドが呼び出されていないことを検証する
        _mockEmpRepo.Verify(r => r.CreateAsync(It.IsAny<Employee>()), Times.Never);
        // UnitofWorkのBeginAsync()メソッドが呼び出されていないことを検証する
        _mockUnitOfWork.Verify(u => u.BeginAsync(), Times.Never);
        // エラーメッセージを検証する
        Assert.AreEqual("部署Id:3fa85f64-5717-4562-b3fc-2c963f66afa8に一致する部署は存在しないため、従業員の登録はできません。",
            ex.Message);
    }
    [TestMethod("登録処理で例外が発生した場合、トランザクションをロールバックする")]
    public async Task RegisterEmployeeAsync_ShouldRollback()
    {
        var department = new Department("3fa85f64-5717-4562-b3fc-2c963f66afa8", "人事部");
        var employee = new Employee("3fa85f64-5717-4562-b3fc-2c963f66afa1", "浜田三郎");
        employee.ChangeDepartment(department);
        // テストデータをRepositoryのモックに設定する
        _mockDeptRepo.Setup(r => r.SelectByIdAsync(department.Id)).ReturnsAsync(department);
        _mockEmpRepo.Setup(r => r.CreateAsync(employee)).ThrowsAsync(new Exception("DBエラー"));
        // メソッドを実行してスローされた例外を受け取る
        await Assert.ThrowsExceptionAsync<Exception>(
            () => _interactor.RegisterEmployeeAsync(employee));
        // トランザクションが開始されたことを検証する
        _mockUnitOfWork.Verify(u => u.BeginAsync(), Times.Once);
        // トランザクションがロールバックされたことを検証する
        _mockUnitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
        // トランザクションがコミットされなかったことを検証する
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }
}