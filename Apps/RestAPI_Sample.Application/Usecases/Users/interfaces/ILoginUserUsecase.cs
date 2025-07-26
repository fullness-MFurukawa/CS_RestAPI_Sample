using RestAPI_Sample.Application.Domains.Models;
namespace RestAPI_Sample.Application.Usecases.Users.interfaces;
/// <summary>
/// ログイン処理を行うユースケースインターフェイス
/// 入力されたユーザー情報をもとに認証し、認証に成功した場合はJWTトークンを返却する
/// </summary>
public interface ILoginUserUsecase
{
    /// <summary>
    /// ログイン認証を行い、成功した場合はアクセストークンを返す
    /// </summary>
    /// <param name="loginUser">ログイン要求を表すドメインモデル</param>
    /// <returns>認証成功時のJWTトークン（アクセストークン）</returns>
    Task<string> LoginAsync(LoginUser loginUser);
}