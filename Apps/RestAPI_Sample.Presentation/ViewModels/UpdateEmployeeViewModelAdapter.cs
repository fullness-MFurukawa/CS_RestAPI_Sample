using RestAPI_Sample.Application.Domains.Adapters;
using RestAPI_Sample.Application.Domains.Models;

namespace RestAPI_Sample.Presentation.ViewModels;

/// <summary>
/// UpdateEmployeeViewModelからドメインモデルEmployeeを復元するAdapter
/// </summary>
public class UpdateEmployeeViewModelAdapter :
IRestorer<Employee, UpdateEmployeeViewModel>
{
    /// <summary>
    /// UpdateEmployeeViewModelからドメインモデルEmployeeを復元する
    /// </summary>
    /// <param name="target">UpdateEmployeeViewModel</param>
    /// <returns>ドメインモデルEmployee</returns>
    public Task<Employee> RestoreAsync(UpdateEmployeeViewModel target)
    {
        var employee = new Employee(target.Id,target.Name);
        return Task.FromResult(employee);
    }
}