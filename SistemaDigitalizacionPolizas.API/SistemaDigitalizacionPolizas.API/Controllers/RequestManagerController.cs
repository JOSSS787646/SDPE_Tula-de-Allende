using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.CreateRequestManager_Service.Commands.CreateRequestManager;
using SistemaDigitalizacionPolizas.Application.Services.CreateRequestManager_Service.Commands.UpdateRequestManager;
using SistemaDigitalizacionPolizas.Application.Services.CreateRequestManager_Service.Queries.GetAllRequestManagers;

namespace SistemaDigitalizacionPolizas.API.Controllers
{

   // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RequestManagerController: ControllerBase
    {
        private readonly IMediator _mediator;

        public RequestManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateRequestManagerCommand command)
        {
            var id = await _mediator.Send(command);

            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllRequestManagersQuery());

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateRequestManagerCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return Ok("Updated successfully");
        }


    }
}
