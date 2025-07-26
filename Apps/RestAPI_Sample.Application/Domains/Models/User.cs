using RestAPI_Sample.Application.Exceptions;

namespace RestAPI_Sample.Application.Domains.Models;

/// <summary>
/// ログインユーザーを表すドメインオブジェクト
/// </summary>
public class User : IEquatable<User>
{
    /// <summary>
    /// ユーザーId（UUID）
    /// </summary>
    public string Id { get; private set; }

    /// <summary>
    /// ユーザー名
    /// </summary>
    public string Username { get; private set; }

    /// <summary>
    /// メールアドレス
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// パスワードのハッシュ
    /// </summary>
    public string PasswordHash { get; private set; }
  
    /// <summary>
    /// コンストラクタ（既存IDあり）
    /// </summary>
    public User(string id, string username, string email, string passwordHash)
    {
        UserValidate(id, username, email, passwordHash);
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }

    /// <summary>
    /// コンストラクタ（新規ユーザー）
    /// </summary>
    public User(string username, string email, string passwordHash)
        : this(Guid.NewGuid().ToString(), username, email, passwordHash) { }

    /// <summary>
    /// 入力値バリデーション
    /// </summary>
    private void UserValidate(
        string id, string username, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out _))
            throw new DomainException("ユーザーIDはUUID形式で指定してください。");
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("ユーザー名は必須です。");
        if (username.Length > 30)
            throw new DomainException("ユーザー名は30文字以内で指定してください。");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            throw new DomainException("メールアドレスの形式が不正です。");
        if (email.Length > 100)
            throw new DomainException("メールアドレスは100文字以内で指定してください。");
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("パスワードハッシュは必須です。");
    }

    /// <summary>
    /// ユーザー名を変更
    /// </summary>
    public void ChangeUsername(string newUsername)
    {
        if (string.IsNullOrWhiteSpace(newUsername))
            throw new DomainException("ユーザー名は必須です。");
        if (newUsername.Length > 30)
            throw new DomainException("ユーザー名は30文字以内で指定してください。");
        Username = newUsername;
    }

    /// <summary>
    /// メールアドレスを変更
    /// </summary>
    public void ChangeEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains("@"))
            throw new DomainException("メールアドレスの形式が不正です。");
        if (newEmail.Length > 100)
            throw new DomainException("メールアドレスは100文字以内で指定してください。");
        Email = newEmail;
    }

    /// <summary>
    /// パスワードハッシュを変更
    /// </summary>
    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("パスワードハッシュは必須です。");
        PasswordHash = newPasswordHash;
    }

    /// <summary>
    /// 同一Idによる等価性検証
    /// </summary>
    public bool Equals(User? other)
    {
        if (other == null) return false;
        return Id == other.Id;
    }

    public override bool Equals(object? obj) => Equals(obj as User);
    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString()
    {
        return $"ユーザーID:{Id}, ユーザー名:{Username}, メールアドレス:{Email}";
    }
}