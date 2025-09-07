namespace RestAPI_Sample.Application.Usecases;
/// <summary>
/// Unit of Workパターンを利用したトランザクション制御インターフェイス
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// トランザクションを開始する
    /// </summary>
    /// <returns></returns>
    Task BeginAsync();
    /// <summary>
    /// トランザクションをコミットする
    /// </summary>
    /// <returns></returns>
    Task CommitAsync();
    /// <summary>
    /// トランザクションをロールバックする
    /// </summary>
    /// <returns></returns>
    Task RollbackAsync();
}