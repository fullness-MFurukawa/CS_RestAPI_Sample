using System.Runtime.InteropServices;
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
    void Create(Employee employee);
    /// <summary>
    /// 従業員を変更する
    /// </summary>
    /// <param name="employee"></param>
    void UpdateById(Employee employee);
    /// <summary>
    /// 従業員を削除する
    /// </summary>
    /// <param name="id">従業員Id</param>
    void DeleteById(int id);
    /// <summary>
    /// 従業員をキーワード検索する
    /// </summary>
    /// <param name="keyword">キーワード</param>
    /// <returns>従業員のリスト</returns>
    List<Employee> SelectByNameLike(string keyword);
}