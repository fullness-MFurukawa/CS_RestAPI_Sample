using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Infrastructure.Adapters;
using RestAPI_Sample.Infrastructure.Entitties;
namespace RestAPI_Sample.Infrastructure.Tests.Adapters;
/// <summary>
/// ドメインオブジェクト:EmployeeとEmployeeEntityの相互変換クラスの単体テスト
/// </summary>
[TestClass]
public class EmployeeEntityAdapterTests
{
    [TestMethod("ドメインオブジェクトからエンティティに変換できる")]
    public async Task ConvertAsync_ShouldSucceed()
    {
        var domain = new Employee(Guid.NewGuid().ToString(), "山田太郎");
        domain.ChangeDepartment(new Department(Guid.NewGuid().ToString(), "営業部"));
        var adapter = new EmployeeEntityAdapter();

        var entity = await adapter.ConvertAsync(domain);

        Assert.AreEqual(domain.Id, entity.Uuid);
        Assert.AreEqual(domain.Name, entity.Name);
    }

    [TestMethod("エンティティからドメインに復元できる")]
    public async Task RestoreAsync_ShouldSucceed_WithoutDepartment()
    {
        var employeeEntity = new EmployeeEntity
        {
            Id = 11,
            Uuid = Guid.NewGuid().ToString(),
            Name = "田中一郎",
            Department = null
        };
        var adapter = new EmployeeEntityAdapter();

        var domain = await adapter.RestoreAsync(employeeEntity);

        Assert.AreEqual(employeeEntity.Uuid, domain.Id);
        Assert.AreEqual(employeeEntity.Name, domain.Name);
        Assert.IsNull(domain.Department);
    }

    [TestMethod("ConvertAsync()にnullを渡すとInternalExceptionが発生する")]
    [ExpectedException(typeof(InternalException))]
    public async Task ConvertAsync_ThrowsInternalException_WhenNull()
    {
        var adapter = new EmployeeEntityAdapter();
        await adapter.ConvertAsync(null!);
    }

    [TestMethod("RestoreAsync()にnullを渡すとInternalExceptionが発生する")]
    [ExpectedException(typeof(InternalException))]
    public async Task RestoreAsync_ThrowsInternalException_WhenNull()
    {
        var adapter = new EmployeeEntityAdapter();
        await adapter.RestoreAsync(null!);
    }

}