using RestAPI_Sample.Application.Domains.Adapters;
using RestAPI_Sample.Application.Domains.Models;
namespace RestAPI_Sample.Presentation.ViewModels;
/// <summary>
/// LoginUserViewModelからドメインモデルLoginUserを復元するAdapter
/// </summary>
public class LoginUserViewModelAdapter : IRestorer<LoginUser, LoginUserViewModel>
{
    /// <summary>
    /// LoginUserViewModelからドメインモデルLoginUserを復元する
    /// </summary>
    /// <param name="target">LoginUserViewModel</param>
    /// <returns>ドメインモデルLoginUser</returns>
    public Task<LoginUser> RestoreAsync(LoginUserViewModel target)
    {
        var loginUser = new LoginUser(
            target.UsernameOrEmail.Trim(),
            target.Password
        );
        return Task.FromResult(loginUser);
    }
}