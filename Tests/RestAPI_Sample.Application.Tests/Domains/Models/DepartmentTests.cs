using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Exceptions;
namespace RestAPI_Sample.Application.Tests.Domains.Models;

/// <summary>
/// ドメインオブジェクト:Department(部署)の単体テストドライバ
/// </summary>
[TestClass]
public class DepartmentTests
{
    [TestMethod("部署Idと部署名が正しい場合、Departmentが生成される")]
    public void CreateDepartment_ShouldSucceed()
    {
        var id = Guid.NewGuid().ToString();
        var name = "営業部";
        var department = new Department(id, name);
        Assert.AreEqual(id, department.Id);
        Assert.AreEqual(name, department.Name);
    }

    [TestMethod("部署名が空文字の場合、例外がスローされる")]
    public void CreateDepartment_ShouldThrowException_Case1()
    {
        var id = Guid.NewGuid().ToString();
        var ex = Assert.ThrowsException<DomainException>(() => new Department(id, ""));
        Assert.AreEqual("部署名は必須です。 name", ex.Message);
    }

    [TestMethod("無効なUUIDを指定した場合、例外がスローされる")]
    public void CreateDepartment_ShouldThrowException_Case2()
    {
        var invalidId = "not-a-uuid";
        var name = "開発部";
        var ex = Assert.ThrowsException<DomainException>(() => new Department(invalidId, name));
        Assert.AreEqual("部署IdはUUID形式でなければなりません。id", ex.Message);
    }

    [TestMethod("ChangeName()メソッドで有効な部署名に変更できる")]
    public void ChangeName__ShouldSucceed()
    {
        var department = new Department("経理部");
        department.ChangeName("人事部");
        Assert.AreEqual("人事部", department.Name);
    }

    [TestMethod("ChangeName()メソッドで空文字を指定すると例外がスローされる")]
    public void ChangeName_ShouldThrowException_Case1()
    {
        var department = new Department("総務部");
        var ex = Assert.ThrowsException<DomainException>(() => department.ChangeName(""));
        Assert.AreEqual("部署名は必須です。 name", ex.Message);
    }

    [TestMethod("ChangeName()メソッドで21文字以上の名前を指定すると例外がスローされる")]
    public void ChangeName_ShouldThrowException_Case2()
    {
        var department = new Department("法務部");
        var tooLongName = new string('A', 21);
        var ex = Assert.ThrowsException<DomainException>(() => department.ChangeName(tooLongName));
        Assert.AreEqual("部署名は20文字以内で指定してください。現在の長さ: 21", ex.Message);
    }

    [TestMethod("同じIdを持つDepartment同士は等価である")]
    public void Equals_ShouldBeEqual()
    {
        var id = Guid.NewGuid().ToString();
        var dept1 = new Department(id, "営業部");
        var dept2 = new Department(id, "開発部");
        Assert.IsTrue(dept1.Equals(dept2));
    }

    [TestMethod("異なるIdを持つDepartment同士は等価でない")]
    public void Equals_WithDifferentId_ShouldNotBeEqual()
    {
        var dept1 = new Department("営業部");
        var dept2 = new Department("開発部");
        Assert.IsFalse(dept1.Equals(dept2));
    }
}