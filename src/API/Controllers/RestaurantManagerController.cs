using Application.Mediator.RestaurantManager;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantManagerController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public RestaurantManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("clientsGroup")]
        public async Task<ActionResult> AddClientGroup(AddClientsGroupRequest request)
        {
            await _mediator.Send(request);

            return Ok();
        }

        [HttpGet("clientsGroups")]
        public async Task<ActionResult> GetClientsGroups()
        {
            var result = await _mediator.Send(new GetClientsGroupsRequest());

            return Ok(result);
        }
    }
}
