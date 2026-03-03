using LocaCraftAPI.Controllers;
using LocaCraftAPI.Models;
using LocaCraftAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LocaCraftAPI.Tests.Controllers;

public class LeaseControllerTests
{
    private readonly Mock<ILeaseRepository> _repoMock = new();
    private readonly LeaseController _controller;

    public LeaseControllerTests()
    {
        _controller = new LeaseController(_repoMock.Object);
    }

    private static Lease MakeLease(int id = 1) => new()
    {
        Id = id,
        LeaseName = "Test Lease",
        RealEstateAssetId = 10,
        LessorId = 5,
        MonthlyRent = 800,
        StartDate = new DateTime(2024, 1, 1)
    };

    [Fact]
    public async Task GetAllLeases_ReturnsOkWithList()
    {
        var leases = new List<Lease> { MakeLease() };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(leases);

        var result = await _controller.GetAllLeases();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(leases, ok.Value);
    }

    [Fact]
    public async Task GetLeaseById_Found_ReturnsOk()
    {
        var lease = MakeLease();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(lease);

        var result = await _controller.GetLeaseById(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(lease, ok.Value);
    }

    [Fact]
    public async Task GetLeaseById_NotFound_ReturnsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Lease?)null);

        var result = await _controller.GetLeaseById(99);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetLeasesByRealEstateAssetId_ReturnsOkWithList()
    {
        var leases = new List<Lease> { MakeLease() };
        _repoMock.Setup(r => r.GetByRealEstateAssetIdAsync(10)).ReturnsAsync(leases);

        var result = await _controller.GetLeasesByRealEstateAssetId(10);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(leases, ok.Value);
    }

    [Fact]
    public async Task CreateLease_ReturnsCreated()
    {
        var lease = MakeLease();
        _repoMock.Setup(r => r.CreateAsync(lease)).Returns(Task.CompletedTask);

        var result = await _controller.CreateLease(lease);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(lease, created.Value);
    }

    [Fact]
    public async Task UpdateLease_IdMismatch_ReturnsBadRequest()
    {
        var result = await _controller.UpdateLease(1, MakeLease(2));

        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task UpdateLease_NotFound_ReturnsNotFound()
    {
        var lease = MakeLease();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Lease?)null);

        var result = await _controller.UpdateLease(1, lease);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task UpdateLease_Ok_ReturnsOkWithLease()
    {
        var lease = MakeLease();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(lease);
        _repoMock.Setup(r => r.UpdateAsync(lease)).Returns(Task.CompletedTask);

        var result = await _controller.UpdateLease(1, lease);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(lease, ok.Value);
    }

    [Fact]
    public async Task Delete_NotFound_ReturnsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Lease?)null);

        var result = await _controller.Delete(99);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_Ok_ReturnsNoContent()
    {
        var lease = MakeLease();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(lease);
        _repoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }
}
