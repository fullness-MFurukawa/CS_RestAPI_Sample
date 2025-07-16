using RestAPI_Sample.Application.Domains.Adapters;
using RestAPI_Sample.Application.Domains.Models;

namespace RestAPI_Sample.Presentation.ViewModels;

/// <summary>
/// InputEmployeeViewModelからドメインモデルEmployeeを復元するAdapter
/// </summary>
public class InputEmployeeViewModelAdapter :
IRestorer<Employee, InputEmployeeViewModel>
{
    /// <summary>
    /// RegisterEmployeeViewModelからドメインモデルEmployeeを復元する
    /// </summary>
    /// <param name="target">RegisterEmployeeViewModel</param>
    /// <returns>ドメインモデルEmployee</returns>
    public Task<Employee> RestoreAsync(InputEmployeeViewModel target)
    {
        var employee = new Employee(target.Name);
        return Task.FromResult(employee);
    }
}