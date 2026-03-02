using LocaCraftAPI.Controllers;
using LocaCraftAPI.Models;
using LocaCraftAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LocaCraftAPI.Tests.Controllers;

public class RealEstateAssetControllerTests
{
    private readonly Mock<IRealEstateAssetRepository> _repoMock = new();
    private readonly RealEstateAssetController _controller;

    public RealEstateAssetControllerTests()
    {
        _controller = new RealEstateAssetController(_repoMock.Object);
    }

    private static RealEstateAsset MakeAsset(int id = 1) => new()
    {
        Id = id,
        Name = "Test Asset",
        Address = "1 rue de la Paix",
        PostalCode = "75001",
        City = "Paris",
        Country = "France"
    };

    [Fact]
    public async Task GetAllRealEstateAsset_ReturnsOkWithList()
    {
        var assets = new List<RealEstateAsset> { MakeAsset() };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(assets);

        var result = await _controller.GetAllRealEstateAsset();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(assets, ok.Value);
    }

    [Fact]
    public async Task GetRealEstateAssetById_Found_ReturnsOk()
    {
        var asset = MakeAsset();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(asset);

        var result = await _controller.GetRealEstateAssetById(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(asset, ok.Value);
    }

    [Fact]
    public async Task GetRealEstateAssetById_NotFound_ReturnsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((RealEstateAsset?)null);

        var result = await _controller.GetRealEstateAssetById(99);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateRealEstateAsset_ReturnsCreated()
    {
        var asset = MakeAsset();
        _repoMock.Setup(r => r.CreateAsync(asset)).Returns(Task.CompletedTask);

        var result = await _controller.CreateRealEstateAsset(asset);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(asset, created.Value);
    }

    [Fact]
    public async Task UpdateRealEstateAsset_IdMismatch_ReturnsBadRequest()
    {
        var result = await _controller.UpdateRealEstateAsset(1, MakeAsset(2));

        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task UpdateRealEstateAsset_NotFound_ReturnsNotFound()
    {
        var asset = MakeAsset();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((RealEstateAsset?)null);

        var result = await _controller.UpdateRealEstateAsset(1, asset);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task UpdateRealEstateAsset_Ok_ReturnsOkWithAsset()
    {
        var asset = MakeAsset();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(asset);
        _repoMock.Setup(r => r.UpdateAsync(asset)).Returns(Task.CompletedTask);

        var result = await _controller.UpdateRealEstateAsset(1, asset);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(asset, ok.Value);
    }

    [Fact]
    public async Task Delete_NotFound_ReturnsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((RealEstateAsset?)null);

        var result = await _controller.Delete(99);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_Ok_ReturnsNoContent()
    {
        var asset = MakeAsset();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(asset);
        _repoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }
}
