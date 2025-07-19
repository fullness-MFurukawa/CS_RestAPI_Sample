using Microsoft.AspNetCore.Mvc;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Application.Usecases.Employees.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
namespace RestAPI_Sample.Presentation.Controllers;
/// <summary>
/// 従業員削除 API Controller
/// </summary>
[ApiController]
[Route("api/employees/register")]
[SwaggerTag("従業員登録")]
public class DeleteEmployeeController : ControllerBase
{
    private readonly IDeleteEmployeeUsecase _usecase;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="usecase">従業員削除ユースケースを実現するインターフェイス</params>
    public DeleteEmployeeController(IDeleteEmployeeUsecase usecase)
    {
        _usecase = usecase;
    }
    /// <summary>
    /// リクエストハンドラ
    /// 指定された従業員Idの従業員を削除する
    /// </summary>
    /// <param name="employeeId">従業員Id</param>
    /// <returns></returns>
    [HttpDelete("{employeeId}")]
    [SwaggerOperation(Summary = "従業員を削除します。")]
    public async Task<IActionResult> DeleteEmployeeAsync(string employeeId)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
        {
            return BadRequest(new { message = "従業員Idが指定されていません。" });
        }
        try
        {
            await _usecase.DeleteEmployeeAsync(employeeId);
            return Ok(new { message = $"従業員ID {employeeId} を削除しました。" });
        }
        catch (NotFoundException ex)
        {
             return NotFound(new { message = ex.Message });
        }
    }
}