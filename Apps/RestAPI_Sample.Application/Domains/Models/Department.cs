using RestAPI_Sample.Application.Exceptions;
namespace RestAPI_Sample.Application.Domains.Models;
/// <summary>
/// 部署を表すドメインオブジェクト
/// </summary>
public sealed class Department : IEquatable<Department>
{
    /// <summary>
    /// 部署Id(UUID形式)
    /// </summary>
    public string Id { get; private set; }
    /// <summary>   
    /// 部署名
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="id">部署Id</param>
    /// <param name="name">部署名</param>
    public Department(string id, string name)
    {
        // データのドメインルール違反チェック
        ValidateDepartment(id, name);
        Id = id;
        Name = name;
    }
    /// <summary>
    /// コンストラクタ:新しい部署Id(UUID)を生成する
    /// </summary>
    /// <param name="name">部署名</param>
    public Department(string name): this(Guid.NewGuid().ToString(), name){}

    /// <summary>
    /// データのドメインルール違反チェック
    /// </summary>
    /// <param name="id">部署Id</param>
    /// <param name="name">部署名</param>
    /// <exception cref="DomainException">引数が無効な場合にスローされる</exception>
    private void ValidateDepartment(string id, string name)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new DomainException($"部署Idは必須です。{nameof(id)}");
        if (!Guid.TryParse(id, out _))
            throw new DomainException($"部署IdはUUID形式でなければなりません。{nameof(id)}");
        ValidateName(name);// 部署名のバリデーション処理（共通）
    }   

    /// <summary>
    /// 部署名のバリデーション処理（共通）
    /// </summary>
    /// <param name="name">部署名</param>
    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException($"部署名は必須です。 {nameof(name)}");
        if (name.Length > 20)
            throw new DomainException($"部署名は20文字以内で指定してください。現在の長さ: {name.Length}");
    }

    /// <summary>
    /// 部署名を変更する
    /// </summary>
    /// <param name="newName">新しい部署名</param>
    /// <exception cref="DomainException">部署名が無効な場合にスローされる</exception>
    public void ChangeName(string newName)
    {
        // 部署名のバリデーション処理（共通）
        ValidateName(newName);
        Name = newName;
    }

    /// <summary>
    /// Idプロパティによる等価性検証
    /// </summary>
    /// <param name="other">別なインスタンス</param>
    /// <returns>true:等価、false:不等価</returns>
    public bool Equals(Department? other)
    {
        if (other == null) return false;
        return Id == other.Id;
    }

    /// <summary>
    /// インスタンスまたは、ハッシュ値を使った等価性検証
    /// </summary>
    public override bool Equals(object? obj) => Equals(obj as Department);
    public override int GetHashCode() => Id!.GetHashCode();
    
    /// <summary>
    /// 文字列表現を出力
    /// </summary>
    public override string ToString()
    {
        return $"部署Id:{Id} , 部署名{Name}";
    }
}