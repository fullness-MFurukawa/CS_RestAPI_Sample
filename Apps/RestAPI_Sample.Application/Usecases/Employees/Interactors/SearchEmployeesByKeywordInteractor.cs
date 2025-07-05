using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Domains.Repositories;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Application.Usecases.Employees.Interfaces;
namespace RestAPI_Sample.Application.Usecases.Employees.Interactors;
/// <summary>
/// ユースケース:[従業員をキーワード検索する]を実現するインターフェイスの実装
/// </summary>
public class SearchEmployeesByKeywordInteractor : ISearchEmployeesByKeywordUseCase
{
    private readonly IEmployeeRepository _repository;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="repository">ドメインオブジェクト:Employee(従業員)のCRUD操作インターフェイス</param>
    public SearchEmployeesByKeywordInteractor(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// ユースケースの実現メソッド
    /// </summary>
    /// <param name="keyword">従業員検索キーワード</param>
    /// <returns>従業員リスト</returns>
    /// <exception cref="NotFoundException">該当データが存在しない場合にスローされる</exception>
    public async Task<List<Employee>> ExecuteAsync(string keyword)
    {
        // キーワード検索する
        var result = await _repository.SelectByNameLikeAsync(keyword);
        if (result == null || result.Count == 0)
        {
            throw new NotFoundException(
                $"キーワード:[{keyword}]を含む従業員データは見つかりませんでした。");
        }
        return result;
    }
}