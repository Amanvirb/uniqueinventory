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

    [HttpGet("{name}")] //api/GetProductDetail
    public async Task<IActionResult> GetLocationDetail(string name)
    {
        return HandleResult(await Mediator.Send(new LocationDetail.Query { Name = name }));
    }

    [HttpDelete("{id}")] //api/GetProductDetail
    public async Task<IActionResult> DeleteLocation(int id)
    {
        return HandleResult(await Mediator.Send(new DeleteLocation.Command { Id = id }));
    }

}
