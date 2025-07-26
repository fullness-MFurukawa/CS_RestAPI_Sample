using System.Security.Authentication;
using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Domains.Repositories;
using RestAPI_Sample.Application.Security;
using RestAPI_Sample.Application.Usecases.Users.interfaces;
namespace RestAPI_Sample.Application.Usecases.Users.Interactors;
/// <summary>
/// ユースケース：[ユーザーをログイン認証する] を実現するインターフェイスの実装
/// </summary>
public class LoginUserInteractor : ILoginUserUsecase
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenProvider _jwtTokenProvider;

     /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="repository">ユーザー検索用リポジトリ</param>
    /// <param name="passwordHasher">パスワード検証用ハッシュ化サービス</param>
    /// <param name="jwtTokenProvider">JWTトークン生成プロバイダー</param>
    public LoginUserInteractor(
        IUserRepository repository,IPasswordHasher passwordHasher,IJwtTokenProvider jwtTokenProvider)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _jwtTokenProvider = jwtTokenProvider;
    }
    /// <summary>
    /// ログイン認証処理
    /// </summary>
    /// <param name="loginUser">ログイン要求情報</param>
    /// <returns>認証成功時のJWTトークン</returns>
    /// <exception cref="AuthenticationException">認証に失敗した場合にスロー</exception>
    public async Task<string> LoginAsync(LoginUser loginUser)
    {
        // 入力されたユーザー名またはメールアドレスに該当するユーザーを取得
        var user = await _repository.FindByUsernameOrEmailAsync(loginUser.UsernameOrEmail);
        if (user == null)
        {
            throw new AuthenticationException("ユーザーが存在しません。");
        }

        // パスワードを検証する
        var isValid = _passwordHasher.Verify(loginUser.Password, user.PasswordHash);
        if (!isValid)
        {
            throw new AuthenticationException("パスワードが正しくありません。");
        }

        // 認証成功時、JWTトークンを生成して返却
        var token = _jwtTokenProvider.GenerateToken(user.Id, user.Username);
        return token;
    }
}