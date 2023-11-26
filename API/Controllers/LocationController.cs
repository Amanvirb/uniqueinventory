using Application.Locations;
using Domain;

namespace API.Controllers;
public class LocationController : BaseApiController
{
    [HttpPost] //api//CreateLocation
    public async Task<IActionResult> CreateLocation(CommonDto name)
    {
        return HandleResult(await Mediator.Send(new CreateLocation.Command { Location = name }));
    }

    [HttpGet] //api/ GetLocationList
    public async Task<IActionResult> GetLocationList()
    {
        return HandleResult(await Mediator.Send(new LocationList.Query()));
    }
    [HttpGet("id")] //api/GetProductDetail
    public async Task<IActionResult> GetLocationDetail(int Id)
    {
        return HandleResult(await Mediator.Send(new LocationDetail.Query { Id = Id }));
    }

}
