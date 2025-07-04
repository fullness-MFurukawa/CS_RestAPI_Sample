using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Application.Exceptions;
using RestAPI_Sample.Infrastructure.Adapters;
using RestAPI_Sample.Infrastructure.Entitties;
namespace RestAPI_Sample.Infrastructure.Tests.Adapters;
/// <summary>
/// ドメインオブジェクト:DepartmentとDepartmentEntityの相互変換クラスの単体テストドライバ
/// </summary>
[TestClass]
public class DepartmentEntityAdapterTests
{
    [TestMethod("ドメインオブジェクトからエンティティに変換できる")]
    public async Task ConvertAsync_ShouldSucceed()
    {
        var domain = new Department(Guid.NewGuid().ToString(), "営業部");
        var adapter = new DepartmentEntityAdapter();
        var entity = await adapter.ConvertAsync(domain);
        Assert.AreEqual(domain.Id, entity.Uuid);
        Assert.AreEqual(domain.Name, entity.Name);
    }

    [TestMethod("エンティティからドメインに復元できる")]
    public async Task RestoreAsync_ShouldSucceed()
    {
        var entity = new DepartmentEntity
        {
            Id = 1,
            Uuid = Guid.NewGuid().ToString(),
            Name = "開発部"
        };
        var adapter = new DepartmentEntityAdapter();
        var domain = await adapter.RestoreAsync(entity);
        Assert.AreEqual(entity.Uuid, domain.Id);
        Assert.AreEqual(entity.Name, domain.Name);
    }

    [TestMethod("ConvertAsync()メソッドにnullを渡すとInternalExceptionが発生する")]
    [ExpectedException(typeof(InternalException))]
    public async Task ConvertAsync_ThrowsInternalException()
    {
        var adapter = new DepartmentEntityAdapter();
        await adapter.ConvertAsync(null!);  
    }

    [TestMethod("RestoreAsync()メソッドにnullを渡すとInternalExceptionが発生する")]
    [ExpectedException(typeof(InternalException))]
    public async Task RestoreAsync_ThrowsInternalException()
    {
        var adapter = new DepartmentEntityAdapter();
        await adapter.RestoreAsync(null!);  
    }
}