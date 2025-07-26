using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using RestAPI_Sample.Application.Usecases.Employees.Interfaces;
using RestAPI_Sample.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
namespace RestAPI_Sample.Presentation.Controllers;
/// <summary>
/// 従業員キーワード検索 API Controller
/// </summary>
[ApiController]
[Route("api/employees/search")]
[SwaggerTag("従業員キーワード検索")]
public class SearchEmployeesByKeywordController : ControllerBase
{
    private readonly ISearchEmployeesByKeywordUseCase _useCase;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="useCase">従業員キーワード検索ユースケースを実現するインターフェイス</param>
    public SearchEmployeesByKeywordController(ISearchEmployeesByKeywordUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>
    /// リクエストハンドラ
    /// </summary>
    /// <param name="keyword">検索キーワード</param>
    /// <returns></returns>
    [Authorize] 
    [HttpGet]
    [SwaggerOperation(Summary = "従業員名に指定キーワードを含むデータを検索します。")]
    public async Task<IActionResult> Get([FromQuery] string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return BadRequest(new { message = "検索キーワードを入力してください。" });
        }
        try
        {
            // ユースケースを実行
            var result = await _useCase.ExecuteAsync(keyword);
            // 結果を返す
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            // 見つからなかった場合は例外メッセージを返す
            return NotFound(new { message = ex.Message });
        }
    }
}