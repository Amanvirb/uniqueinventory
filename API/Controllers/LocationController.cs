using Application.Locations;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[Authorize(Roles = "SuperAdmin,Admin,Employee")]
public class LocationController : BaseApiController
{
    [AllowAnonymous]
    [HttpPost] 
    public async Task<IActionResult> CreateLocation(AddLocationDto location)
    {
        return HandleResult(await Mediator.Send(new CreateLocation.Command { Location = location }));
    }

    [AllowAnonymous]
    [HttpGet] 
    public async Task<IActionResult> GetLocationList()
    {
        return HandleResult(await Mediator.Send(new LocationList.Query()));
    }

    [AllowAnonymous]
    [HttpGet("{name}")] 
    public async Task<IActionResult> GetLocationDetail(string name)
    {
        return HandleResult(await Mediator.Send(new LocationDetail.Query { Name = name }));
    }

    [AllowAnonymous]
    [HttpPut] 
    public async Task<IActionResult> UpdateLocation(CommonDto location)
    {
        return HandleResult(await Mediator.Send(new EditLocation.Command {Location = location }));
    }

    [HttpDelete("{id}")] 
    public async Task<IActionResult> DeleteLocation(int id)
    {
        return HandleResult(await Mediator.Send(new DeleteLocation.Command { Id = id }));
    }

}
