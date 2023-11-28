using Application.History;

namespace API.Controllers
{
    public class ProductHistoryController : BaseApiController
    {
       
        [HttpGet("product")]  //api/GetProductUpdateHistoryList
        public async Task<IActionResult> GetProductUpdateHistory()
        {
            return HandleResult(await Mediator.Send(new GetProductUpdateHistory.Query()));
        }

        [HttpGet("serialNumber")] //api/GetSerialNumberHistoryList
        public async Task<IActionResult> GetSerialNumberHistory()
        {
            return HandleResult(await Mediator.Send(new GetSerialNumberHistory.Query()));
        }
        

    }
}
