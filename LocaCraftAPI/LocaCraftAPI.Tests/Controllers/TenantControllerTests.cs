using LocaCraftAPI.Controllers;
using LocaCraftAPI.Models;
using LocaCraftAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LocaCraftAPI.Tests.Controllers;

public class TenantControllerTests
{
    private readonly Mock<ITenantRepository> _repoMock = new();
    private readonly TenantController _controller;

    public TenantControllerTests()
    {
        _controller = new TenantController(_repoMock.Object);
    }

    private static Tenant MakeTenant(int id = 1) => new()
    {
        Id = id,
        LeaseId = 10,
        Name = "Martin",
        Surname = "Paul",
        Address = "2 avenue du Test",
        City = "Lyon",
        PostalCode = "69001",
        Country = "France"
    };

    [Fact]
    public async Task GetAllTenants_ReturnsOkWithList()
    {
        var tenants = new List<Tenant> { MakeTenant() };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tenants);

        var result = await _controller.GetAllTenants();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(tenants, ok.Value);
    }

    [Fact]
    public async Task GetTenantById_Found_ReturnsOk()
    {
        var tenant = MakeTenant();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tenant);

        var result = await _controller.GetTenantById(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(tenant, ok.Value);
    }

    [Fact]
    public async Task GetTenantById_NotFound_ReturnsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Tenant?)null);

        var result = await _controller.GetTenantById(99);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetTenantByLeaseId_Found_ReturnsOk()
    {
        var tenant = MakeTenant();
        _repoMock.Setup(r => r.GetByLeaseIdAsync(10)).ReturnsAsync(tenant);

        var result = await _controller.GetTenantByLeaseId(10);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(tenant, ok.Value);
    }

    [Fact]
    public async Task GetTenantByLeaseId_NotFound_ReturnsNotFound()
    {
        _repoMock.Setup(r => r.GetByLeaseIdAsync(99)).ReturnsAsync((Tenant?)null);

        var result = await _controller.GetTenantByLeaseId(99);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetTenantsByLeaseId_ReturnsOkWithList()
    {
        var tenants = new List<Tenant> { MakeTenant() };
        _repoMock.Setup(r => r.GetAllByLeaseIdAsync(10)).ReturnsAsync(tenants);

        var result = await _controller.GetTenantsByLeaseId(10);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(tenants, ok.Value);
    }

    [Fact]
    public async Task CreateTenant_ReturnsCreated()
    {
        var tenant = MakeTenant();
        _repoMock.Setup(r => r.CreateAsync(tenant)).Returns(Task.CompletedTask);

        var result = await _controller.CreateTenant(tenant);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(tenant, created.Value);
    }

    [Fact]
    public async Task UpdateTenant_IdMismatch_ReturnsBadRequest()
    {
        var result = await _controller.UpdateTenant(1, MakeTenant(2));

        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task UpdateTenant_NotFound_ReturnsNotFound()
    {
        var tenant = MakeTenant();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Tenant?)null);

        var result = await _controller.UpdateTenant(1, tenant);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task UpdateTenant_Ok_ReturnsOkWithTenant()
    {
        var tenant = MakeTenant();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tenant);
        _repoMock.Setup(r => r.UpdateAsync(tenant)).Returns(Task.CompletedTask);

        var result = await _controller.UpdateTenant(1, tenant);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(tenant, ok.Value);
    }

    [Fact]
    public async Task Delete_NotFound_ReturnsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Tenant?)null);

        var result = await _controller.Delete(99);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_Ok_ReturnsNoContent()
    {
        var tenant = MakeTenant();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tenant);
        _repoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }
}
