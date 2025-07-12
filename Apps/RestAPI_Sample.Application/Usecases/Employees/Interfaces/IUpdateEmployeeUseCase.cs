using RestAPI_Sample.Application.Domains.Models;

namespace RestAPI_Sample.Application.Usecases.Employees.Interfaces;
/// <summary>
/// ユースケース:[従業員を変更する]を実現するインターフェイス
/// </summary>
public interface IUpdateEmployeeUsecase
{
    /// <summary>
    /// 指定された従業員Idの従業員を取得する
    /// クライアント側の[削除画面]で利用するため
    /// </summary>
    /// <param name="id">従業員Id</param>
    /// <returns>該当従業員</returns>
    /// <exception cref="NotFoundException">従業員が存在しない場合にスローされる</exception>
    Task<Employee> GetEmployeeByIdAsync(string id);

    /// <summary>
    /// 従業員を更新する
    /// </summary>
    /// <param name="employee">更新対象従業員</param>
    /// <exception cref="NotFoundException">従業員が存在しない場合にスローされる</exception>
    /// <exception cref="NotFoundException">部署が存在しない場合にスローされる</exception>
    Task UpdateEmployeeAsync(Employee employee);
}
