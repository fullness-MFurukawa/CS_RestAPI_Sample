using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Domains.Repositories;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Application.Usecases.Employees.Interfaces;
namespace RestAPI_Sample.Application.Usecases.Employees.Interactors;
/// <summary>
/// ユースケース:[従業員を削除する]を実現するインターフェイスの実装
/// </summary>
public class DeleteEmployeeInteractor : IDeleteEmployeeUsecase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="employeeRepository">ドメインオブジェクト:Employee(従業員)のCRUD操作インターフェイス</param>
    /// <param name="unitOfWork">Unit of Workパターンを利用したトランザクション制御インターフェイス</param>
    public DeleteEmployeeInteractor(
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }
    /// <summary>
    /// 指定された従業員Idの従業員を取得する
    /// クライアント側の[削除画面]で利用するため
    /// </summary>
    /// <param name="id">従業員Id</param>
    /// <returns>該当従業員</returns>
    /// <exception cref="NotFoundException">従業員が存在しない場合にスローされる</exception>
    public async Task DeleteEmployeeAsync(string id)
    {
        // トランザクションを開始する
        await _unitOfWork.BeginAsync();
        try
        {
            var result = await _employeeRepository.DeleteByIdAsync(id);
            if (!result)
            {
                throw new NotFoundException(
                    $"従業員Id:{id}に一致する従業員は存在しないため、削除はできません。");
            }
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

    /// <summary>
    /// 指定された従業員Idの従業員を取得する
    /// クライアント側の[削除画面]で利用するため
    /// </summary>
    /// <param name="id">従業員Id</param>
    /// <returns>該当従業員</returns>
    /// <exception cref="NotFoundException">従業員が存在しない場合にスローされる</exception>
    public async Task<Employee> GetEmployeeByIdAsync(string id)
    {
        var result = await _employeeRepository.SelectByIdAsync(id);
        if (result == null)
        {
            throw new NotFoundException(
                $"従業員Id:{id}に一致する従業員は存在しません。");
        }
        return result;
    }
}