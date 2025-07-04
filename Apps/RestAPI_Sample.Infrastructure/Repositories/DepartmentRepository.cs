using Microsoft.EntityFrameworkCore;
using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Domains.Repositories;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Infrastructure.Adapters;
using RestAPI_Sample.Infrastructure.Contexts;

namespace RestAPI_Sample.Infrastructure.Repositories;
/// <summary>
/// ドメインオブジェクト:Department(部署)のCRUD操作インターフェイスの実装
/// </summary>
public class DepartmentRepository : IDepartmentRepositiry
{
    private readonly AppDbContext _context;
    private readonly DepartmentEntityAdapter _adapter;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">アプリケーションDbContext</param>
    /// <param name="adapter">ドメインオブジェクト:DepartmentとDepartmentEntityの相互変換</param>
    public DepartmentRepository(AppDbContext context, DepartmentEntityAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    /// <summary>
    /// すべての部署を取得する
    /// </summary>
    /// <returns>Departmentのリスト</returns>
    public async Task<List<Department>> SelectAllAync()
    {
        try
        {
            var results = await _context.Departments
            .AsNoTracking().ToListAsync();
            var departments = new List<Department>();
            foreach (var result in results)
            {
                var department = await _adapter.RestoreAsync(result);
                departments.Add(department);
            }
            return departments;
        }
        catch (Exception ex)
        {
             // 例外が発生した場合はInternalExceptionをスローする
            throw new InternalException(
                $"すべての部署取得に失敗しました。", ex);
        }
    }

    /// <summary>
    /// 指定された部署Idの部署を取得する
    /// </summary>
    /// <param name="id">部署Id</param>
    /// <returns>Departmentまたnull</returns>
    public async Task<Department?> SelectByIdAsync(string id)
    {
        try
        {
            var result = await _context.Departments
            .Where(department => department.Uuid == id)
            .SingleOrDefaultAsync();
            if (result == null)
            {
                return null;
            }
            return await _adapter.RestoreAsync(result);
        }
        catch (Exception ex)
        {
            // 例外が発生した場合はInternalExceptionをスローする
            throw new InternalException(
                $"部署の取得に失敗しました。 id={id}", ex);
        }
    }
}