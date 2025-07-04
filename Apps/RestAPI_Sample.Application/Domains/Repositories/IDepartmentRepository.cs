using RestAPI_Sample.Application.Domains.Models;

namespace RestAPI_Sample.Application.Domains.Repositories;
/// <summary>
/// ドメインオブジェクト:Department(部署)のCRUD操作インターフェイス
/// </summary>
public interface IDepartmentRepositiry
{
    /// <summary>
    /// すべての部署を取得する
    /// </summary>
    /// <returns>Departmentのリスト</returns>
    List<Department> SelectAll();
    /// <summary>
    /// 指定された部署Idの部署を取得する
    /// </summary>
    /// <param name="id">部署Id</param>
    /// <returns>Departmentまたnull</returns>
    /// <exception cref="NotFoundException">該当部署が存在しない場合にスローされる</exception>
    Department? SelectById(int id);
}