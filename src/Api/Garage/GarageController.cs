using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("garage/automobiles")]
public class GarageController : ControllerBase
{
  private readonly AutomobileService _automobileService;
  private readonly ILogger<GarageController> _logger;

    public GarageController(
      AutomobileService automobileService,
      ILogger<GarageController> logger)
    {
    _automobileService = automobileService;
    _logger = logger;
    }

    [HttpGet()]
    [ProducesResponseType<IEnumerable<Automobile>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Automobile>>> GetAutomobiles([FromQuery] AutomobileQuery automobileQuery, CancellationToken cancellationToken)
    {      
      return Ok(await _automobileService.GetAutomobilesByQuery(automobileQuery, cancellationToken));
    }

    [HttpGet("{id}")]
    [ProducesResponseType<Automobile>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Automobile>> GetAutomobile(Guid id, CancellationToken cancellationToken)
    {
      var automobile = new Automobile{ Id = id, Make = "", Model = "", Name = "", Year = 0, LastServiceDate = null };

      return automobile == null ? NotFound() : Ok(automobile);
    }

    [HttpPost()]
    [ProducesResponseType<Automobile>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Automobile>> AddAutomobile([FromBody] NewAutomobile newAutomobile, CancellationToken cancellationToken)
    {
      var automobile = await _automobileService.AddAutomobile(newAutomobile, cancellationToken);

      if (automobile == null){
        return Conflict("Automobile already exists.");
      }

      var location = Url.Action(nameof(AddAutomobile), $"/{automobile.Id}");

      return Created(location, automobile);
    }
}
