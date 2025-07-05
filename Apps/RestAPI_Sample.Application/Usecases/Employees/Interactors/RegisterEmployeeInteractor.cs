using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Domains.Repositories;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Application.Usecases.Employees.Interfaces;
namespace RestAPI_Sample.Application.Usecases.Employees.Interactors;
/// <summary>
/// ユースケース:[従業員を登録する]を実現するインターフェイスの実装
/// </summary>
public class RegisterEmployeeInteractor : IRegisterEmployeeUseCase
{
    private readonly IDepartmentRepositiry _departmentRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="departmentRepository">ドメインオブジェクト:Department(部署)のCRUD操作インターフェイス</param>
    /// <param name="employeeRepository">ドメインオブジェクト:Employee(従業員)のCRUD操作インターフェイス</param>
    /// <param name="unitOfWork">Unit of Workパターンを利用したトランザクション制御インターフェイス</param>
    public RegisterEmployeeInteractor(
        IDepartmentRepositiry departmentRepository,
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork)
    {
        _departmentRepository = departmentRepository;
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// すべての部署を取得する
    /// クライアント側の[入力画面]で利用するプルダウンを作成するため
    /// </summary>
    /// <returns>Departmentのリスト</returns>
    public async Task<List<Department>> GetDepartmentsAsync()
    {
        var results = await _departmentRepository.SelectAllAync();
        return results;
    }

    /// <summary>
    /// 指定された部署Idの部署を取得する
    /// クライアント側の[確認画面]、[完了画面]で利用するため
    /// </summary>
    /// <param name="id">部署Id</param>
    /// <returns>該当部署</returns>
    /// <exception cref="NotFoundException">該当データが存在しない場合にスローされる</exception>
    public async Task<Department> GetDepartmentByIdAsync(string id)
    {
        var result = await _departmentRepository.SelectByIdAsync(id);
        if (result == null)
        {
            throw new NotFoundException(
                $"部署Id:{id}に一致する部署は存在しません。");
        }
        return result!;
    }

    /// <summary>
    /// 従業員を登録する
    /// </summary>
    /// <param name="employee">登録対象従業員</param>
    /// <returns></returns>
    /// <exception cref="NotFoundException">所属部署が存在しない場合にスローされる</exception>
    public async Task RegisterEmployeeAsync(Employee employee)
    {
        var result = await _departmentRepository.SelectByIdAsync(employee.Department!.Id);
        if (result == null)
        {
            throw new NotFoundException(
            $"部署Id:{employee.Department.Id}に一致する部署は存在したいため、従業員の登録はできません。");
        }
        // トランザクションを開始する
        await _unitOfWork.BeginAsync();
        try
        {
            // 従業員を登録する
            await _employeeRepository.CreateAsync(employee);
            // トランザクションをコミットする
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            // トランザクションをロールバックする
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}