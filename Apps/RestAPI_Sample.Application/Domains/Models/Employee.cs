using RestAPI_Sample.Application.Exceptions;
namespace RestAPI_Sample.Application.Domains.Models;
/// <summary>
/// 従業員を表すドメインオブジェクト
/// </summary>
public class Employee : IEquatable<Employee>
{
    /// <summary>
    /// 従業員Id(UUID形式)
    /// </summary>
    public string Id { get; private set; }
    /// <summary>   
    /// 従業員
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 所属部署を参照
    /// </summary>
    public Department? Department { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="id">従業員Id</param>
    /// <param name="name">従業員名</param>
    public Employee(string id, string name)
    {
        // データのドメインルール違反チェック
        ValidateEmployee(id, name);
        Id = id;
        Name = name;
    }
    /// <summary>
    /// コンストラクタ:新しい従業員Id(UUID)を生成する
    /// </summary>
    /// <param name="name">従業員名</param>
    public Employee(string name) : this(Guid.NewGuid().ToString(), name) { }


    /// <summary>
    /// データのドメインルール違反チェック
    /// </summary>
    /// <param name="id">従業員Id</param>
    /// <param name="name">従業員名</param>
    /// <exception cref="DomainException">引数が無効な場合にスローされる</exception>
    private void ValidateEmployee(string id, string name)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new DomainException($"従業員Idは必須です。{nameof(id)}");
        if (!Guid.TryParse(id, out _))
            throw new DomainException($"従業員IdはUUID形式でなければなりません。{nameof(id)}");
        ValidateName(name);// 従業員名のバリデーション処理（共通）
    }

    /// <summary>
    /// 従業員名のバリデーション処理（共通）
    /// </summary>
    /// <param name="name">従業員名</param>
    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException($"従業員名は必須です。 {nameof(name)}");
        if (name.Length > 20)
            throw new DomainException($"従業員名は20文字以内で指定してください。現在の長さ: {name.Length}");
    }

    /// <summary>
    /// 従業員名を変更する
    /// </summary>
    /// <param name="newName">新しい従業員名</param>
    /// <exception cref="DomainException">従業員名が無効な場合にスローされる</exception>
    public void ChangeName(string newName)
    {
        // 従業員名のバリデーション処理（共通）
        ValidateName(newName);
        Name = newName;
    }

    /// <summary>
    /// 所属部署を変更する
    /// </summary>
    /// <param name="department">所属部署またはnull</param>
    public void ChangeDepartment(Department? department)
    {
        Department = department;
    }

    /// <summary>
    /// Idプロパティによる等価性検証
    /// </summary>
    /// <param name="other">別なインスタンス</param>
    /// <returns>true:等価、false:不等価</returns>
    public bool Equals(Employee? other)
    {
        if (other == null) return false;
        return Id == other.Id;
    }

    /// <summary>
    /// インスタンスまたは、ハッシュ値を使った等価性検証
    /// </summary>
    public override bool Equals(object? obj) => Equals(obj as Employee);
    public override int GetHashCode() => Id!.GetHashCode();
    
    /// <summary>
    /// 文字列表現を出力
    /// </summary>
    public override string ToString()
    {
        return $"従業員Id:{Id} , 従業員名{Name} , 所属部署: {Department}";
    }
}