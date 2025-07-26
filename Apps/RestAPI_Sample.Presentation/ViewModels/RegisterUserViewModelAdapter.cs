using RestAPI_Sample.Application.Domains.Adapters;
using RestAPI_Sample.Application.Domains.Models;
namespace RestAPI_Sample.Presentation.ViewModels;
/// <summary>
/// RegisterUserViewModelからドメインモデルUserを復元するAdapter
/// </summary>
public class RegisterUserViewModelAdapter : IRestorer<User, RegisterUserViewModel>
{
    /// <summary>
    /// RegisterUserViewModelからドメインモデルUserを復元する
    /// </summary>
    /// <param name="target">RegisterUserViewModel</param>
    /// <returns>ドメインモデルUser</returns>
    public Task<User> RestoreAsync(RegisterUserViewModel target)
    {
        var user = new User(
            target.Username,
            target.Email,
            target.Password);
        return Task.FromResult(user);
    }
}