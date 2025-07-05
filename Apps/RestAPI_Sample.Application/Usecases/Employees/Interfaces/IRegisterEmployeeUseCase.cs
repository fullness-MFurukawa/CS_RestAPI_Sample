using RestAPI_Sample.Application.Domains.Models;
namespace RestAPI_Sample.Application.Usecases.Employees.Interfaces;
/// <summary>
/// ユースケース:[従業員を登録する]を実現するインターフェイス
/// </summary>
public interface IRegisterEmployeeUseCase
{
    /// <summary>
    /// すべての部署を取得する
    /// クライアント側の[入力画面]で利用するプルダウンを作成するため
    /// </summary>
    /// <returns>Departmentのリスト</returns>
    Task<List<Department>> GetDepartmentsAsync();
    /// <summary>
    /// 指定された部署Idの部署を取得する
    /// クライアント側の[確認画面]、[完了画面]で利用するため
    /// </summary>
    /// <param name="id">部署Id</param>
    /// <returns>該当部署</returns>
    /// <exception cref="NotFoundException">該当データが存在しない場合にスローされる</exception>
    Task<Department> GetDepartmentByIdAsync(string id);
    /// <summary>
    /// 従業員を登録する
    /// </summary>
    /// <param name="employee">登録対象従業員</param>
    /// <returns></returns>
    /// <exception cref="NotFoundException">所属部署が存在しない場合にスローされる</exception>
    Task RegisterEmployeeAsync(Employee employee);
}