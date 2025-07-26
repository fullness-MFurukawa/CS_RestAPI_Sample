using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using RestAPI_Sample.Application.Usecases.Users.interfaces;
using RestAPI_Sample.Presentation.ViewModels;
using Swashbuckle.AspNetCore.Annotations;

namespace RestAPI_Sample.Presentation.Controllers;

/// <summary>
/// ユーザー認証(ログイン)用APIコントローラー
/// </summary>
[ApiController]
[Route("api/auth/login")]
[SwaggerTag("ユーザー認証（ログイン）処理")]
public class LoginUserController : ControllerBase
{
    private readonly ILoginUserUsecase _usecase;
    private readonly LoginUserViewModelAdapter _adapter;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="usecase">ユーザー認証ユースケース</param>
    /// <param name="adapter">ViewModelからドメインオブジェクトへの変換アダプター</param>
    public LoginUserController(
        ILoginUserUsecase usecase, LoginUserViewModelAdapter adapter)
    {
        _usecase = usecase;
        _adapter = adapter;
    }

    /// <summary>
    /// ログイン認証処理（ユーザー名またはメールアドレス＋パスワード）
    /// </summary>
    /// <param name="viewModel">ログイン情報</param>
    /// <returns>JWTトークン</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "ユーザーのログイン認証",
        Description = "ユーザー名またはメールアドレスとパスワードでログインを行い、JWTトークンを発行します。")]
    [SwaggerResponse(200, "認証成功（JWTトークン返却）", typeof(TokenResponse))]
    [SwaggerResponse(401, "認証失敗（ユーザーが存在しない、またはパスワード不一致）")]
    public async Task<IActionResult> Login([FromBody] LoginUserViewModel viewModel)
    {
        try
        {
            var loginUser = await _adapter.RestoreAsync(viewModel);
            var token = await _usecase.LoginAsync(loginUser);
            return Ok(new TokenResponse { Token = token });
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}