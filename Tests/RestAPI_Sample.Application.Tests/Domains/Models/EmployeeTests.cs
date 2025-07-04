using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Exceptions;

namespace RestAPI_Sample.Application.Tests.Domains.Models;
/// <summary>
/// ドメインオブジェクト:Employee(従業員)の単体テストドライバ
/// </summary>
[TestClass]
public class EmployeeTests
{
    [TestMethod("正しいIdと名前でEmployeeが作成される")]
    public void CreateEmployee_ShouldSucceed()
    {
        var id = Guid.NewGuid().ToString();
        var name = "山田太郎";
        var employee = new Employee(id, name);
        Assert.AreEqual(id, employee.Id);
        Assert.AreEqual(name, employee.Name);
    }

    [TestMethod("名前が空文字ならDomainExceptionがスローされる")]
    public void CreateEmployee_WithEmptyName_ShouldThrowException()
    {
        var id = Guid.NewGuid().ToString();
        var ex = Assert.ThrowsException<DomainException>(() => new Employee(id, ""));
        Assert.AreEqual("従業員名は必須です。 name", ex.Message);
    }

    [TestMethod("UUID形式でないIdはDomainExceptionがスローされる")]
    public void CreateEmployee_WithInvalidId_ShouldThrowException()
    {
        var invalidId = "not-a-uuid";
        var ex = Assert.ThrowsException<DomainException>(() => new Employee(invalidId, "佐藤"));
        Assert.AreEqual("従業員IdはUUID形式でなければなりません。id", ex.Message);
    }

    [TestMethod("ChangeName()メソッドで名前が更新される")]
    public void ChangeName_ShouldSucceed()
    {
        var employee = new Employee("田中一郎");
        employee.ChangeName("田中二郎");
        Assert.AreEqual("田中二郎", employee.Name);
    }

    [TestMethod("ChangeName()メソッドで空文字の名前に変更しようとすると例外がスローされる")]
    public void ChangeName_ShouldThrowException_Case1()
    {
        var employee = new Employee("鈴木一郎");
        var ex = Assert.ThrowsException<DomainException>(() => employee.ChangeName(""));
        Assert.AreEqual("従業員名は必須です。 name", ex.Message);
    }

    [TestMethod("ChangeName()メソッドで21文字以上の名前に変更しようとすると例外がスローされる")]
    public void ChangeName_WithTooLongName_ShouldThrowException()
    {
        var employee = new Employee("佐藤花子");
        var longName = new string('あ', 21);
        var ex = Assert.ThrowsException<DomainException>(() => employee.ChangeName(longName));
        Assert.AreEqual($"従業員名は20文字以内で指定してください。現在の長さ: {longName.Length}", ex.Message);
    }

    [TestMethod("ChangeDepartment()メソッドで部署が設定される")]
    public void ChangeDepartment_ShouldUpdateDepartment()
    {
        var employee = new Employee("田村真理子");
        var department = new Department("経理部");
        employee.ChangeDepartment(department);
        Assert.AreEqual(department, employee.Department);
    }

    [TestMethod("Idが同じEmployeeは等価と判定される")]
    public void Equals_ShouldReturnTrue()
    {
        var id = Guid.NewGuid().ToString();
        var e1 = new Employee(id, "A");
        var e2 = new Employee(id, "B");
        Assert.IsTrue(e1.Equals(e2));
    }
    
    [TestMethod("Idが異なるEmployeeは等価でないと判定される")]
    public void Equals_ShouldReturnFalse()
    {
        var e1 = new Employee("田中真一");
        var e2 = new Employee("山本優");
        Assert.IsFalse(e1.Equals(e2));
    }
}