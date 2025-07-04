using RestAPI_Sample.Application.Domains.Models;

namespace RestAPI_Sample.Application.Domains.Repositories;
/// <summary>
/// ドメインオブジェクト:Employee(従業員)のCRUD操作インターフェイス
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// 従業員を永続化する
    /// </summary>
    /// <param name="employee">従業員</param>
    Task CreateAsync(Employee employee);
    /// <summary>
    /// 従業員を変更する
    /// </summary>
    /// <param name="employee"></param>
    Task<bool> UpdateByIdAsync(Employee employee);
    /// <summary>
    /// 従業員を削除する
    /// </summary>
    /// <param name="id">従業員Id</param>
    Task<bool> DeleteByIdAsync(string id);
    /// <summary>
    /// 従業員をキーワード検索する
    /// </summary>
    /// <param name="keyword">キーワード</param>
    /// <returns>従業員のリスト</returns>
    Task<List<Employee>?> SelectByNameLikeAsync(string keyword);
    /// <summary>
    /// 指定された従業員Idで従業員を取得する
    /// </summary>
    /// <param name="id">従業員Id</param>
    /// <returns>従業員</returns>
    Task<Employee?> SelectByIdAsync(string id);
}