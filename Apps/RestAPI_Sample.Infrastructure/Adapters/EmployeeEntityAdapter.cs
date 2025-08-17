using RestAPI_Sample.Application.Domains.Adapters;
using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Infrastructure.Entitties;
namespace RestAPI_Sample.Infrastructure.Adapters;
/// <summary>
/// ドメインオブジェクト:EmployeeとEmployeeEntityの相互変換クラス
/// </summary> 
/// <typeparam name="Employee">ドメインオブジェクト:Employee</typeparam>
/// <typeparam name="EmployeeEntity">EFCore:EmployeeEntity</typeparam>
public class EmployeeEntityAdapter :
IConverter<Employee, EmployeeEntity>, IRestorer<Employee, EmployeeEntity>
{
    /// <summary>
    /// ドメインオブジェクト:EmployeeをEmployeeEntityに変換する
    /// </summary>
    /// <param name="domain">ドメインオブジェクト:Employee</param>
    /// <returns>EFCore:EmployeeEntity</returns>
    public Task<EmployeeEntity> ConvertAsync(Employee domain)
    {
        // 引数domainがnullの場合
        _ = domain ?? throw new InternalException("引数domainがnullです。");
        // EmployeeEntityを生成する
        var entity = new EmployeeEntity();
        entity.Uuid = domain.Id;
        entity.Name = domain.Name;
        return Task.FromResult(entity);
    }
    
    /// <summary>
    /// EmployeeEntityからドメインオブジェクト:Employeeを復元する
    /// </summary>
    /// <param name="target">>EFCore:EmployeeEntity</param>
    /// <returns>ドメインオブジェクト:Employee</returns>
    public Task<Employee> RestoreAsync(EmployeeEntity target)
    {
        // 引数targetがnullの場合
        _ = target ?? throw new InternalException("引数targetがnullです。");
        // ドメインオブジェクト:Employeeを復元する
        var domain = new Employee(target.Uuid, target.Name);
        return Task.FromResult(domain);
    }
}