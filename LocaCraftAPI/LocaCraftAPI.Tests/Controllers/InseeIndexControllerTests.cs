using LocaCraftAPI.Controllers;
using LocaCraftAPI.Models;
using LocaCraftAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LocaCraftAPI.Tests.Controllers;

public class InseeIndexControllerTests
{
    private readonly Mock<IInseeService> _serviceMock = new();
    private readonly InseeIndexController _controller;

    public InseeIndexControllerTests()
    {
        _controller = new InseeIndexController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetIndex_ServiceReturnsData_ReturnsOk()
    {
        var data = new List<InseeIndexModel>
        {
            new() { Period = "2024-T1", IRL = 141.63 }
        };
        _serviceMock.Setup(s => s.GetIndexAsync(20)).ReturnsAsync(data);

        var result = await _controller.GetIndex(20);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(data, ok.Value);
    }

    [Fact]
    public async Task GetIndex_ServiceThrows_Returns500()
    {
        _serviceMock.Setup(s => s.GetIndexAsync(20))
            .ThrowsAsync(new HttpRequestException("INSEE unavailable"));

        var result = await _controller.GetIndex(20);

        var statusResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusResult.StatusCode);
    }
}
