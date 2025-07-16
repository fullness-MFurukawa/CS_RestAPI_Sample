using Microsoft.AspNetCore.Mvc;
using RestAPI_Sample.Application.Usecases.Employees.Interfaces;
using RestAPI_Sample.Presentation.ViewModels;
using Swashbuckle.AspNetCore.Annotations;

namespace RestAPI_Sample.Presentation.Controllers;
/// <summary>
/// 従業員登録 API Controller
/// </summary>
[ApiController]
[Route("api/employees/update")]
[SwaggerTag("従業員変更")]
public class UpdateEmployeeController : ControllerBase
{
    private readonly InputEmployeeViewModelAdapter _adapter;
    private readonly IUpdateEmployeeUsecase _usecase;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="useCase">従業員変更ユースケースを実現するインターフェイス</param>
    /// <param name="adapter">IputEmployeeViewModelからEmployeeを復元するAdapter</param>
    public UpdateEmployeeController(
        InputEmployeeViewModelAdapter adapter, IUpdateEmployeeUsecase usecase)
    {
        _adapter = adapter;
        _usecase = usecase;
    }
    
    /// <summary>
    /// リクエストハンドラ
    /// </summary>
    /// <returns></returns>
    [HttpGet("departments")]
    [SwaggerOperation(Summary = "すべての部署を取得します。")]
    public async Task<IActionResult> GetDepartments()
    {
        var result = await _usecase.GetDepartmentsAsync();
        return Ok(result);
    }
}