using RestAPI_Sample.Application.Domains.Adapters;
using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Infrastructure.Entitties;
namespace RestAPI_Sample.Infrastructure.Adapters;
/// <summary>
/// ドメインオブジェクト:DepartmentとDepartmentEntityの相互変換クラス
/// </summary> 
/// <typeparam name="Department">ドメインオブジェクト:Department</typeparam>
/// <typeparam name="DepartmentEntity">EFCore:DepartmentEntity</typeparam>
public class DepartmentEntityAdapter :
IConverter<Department, DepartmentEntity>, IRestorer<Department, DepartmentEntity>
{
    /// <summary>
    /// ドメインオブジェクト:DepartmentをDepartmentEntityに変換する
    /// </summary>
    /// <param name="domain">ドメインオブジェクト:Department</param>
    /// <returns>EFCore:DepartmentEntity</returns>
    public Task<DepartmentEntity> ConvertAsync(Department domain)
    {
        var entity = new DepartmentEntity();
        entity.Uuid = domain.Id;
        entity.Name = domain.Name;
        return Task.FromResult(entity);
    }
    /// <summary>
    /// DepartmentEntityからドメインオブジェクト:Departmentを復元する
    /// </summary>
    /// <param name="target">>EFCore:DepartmentEntity</param>
    /// <returns>ドメインオブジェクト:Department</returns>
    public Task<Department> RestoreAsync(DepartmentEntity target)
    {
        var domain = new Department(target.Uuid, target.Name);
        return Task.FromResult(domain);
    }
}