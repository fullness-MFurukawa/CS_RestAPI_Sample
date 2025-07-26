using Microsoft.EntityFrameworkCore;
using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Domains.Repositories;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Infrastructure.Adapters;
using RestAPI_Sample.Infrastructure.Contexts;
namespace RestAPI_Sample.Infrastructure.Repositories;
/// <summary>
/// ドメインオブジェクト:User(ユーザー)のCRUD操作インターフェイスの実装
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly UserEntityAdapter _adapter;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">アプリケーションDbContext</param>
    /// <param name="adapter">ドメインオブジェクト:UserとUserEntityの相互変換</param>
    public UserRepository(AppDbContext context, UserEntityAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    /// <summary>
    /// ユーザー名またはメールアドレスが既に存在するか確認する
    /// </summary>
    public async Task<bool> ExistsByUsernameOrEmailAsync(string username, string email)
    {
        try
        {
            return await _context.Users  // ← DbSet<UserEntity>
            .AnyAsync(u => u.Username == username || u.Email == email);  // u: UserEntity
        }
        catch (Exception ex)
        {
            // 例外が発生した場合はInternalExceptionをスローする
            throw new InternalException(
                $"ユーザー名とメールアドレス存在確認に失敗しました。 username={username} , email={email}", ex);
        }
    }
    /// <summary>
    /// メールアドレスからユーザーを取得する（ログイン用）
    /// </summary>
    public async Task<User?> FindByEmailAsync(string email)
    {
        try
        {
            var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
            return entity != null ? await _adapter.RestoreAsync(entity) : null;
        }
        catch (Exception ex)
        {
            // 例外が発生した場合はInternalExceptionをスローする
            throw new InternalException(
                $"メールアドレスでのユーザー取得に失敗しました。 email={email}", ex);
        }
    }
    /// <summary>
    /// ユーザーUUIDからユーザーを取得する
    /// </summary>
    public async Task<User?> FindByIdAsync(string userId)
    {
        try
        {
            var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserUuid.ToString() == userId);
            return entity != null ? await _adapter.RestoreAsync(entity) : null;
        }
        catch (Exception ex)
        {
            // 例外が発生した場合はInternalExceptionをスローする
            throw new InternalException(
                $"ユーザーIdでのユーザー取得に失敗しました。 userId={userId}", ex);
        }
    }

    /// <summary>
    /// ユーザーを永続化する
    /// </summary>
    public async Task SaveAsync(User user)
    {
        try
        {
            var entity = await _adapter.ConvertAsync(user);
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // 例外が発生した場合はInternalExceptionをスローする
            throw new InternalException(
                $"ユーザー永続化に失敗しました。 user={user}", ex);
        }
    }

    /// <summary>
    /// ユーザー名またはパスワードからユーザーを取得する
    /// </summary>
    /// <param name="usernameOrEmail">ユーザー名またはメールアドレス</param>
    /// <returns></returns>
    public async Task<User?> FindByUsernameOrEmailAsync(string usernameOrEmail)
    {
        var entity = await _context.Users
        .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);

        return entity != null ? await _adapter.RestoreAsync(entity) : null;
    }
}