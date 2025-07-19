using RestAPI_Sample.Application.Domains.Models;
namespace RestAPI_Sample.Application.Domains.Repositories;
/// <summary>
/// ドメインオブジェクト:User(ユーザー)のCRUD操作インターフェイス
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// ユーザーを保存する
    /// </summary>
    Task SaveAsync(User user);

    /// <summary>
    /// ユーザー名またはメールアドレスが既に存在するか確認する
    /// </summary>
    Task<bool> ExistsByUsernameOrEmailAsync(string username, string email);

    /// <summary>
    /// メールアドレスからユーザーを取得する（ログイン用）
    /// </summary>
    Task<User?> FindByEmailAsync(string email);

    /// <summary>
    /// ユーザーUUIDから取得
    /// </summary>
    Task<User?> FindByIdAsync(string userId);
}