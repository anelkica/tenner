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

    #region search tests

    [Fact]
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

    [Theory]
    [InlineData("", 10)]
    [InlineData(" ", 10)]
    public async Task Search_InvalidQuery_ReturnsBadRequest(string query, int limit)
    {
        var result = await _controller.Search(query, limit);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public async Task Search_InvalidLimit_ReturnsBadRequest(int limit)
    {
        var result = await _controller.Search("cats", limit);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Search_UsesCorrectCacheKey()
    {
        string query = "dogs";
        int limit = 20;
        string pos = "v1";
        string expectedKey = $"search:{query}:{limit}:{pos}";

        await _controller.Search(query, limit, pos);

        _cache.Verify(x => x.GetAsync<TenorResponse>(expectedKey), Times.Once);
    }

    [Fact]
    public async Task Search_CacheHit_ReturnsCachedResult()
    {
        var cachedResponse = new TenorResponse { Results = [], Next = string.Empty };
        _cache.Setup(x => x.GetAsync<TenorResponse>(It.IsAny<string>()))
              .ReturnsAsync(cachedResponse);

        var result = await _controller.Search("cats", 10);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(cachedResponse, ok.Value);
    }

    [Fact]
    public async Task Search_CacheMiss_StoresResultInCache()
    {
        var tenorResponse = new TenorResponse { Results = [], Next = string.Empty };
        _cache.Setup(x => x.GetAsync<TenorResponse>(It.IsAny<string>()))
              .ReturnsAsync((TenorResponse?)null);
        _tenor.Setup(x => x.SearchAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
              .ReturnsAsync(tenorResponse);

        await _controller.Search("cats", 10);

        _cache.Verify(x => x.SetAsync(
            It.IsAny<string>(),
            tenorResponse,
            It.IsAny<TimeSpan?>()), Times.Once);
    }

    #endregion

    #region featured tests

    [Fact]
    public async Task Featured_TenorReturnsNull_Returns502()
    {
        _cache.Setup(x => x.GetAsync<TenorResponse>(It.IsAny<string>()))
              .ReturnsAsync((TenorResponse?)null);
        _tenor.Setup(x => x.GetFeaturedAsync(It.IsAny<int>(), It.IsAny<string>()))
              .ReturnsAsync((TenorResponse?)null);
        var result = await _controller.GetFeatured(10);
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(502, statusResult.StatusCode);
    }

    [Fact]
    public async Task Featured_UsesCorrectCacheKey()
    {
        int limit = 10;
        string? pos = null;
        string expectedKey = $"featured:{limit}:{pos}";

        await _controller.GetFeatured(limit, pos);

        _cache.Verify(x => x.GetAsync<TenorResponse>(expectedKey), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public async Task Featured_InvalidLimit_ReturnsBadRequest(int limit)
    {
        var result = await _controller.GetFeatured(limit);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Featured_CacheHit_ReturnsCachedResult()
    {
        var cachedResponse = new TenorResponse { Results = [], Next = string.Empty };
        _cache.Setup(x => x.GetAsync<TenorResponse>(It.IsAny<string>()))
              .ReturnsAsync(cachedResponse);

        var result = await _controller.GetFeatured(10);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(cachedResponse, ok.Value);
    }

    [Fact]
    public async Task Featured_CacheMiss_StoresResultInCache()
    {
        var tenorResponse = new TenorResponse { Results = [], Next = string.Empty };
        _cache.Setup(x => x.GetAsync<TenorResponse>(It.IsAny<string>()))
              .ReturnsAsync((TenorResponse?)null);
        _tenor.Setup(x => x.GetFeaturedAsync(It.IsAny<int>(), It.IsAny<string>()))
              .ReturnsAsync(tenorResponse);

        await _controller.GetFeatured(10);

        _cache.Verify(x => x.SetAsync(
            It.IsAny<string>(),
            tenorResponse,
            It.IsAny<TimeSpan?>()), Times.Once);
    }

    #endregion

    #region register share tests

    [Fact]
    public async Task RegisterShare_EmptyId_ReturnsBadRequest()
    {
        var result = await _controller.RegisterShare("");
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RegisterShare_ValidId_Returns204()
    {
        _tenor.Setup(x => x.RegisterShareAsync(It.IsAny<string>(), It.IsAny<string>()))
              .Returns(Task.CompletedTask);

        var result = await _controller.RegisterShare("123");

        Assert.IsType<NoContentResult>(result);
    }

    #endregion

    #region autocomplete tests

    [Fact]
    public async Task Autocomplete_TenorReturnsNull_Returns502()
    {
        _tenor.Setup(x => x.GetAutocompleteAsync(It.IsAny<string>(), It.IsAny<int>()))
              .ReturnsAsync((TenorSuggestionsResponse?)null);
        var result = await _controller.Autocomplete("ca", 3);
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(502, statusResult.StatusCode);
    }

    [Theory]
    [InlineData("", 3)]
    [InlineData(" ", 3)]
    public async Task Autocomplete_InvalidQuery_ReturnsBadRequest(string query, int limit)
    {
        var result = await _controller.Autocomplete(query, limit);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public async Task Autocomplete_InvalidLimit_ReturnsBadRequest(int limit)
    {
        var result = await _controller.Autocomplete("ca", limit);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    #endregion
}
