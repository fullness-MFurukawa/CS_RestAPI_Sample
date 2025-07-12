using Microsoft.AspNetCore.Mvc;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Application.Usecases.Employees.Interfaces;
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

    [HttpGet("department")]
    [SwaggerOperation(Summary = "指定された部署Idの部署を取得します。")]
    public async Task<IActionResult> GetDepartment([FromQuery] string? departmentId)
    {
        if (string.IsNullOrWhiteSpace(departmentId))
        {
            return BadRequest(new { message = "部署Idを入力してください。" });
        }
        try
        {
            var result = await _useCase.GetDepartmentByIdAsync(departmentId!);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            // 見つからなかった場合は例外メッセージを返す
            return NotFound(new { message = ex.Message });
        }
    }
}