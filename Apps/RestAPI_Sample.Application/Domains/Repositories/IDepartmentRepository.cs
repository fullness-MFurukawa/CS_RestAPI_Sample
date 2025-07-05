using RestAPI_Sample.Application.Domains.Models;
namespace RestAPI_Sample.Application.Domains.Repositories;
/// <summary>
/// ドメインオブジェクト:Department(部署)のCRUD操作インターフェイス
/// </summary>
public interface IDepartmentRepository
{
    /// <summary>
    /// すべての部署を取得する
    /// </summary>
    /// <returns>Departmentのリスト</returns>
    Task<List<Department>> SelectAllAync();
    /// <summary>
    /// 指定された部署Idの部署を取得する
    /// </summary>
    /// <param name="id">部署Id</param>
    /// <returns>Departmentまたnull</returns>
    Task<Department?> SelectByIdAsync(string id);
}