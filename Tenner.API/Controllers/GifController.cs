using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenner.API.Interfaces;

namespace Tenner.API.Controllers;

[Route("api/gif")]
[ApiController]
public class GifController : ControllerBase
{
    private readonly ICacheService _cache;

    public GifController(ICacheService cache)
    {
        _cache = cache;
    }
}
