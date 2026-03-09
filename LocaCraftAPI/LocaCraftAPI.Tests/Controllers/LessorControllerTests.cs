using LocaCraftAPI.Controllers;
using LocaCraftAPI.Models;
using LocaCraftAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LocaCraftAPI.Tests.Controllers;

public class LessorControllerTests
{
    private readonly Mock<ILessorRepository> _repoMock = new();
    private readonly LessorController _controller;

    public LessorControllerTests()
    {
        _controller = new LessorController(_repoMock.Object);
    }

    private static Lessor MakeLessor(int id = 1) => new()
    {
        Id = id,
        FirstName = "Jean",
        LastName = "Dupont",
        Address = "1 rue de la Paix",
        City = "Paris",
        PostalCode = "75001",
        Country = "France"
    };

    [Fact]
    public async Task GetAllLessors_ReturnsOkWithList()
    {
        var lessors = new List<Lessor> { MakeLessor() };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(lessors);

        var result = await _controller.GetAllLessors();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(lessors, ok.Value);
    }

    [Fact]
    public async Task GetLessorById_Found_ReturnsOk()
    {
        var lessor = MakeLessor();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(lessor);

        var result = await _controller.GetLessorById(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(lessor, ok.Value);
    }

    [Fact]
    public async Task GetLessorById_NotFound_ReturnsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Lessor?)null);

        var result = await _controller.GetLessorById(99);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateLessor_ReturnsCreated()
    {
        var lessor = MakeLessor();
        _repoMock.Setup(r => r.CreateAsync(lessor)).Returns(Task.CompletedTask);

        var result = await _controller.CreateLessor(lessor);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(lessor, created.Value);
    }

    [Fact]
    public async Task UpdateLessor_IdMismatch_ReturnsBadRequest()
    {
        var result = await _controller.UpdateLessor(1, MakeLessor(2));

        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task UpdateLessor_NotFound_ReturnsNotFound()
    {
        var lessor = MakeLessor();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Lessor?)null);

        var result = await _controller.UpdateLessor(1, lessor);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task UpdateLessor_Ok_ReturnsOkWithLessor()
    {
        var lessor = MakeLessor();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(lessor);
        _repoMock.Setup(r => r.UpdateAsync(lessor)).Returns(Task.CompletedTask);

        var result = await _controller.UpdateLessor(1, lessor);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(lessor, ok.Value);
    }

    [Fact]
    public async Task DeleteLessor_NotFound_ReturnsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Lessor?)null);

        var result = await _controller.DeleteLessor(99);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteLessor_Ok_ReturnsNoContent()
    {
        var lessor = MakeLessor();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(lessor);
        _repoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.DeleteLessor(1);

        Assert.IsType<NoContentResult>(result);
    }
}
