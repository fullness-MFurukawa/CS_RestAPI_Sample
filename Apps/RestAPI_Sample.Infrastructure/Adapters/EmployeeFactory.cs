using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Infrastructure.Entitties;
namespace RestAPI_Sample.Infrastructure.Adapters;
/// <summary>
/// 部署、従業員オブジェクトの相互変換Factoryクラス
/// ドメインオブジェクト:DepartmentとDepartmentEntityの相互変換
/// ドメインオブジェクト:EmployeeとEmployeeEntityの相互変換
/// </summary>
public class EmployeeFactory
{
    private readonly DepartmentEntityAdapter _departmentEntityAdapter;
    private readonly EmployeeEntityAdapter _employeeEntityAdapter;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="departmentEntityAdapter">DepartmentとDepartmentEntityの相互変換</param>
    /// <param name="employeeEntityAdapter">EmployeeとEmployeeEntityの相互変換</param> 
    public EmployeeFactory(
        DepartmentEntityAdapter departmentEntityAdapter,
        EmployeeEntityAdapter employeeEntityAdapter)
    {
        _departmentEntityAdapter = departmentEntityAdapter;
        _employeeEntityAdapter = employeeEntityAdapter;
    }

    public async Task<EmployeeEntity> ConvertAsync(Employee domain)
    {
        // EmployeeEntityの生成する
        var entity = await _employeeEntityAdapter.ConvertAsync(domain);
        // Departmentがnullならそのまま返す
        if (domain.Department == null)
            return entity;
        // DepartmentEntityの生成
        var departmentEntity = await _departmentEntityAdapter.ConvertAsync(domain.Department);
        entity.Department = departmentEntity;
        return entity;
    }

    public async Task<List<EmployeeEntity>> ConvertAsync(List<Employee> domains)
    {
        var entities = new List<EmployeeEntity>();
        foreach (var domain in domains)
        {
            var entity = await ConvertAsync(domain);
            entities.Add(entity);
        }
        return entities;
    }

    public async Task<Employee> RestoreAsync(EmployeeEntity target)
    {
        // ドメインオブジェクト:Employeeを復元する
        var domain = await _employeeEntityAdapter.RestoreAsync(target);
        if (target.Department == null)
            return domain;
        // DepartmentEntityがnullでなければDepartmentを復元する
        domain.ChangeDepartment(await _departmentEntityAdapter.RestoreAsync(target.Department));
        return domain;
    }

    public async Task<List<Employee>> RestoreAsync(List<EmployeeEntity> targets)
    {
        var domains = new List<Employee>();
        foreach (var target in targets)
        {
            var domain = await RestoreAsync(target);
            domains.Add(domain);
        }
        return domains;
    }
}