using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Application.Usecases.Users.interfaces;
using RestAPI_Sample.Presentation.ViewModels;
using Swashbuckle.AspNetCore.Annotations;
namespace RestAPI_Sample.Presentation.Controllers;

/// <summary>
/// ユーザー登録API Controller
/// </summary>
[ApiController]
[Route("api/users/register")]
[SwaggerTag("ユーザー登録")]
public class RegisterUserController : ControllerBase
{
    private readonly IRegisterUserUsecase _usecase;
    private readonly RegisterUserViewModelAdapter _adapter;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="usecase">ユーザー登録ユースケースを実現するインターフェイス</param>
    /// <param name="adapter">RegisterUserModelからUserを復元するAdapter</param>
    public RegisterUserController(
        IRegisterUserUsecase usecase, RegisterUserViewModelAdapter adapter)
    {
        _usecase = usecase;
        _adapter = adapter;
    }


    /// <summary>
    /// ユーザー登録API
    /// </summary>
    /// <param name="viewModel">登録用ViewModel</param>
    /// <returns>登録結果</returns>
    [Authorize] 
    [HttpPost]
    [SwaggerOperation(Summary = "ユーザーを登録します。")]
    public async Task<IActionResult> Register([FromBody] RegisterUserViewModel viewModel)
    {
        // バリデーションチェックエラー?
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            // 重複チェック
            await _usecase.ExistsByUsernameOrEmailAsync(viewModel.Username, viewModel.Email);

            // ViewModel → ドメインモデルへ変換
            var user = await _adapter.RestoreAsync(viewModel);

            // ユーザー登録
            await _usecase.RegisterUserAsync(user);

            return Ok(new { Message = "ユーザー登録が完了しました。" });
        }
        catch (ExistsException ex)
        {
            return Conflict(new { Message = ex.Message });
        }
    }
}