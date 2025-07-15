using Microsoft.AspNetCore.Mvc;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Application.Usecases.Employees.Interfaces;
using RestAPI_Sample.Presentation.ViewModels;
using Swashbuckle.AspNetCore.Annotations;

namespace RestAPI_Sample.Presentation.Controllers;
/// <summary>
/// 従業員キーワード検索 API Controller
/// </summary>
[ApiController]
[Route("api/employees/register")]
[SwaggerTag("従業員登録")]
public class RegisterEmployeeController : ControllerBase
{
    private readonly IRegisterEmployeeUseCase _useCase;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="useCase">従業員登録ユースケースを実現するインターフェイス</param>
    public RegisterEmployeeController(IRegisterEmployeeUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>
    /// リクエストハンドラ
    /// </summary>
    /// <returns></returns>
    [HttpGet("departments")]
    [SwaggerOperation(Summary = "すべての部署を取得します。")]
    public async Task<IActionResult> GetDepartments()
    {
        var result = await _useCase.GetDepartmentsAsync();
        return Ok(result);
    }

    /// <summary>
    /// 従業員登録
    /// </summary>
    /// <param name="model">登録する従業員データ</param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerOperation(Summary = "従業員を登録します。")]
    public async Task<IActionResult> Register([FromBody] RegisterEmployeeViewModel model)
    {
        
    }

    /// <summary>
    /// RegisterEmployeeViewModelからの部署Id有無チェック要求
    /// </summary>
    /// <param name="departmentId"></param>
    /// <returns></returns> 
    [AcceptVerbs("Get", "Post")]
    [HttpGet("VerifyDepartmentId")]
    public async Task<IActionResult> VerifyDepartmentId(string departmentId)
    {
        try
        {
            var result = await _useCase.GetDepartmentByIdAsync(departmentId!);
            return new JsonResult(true);
        }
        catch (NotFoundException ex)
        {
            // 見つからなかった場合は例外メッセージを返す
            return new JsonResult(ex.Message);
        }
    }
}