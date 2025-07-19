using RestAPI_Sample.Application.Domains.Adapters;
using RestAPI_Sample.Application.Domains.Models;

namespace RestAPI_Sample.Presentation.ViewModels;

/// <summary>
/// RegisterEmployeeViewModelからドメインモデルEmployeeを復元するAdapter
/// </summary>
public class RegisterEmployeeViewModelAdapter :
IRestorer<Employee, RegisterEmployeeViewModel>
{
    /// <summary>
    /// RegisterEmployeeViewModelからドメインモデルEmployeeを復元する
    /// </summary>
    /// <param name="target">RegisterEmployeeViewModel</param>
    /// <returns>ドメインモデルEmployee</returns>
    public Task<Employee> RestoreAsync(RegisterEmployeeViewModel target)
    {
        var employee = new Employee(target.Name);
        return Task.FromResult(employee);
    }
}