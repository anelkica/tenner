using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections;
using Tenner.API.Controllers;
using Tenner.API.Interfaces;
using Tenner.API.Models;

namespace Tenner.Tests;

public class GifControllerTests
{
    private readonly Mock<ICacheService> _cache = new();
    private readonly Mock<ITenorService> _tenor = new();
    private readonly Mock<ILogger<GifController>> _logger = new();
    private readonly GifController _controller;

    public GifControllerTests()
    {
        _controller = new GifController(_cache.Object, _tenor.Object, _logger.Object);
    }

    [Fact] // if tenor API fails, return 502
    public async Task Search_TenorReturnsNull_Returns502()
    {
        _cache.Setup(x => x.GetAsync<TenorResponse>(It.IsAny<string>()))
              .ReturnsAsync((TenorResponse?)null);
        _tenor.Setup(x => x.SearchAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
              .ReturnsAsync((TenorResponse?)null);

        var result = await _controller.Search("cats", 10);

        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(502, statusResult.StatusCode);
    }

    [Fact]
    public async Task Search_ValidQuery_ReturnsOk()
    {
        TenorResponse tenorResponse = new TenorResponse { Results = [], Next = string.Empty };
        _cache.Setup(x => x.GetAsync<TenorResponse>(It.IsAny<string>()))
              .ReturnsAsync((TenorResponse?)null);
        _tenor.Setup(x => x.SearchAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
              .ReturnsAsync(tenorResponse);

        ActionResult<TenorResponse> result = await _controller.Search("cats", 10);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task Search_EmptyQuery_ReturnsBadRequest()
    {
        ActionResult<TenorResponse> result = await _controller.Search("", 10);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Search_LimitHigh_ReturnsBadRequest()
    {
        ActionResult<TenorResponse> result = await _controller.Search("cats", 600);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Search_LimitLow_ReturnsBadRequest()
    {
        ActionResult<TenorResponse> result = await _controller.Search("cats", 0);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
