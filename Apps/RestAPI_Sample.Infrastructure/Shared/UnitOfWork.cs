using Microsoft.EntityFrameworkCore.Storage;
using RestAPI_Sample.Application.Usecases;
using RestAPI_Sample.Infrastructure.Contexts;
namespace RestAPI_Sample.Infrastructure.Shared;
/// <summary>
/// Unit of Workパターンを利用したトランザクション制御インターフェイスの実装
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">アプリケーション用DbContext</param>
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// トランザクションを開始する
    /// </summary>
    /// <returns></returns>
    public async Task BeginAsync()
    {
        // トランザクションがなければ開始する
        if (_transaction == null)
        {
            // トランザクションを開始する
            _transaction = await _context.Database.BeginTransactionAsync();
        }
    }
    /// <summary>
    /// トランザクションをコミットする
    /// </summary>
    /// <returns></returns>
    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            // トランザクションをコミットする
            await _transaction.CommitAsync();
            // トランザクションを破棄する
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
    /// <summary>
    /// トランザクションをロールバックする
    /// </summary>
    /// <returns></returns>
    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            // トランザクションをロールバックする
            await _transaction.RollbackAsync();
            // トランザクションを破棄する
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}