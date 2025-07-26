using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Domains.Repositories;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Application.Security;
using RestAPI_Sample.Application.Usecases.Users.interfaces;
using RestAPI_Sample.Application.Usecases.Users.Utiles;
namespace RestAPI_Sample.Application.Usecases.Users.Interactors;
/// <summary>
/// ユースケース:[ユーザーを登録する]を実現するインターフェイスの実装
/// </summary>
public class RegisterUserInteractor : IRegisterUserUsecase
{

    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="repository">ドメインオブジェクト:User(ユーザー)のCRUD操作インターフェイス</param>
    /// <param name="unitOfWork">Unit of Workパターンを利用したトランザクション制御インターフェイス</param>
    /// <param name="passwordHasher">パスワードのハッシュ化インターフェイス</param>
    public RegisterUserInteractor(
        IUserRepository repository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// ユーザー名またはメールアドレスが既に存在するか確認する
    /// </summary>
    /// <param name="username">ユーザー名</param>
    /// <param name="email">メールアドレス</param>
    /// <exception cref="ExistsException">データが存在する場合にスローされる</exception>
    public async Task ExistsByUsernameOrEmailAsync(string username, string email)
    {
        var result = await _repository.ExistsByUsernameOrEmailAsync(username, email);
        if (result)
        {
            throw new ExistsException(
                $"ユーザ名:{username}、メールアドレス{email}は既に存在しています。");
        }
    }

    /// <summary>
    /// ユーザーを登録する
    /// </summary>
    /// <param name="user">登録対象ユーザー</param>
    /// <returns></returns>
    public async Task RegisterUserAsync(User user)
    {
        // パスワードをハッシュ化して設定する
        var hashedPassword = _passwordHasher.Hash(user.PasswordHash);
        user.ChangePassword(hashedPassword);
        // トランザクションを開始する
        await _unitOfWork.BeginAsync();
        try
        {
            // ユーザーを永続化する
            await _repository.SaveAsync(user);
            // トランザクションをコミットする
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            // トランザクションをロールバックする
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}