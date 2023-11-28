using Application.Dto;
using Application.Locations;
using Domain;

namespace API.Controllers;
public class LocationController : BaseApiController
{
    [HttpPost] //api//CreateLocation
    public async Task<IActionResult> CreateLocation(AddLocationDto location)
    {
        return HandleResult(await Mediator.Send(new CreateLocation.Command { Location = location }));
    }

    [HttpGet] //api/ GetLocationList
    public async Task<IActionResult> GetLocationList()
    {
        return HandleResult(await Mediator.Send(new LocationList.Query()));
    }
    [HttpGet("{id}/{partNumberName}")] //api/GetProductDetail
    public async Task<IActionResult> GetLocationDetail(int id, string partNumberName)
    {
        return HandleResult(await Mediator.Send(new LocationDetail.Query { Id = id, PartNumberName = partNumberName }));
    }

}
