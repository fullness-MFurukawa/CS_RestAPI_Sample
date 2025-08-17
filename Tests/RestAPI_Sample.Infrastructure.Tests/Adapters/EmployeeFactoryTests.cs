using RestAPI_Sample.Application.Domains.Models;
using RestAPI_Sample.Infrastructure.Adapters;
using RestAPI_Sample.Infrastructure.Entitties;
namespace RestAPI_Sample.Infrastructure.Tests.Adapters;
/// <summary>
/// 部署、従業員オブジェクトの相互変換Factoryクラスの単体テストドライバ
/// </summary>
[TestClass]
public class EmployeeFactoryTests
{
    private static EmployeeFactory? _employeeFactory = null;
    [ClassInitialize]
    public static void ClassInit(TestContext context)
    {
        _employeeFactory = new EmployeeFactory(
            new DepartmentEntityAdapter(),
            new EmployeeEntityAdapter());
    }

    [TestMethod("ドメインオブジェクトからエンティティに変換できる")]
    public async Task ConvertAsync_ShouldSucceed()
    {
        // テストデータの準備
        var domain = new Employee(Guid.NewGuid().ToString(), "山田太郎");
        domain.ChangeDepartment(new Department(Guid.NewGuid().ToString(), "営業部"));
        // テストターゲットの実行
        var entity = await _employeeFactory!.ConvertAsync(domain);
        // 実行結果の検証
        Assert.IsNotNull(domain);
        Assert.AreEqual(domain.Id, entity.Uuid);
        Assert.AreEqual(domain.Name, entity.Name);
        Assert.AreEqual(domain.Department!.Id, entity.Department!.Uuid);
        Assert.AreEqual(domain.Department!.Name, entity.Department!.Name);
    }

    [TestMethod("ドメインオブジェクトのリストからエンティティのリストに変換できる")]
    public async Task ConvertAsync_List_ShouldSucceed()
    {
        // テストデータの準備
        var domains = new List<Employee>();
        var domain = new Employee(Guid.NewGuid().ToString(), "山田太郎");
        domain.ChangeDepartment(new Department(Guid.NewGuid().ToString(), "営業部"));
        domains.Add(domain);
        domain = new Employee(Guid.NewGuid().ToString(), "鈴木次郎");
        domain.ChangeDepartment(new Department(Guid.NewGuid().ToString(), "営業部"));
        domains.Add(domain);
        // テストターゲットの実行
        var entities = await _employeeFactory!.ConvertAsync(domains);
        // 実行結果の検証
        Assert.IsNotNull(entities);
        Assert.AreEqual(2, entities.Count);
        var index = 0;
        foreach (var ety in entities)
        {
            Assert.AreEqual(domains[index].Id, ety.Uuid);
            Assert.AreEqual(domains[index].Name, ety.Name);
            Assert.AreEqual(domains[index].Department!.Id, ety.Department!.Uuid);
            Assert.AreEqual(domains[index].Department!.Name, ety.Department!.Name);
            index++;
        }
    }

    [TestMethod("エンティティからドメインに復元できる")]
    public async Task RestoreAsync_ShouldSucceed()
    {
        // テストデータの準備
        var departmentEntity = new DepartmentEntity
        {
            Id = 1,
            Uuid = Guid.NewGuid().ToString(),
            Name = "開発部"
        };
        var employeeEntity = new EmployeeEntity
        {
            Id = 10,
            Uuid = Guid.NewGuid().ToString(),
            Name = "鈴木花子",
            Department = departmentEntity
        };
        // テストターゲットの実行
        var domain = await _employeeFactory!.RestoreAsync(employeeEntity);
        // 実行結果の検証
        Assert.IsNotNull(domain);
        Assert.AreEqual(employeeEntity.Uuid, domain.Id);
        Assert.AreEqual(employeeEntity.Name, domain.Name);
        Assert.AreEqual(employeeEntity.Department.Uuid, domain.Department!.Id);
        Assert.AreEqual(employeeEntity.Department.Name, domain.Department!.Name);
    }

    [TestMethod("エンティティリストからドメインリストに復元できる")]
    public async Task RestoreAsync_List_ShouldSucceed()
    {
        // テストデータの準備
        var entities = new List<EmployeeEntity>();
        var departmentEntity = new DepartmentEntity
        {
            Id = 1,
            Uuid = Guid.NewGuid().ToString(),
            Name = "開発部"
        };
        var employeeEntity = new EmployeeEntity
        {
            Id = 10,
            Uuid = Guid.NewGuid().ToString(),
            Name = "鈴木花子",
            Department = departmentEntity
        };
        entities.Add(employeeEntity);
        departmentEntity = new DepartmentEntity
        {
            Id = 1,
            Uuid = Guid.NewGuid().ToString(),
            Name = "開発部"
        };
        employeeEntity = new EmployeeEntity
        {
            Id = 10,
            Uuid = Guid.NewGuid().ToString(),
            Name = "山田太郎",
            Department = departmentEntity
        };
        entities.Add(employeeEntity);
        var domains = await _employeeFactory!.RestoreAsync(entities);
        var index = 0;
        foreach (var domain in domains)
        {
            Assert.AreEqual(entities[index].Uuid, domain.Id);
            Assert.AreEqual(entities[index].Name, domain.Name);
            Assert.AreEqual(entities[index].Department!.Uuid, domain.Department!.Id);
            Assert.AreEqual(entities[index].Department!.Name, domain.Department!.Name);
            index++;
        }
    }
}