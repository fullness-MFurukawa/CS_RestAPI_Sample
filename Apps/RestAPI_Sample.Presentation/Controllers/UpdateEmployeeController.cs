using Microsoft.AspNetCore.Mvc;
using RestAPI_Sample.Application.Exceptions;
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
    private readonly UpdateEmployeeViewModelAdapter _adapter;
    private readonly IUpdateEmployeeUsecase _usecase;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="useCase">従業員変更ユースケースを実現するインターフェイス</param>
    /// <param name="adapter">IputEmployeeViewModelからEmployeeを復元するAdapter</param>
    public UpdateEmployeeController(
        UpdateEmployeeViewModelAdapter adapter, IUpdateEmployeeUsecase usecase)
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

    /// <summary>
    /// リクエストハンドラ
    /// 指定された従業員Idの従業員を取得する
    /// </summary>
    /// <param name="employeeId">従業員Id</param>
    /// <returns></returns>
    [HttpGet("{employeeId}")]
    [SwaggerOperation(Summary = "指定された従業員Idの従業員を取得します。")]
    public async Task<IActionResult> GetEmployeeIdAsync(string employeeId)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
        {
            return BadRequest(new { message = "従業員Idが指定されていません。" });
        }
        try
        {
            var result = await _usecase.GetEmployeeByIdAsync(employeeId);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut]
    [SwaggerOperation(Summary = "従業員の氏名または所属部署を変更します。")]
    public async Task<IActionResult> UpdateEmployeeAsync([FromBody] UpdateEmployeeViewModel model)
    {
        // バリデーションチェックエラー
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            // InputEmployeeViewModelからEmployee復元する
            var employee = await _adapter.RestoreAsync(model);
            // 職部署を取得する
            var department = await _usecase.GetDepartmentByIdAsync(model.DepartmentId);
            employee.ChangeDepartment(department);
            // 従業員を変更する
            await _usecase.UpdateEmployeeAsync(employee);
            return Ok(new
            {
                message = "従業員データを更新しました。",
                employee = employee  // 変更後のデータを返す
            });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// InputEmployeeViewModelからの部署Id有無チェック要求
    /// </summary>
    /// <param name="departmentId"></param>
    /// <returns></returns> 
    //[AcceptVerbs("Get", "Post")]
    [HttpGet("VerifyDepartmentId")]
    public async Task<IActionResult> VerifyDepartmentId(string departmentId)
    {
        try
        {
            var result = await _usecase.GetDepartmentByIdAsync(departmentId!);
            return new JsonResult(true);
        }
        catch (NotFoundException ex)
        {
            // 見つからなかった場合は例外メッセージを返す
            return new JsonResult(ex.Message);
        }
    }
}