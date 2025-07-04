using Microsoft.EntityFrameworkCore;
using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Domains.Repositories;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Infrastructure.Adapters;
using RestAPI_Sample.Infrastructure.Contexts;

namespace RestAPI_Sample.Infrastructure.Repositories;
/// <summary>
/// ドメインオブジェクト:Employee(従業員)のCRUD操作インターフェイスの実装
/// </summary>
public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;
    private readonly EmployeeEntityAdapter _adapter;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">アプリケーションDbContext</param>
    /// <param name="adapter">ドメインオブジェクト:EmployeeとEmployeeEntityの相互変換</param>
    public EmployeeRepository(AppDbContext context, EmployeeEntityAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    /// <summary>
    /// 従業員を永続化する
    /// </summary>
    /// <param name="employee">従業員</param>
    public async Task CreateAsync(Employee employee)
    {
        try
        {
            // ドメインオブジェクトをEF CoreのEntityに変換する
            var entity = await _adapter.ConvertAsync(employee);
            // 従業員を追加する
            await _context.Employees.AddAsync(entity);
            // データベースに反映させる
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // 例外が発生した場合はInternalExceptionをスローする
            throw new InternalException(
                "従業員の追加に失敗しました。", ex);
        }
    }
    /// <summary>
    /// 従業員を削除する
    /// </summary>
    /// <param name="id">従業員Id</param>
    public async Task<bool> DeleteByIdAsync(string id)
    {
        try
        {
            // 削除対象を非同期で取得する
            var entity = await _context.Employees
                .Where(e => e.Uuid == id).FirstOrDefaultAsync();
            // 見つからない場合はfalseを返す
            if (entity == null)
            {
                return false;
            }
            // 取得したEntityを削除する
            _context.Employees.Remove(entity); 
            // 削除をデータベースに反映させる
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // 例外が発生した場合はInternalExceptionをスローする
            throw new InternalException(
                "従業員の削除に失敗しました。", ex);
        }
    }
    /// <summary>
    /// 指定された従業員Idで従業員を取得する
    /// </summary>
    /// <param name="id">従業員Id</param>
    /// <returns>従業員</returns>
    public async Task<Employee?> SelectByIdAsync(string id)
    {
        try
        {
            // 従業員Idで従業員を取得する
            var entity = await _context.Employees
                .Where(e => e.Uuid == id).AsNoTracking()
                .FirstOrDefaultAsync();
            // 存在しない場合はnullを返す
            if (entity == null)
            {
                return null;
            }
            // Ef CoreのEntityをドメインオブジェクトに変換して返す
            return await _adapter.RestoreAsync(entity);
        }
        catch (Exception ex)
        {
            // 例外が発生した場合はInternalExceptionをスローする
            throw new InternalException(
                $"従業員の取得に失敗しました。 id={id}", ex);
        }
    }
    /// <summary>
    /// 従業員をキーワード検索する
    /// </summary>
    /// <param name="keyword">キーワード</param>
    /// <returns>従業員のリスト</returns>
    public async Task<List<Employee>?> SelectByNameLikeAsync(string keyword)
    {
        try
        {
            // 指定されたキーワードで従業員を取得する
            var results = await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.Name.Contains(keyword))
                .AsNoTracking().ToListAsync();
            // EF CoreのEntityからドメインオブジェクトを復元する
            var employees = new List<Employee>();
            foreach (var result in results)
            {
                var employee = await _adapter.RestoreAsync(result);
                employees.Add(employee);
            }
            return employees;
        }
        catch (Exception ex)
        {
            // 例外が発生した場合はInternalExceptionをスローする
            throw new InternalException(
                $"従業員の取得に失敗しました。 keyword={keyword}", ex);
        }
    }
    /// <summary>
    /// 従業員を変更する
    /// </summary>
    /// <param name="employee"></param>
    public async Task<bool> UpdateByIdAsync(Employee employee)
    {
        try
        {
            // 従業員Idで従業員を取得する
            var entity = await _context.Employees
                .Where(e => e.Uuid == employee.Id).FirstOrDefaultAsync();
            // 存在しない場合はfalseを返す
            if (entity == null)
            {
                return false;
            }
            // 氏名を変更する
            entity.Name = employee.Name;
            // 従業員データを更新する
            _context.Employees.Update(entity);
            // 変更をデータベースに反映させる
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // 例外が発生した場合はInternalExceptionをスローする
            throw new InternalException(
                $"従業員の変更に失敗しました。 id={employee.Id}", ex);
        }
    }
}