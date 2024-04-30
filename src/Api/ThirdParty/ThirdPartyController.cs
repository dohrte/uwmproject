using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("thirdparty")]
public class ThirdPartyController : ControllerBase
{
  private readonly PokeApiService _pokeApiService;
  private readonly ILogger<ThirdPartyController> _logger;

  public ThirdPartyController(
    PokeApiService pokeApiService,
    ILogger<ThirdPartyController> logger)
  {
    _pokeApiService = pokeApiService;
    _logger = logger;
  }

  [HttpGet("locations/{locationName}")]
  [ProducesResponseType<Location>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<Location>> GetPokeLocation(string locationName, CancellationToken cancellationToken)
  {
    var location = await _pokeApiService.GetLocation(locationName, cancellationToken);

    return location == null ? NotFound() : Ok(location);
  }
}
