using RestAPI_Sample.Application.Domains.Models;
namespace RestAPI_Sample.Application.Usecases.Employees.Interfaces;
/// <summary>
/// ユースケース:[従業員をキーワード検索する]を実現するインターフェイス
/// </summary>
public interface ISearchEmployeesByKeywordUseCase
{
    /// <summary>
    /// ユースケースの実現メソッド
    /// </summary>
    /// <param name="keyword">従業員検索キーワード</param>
    /// <returns>従業員リスト</returns>
    /// <exception cref="NotFoundException">該当データが存在しない場合にスローされる</exception>
    Task<List<Employee>> ExecuteAsync(string keyword);
}