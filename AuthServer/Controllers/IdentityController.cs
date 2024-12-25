using Auth_Application.Attributes;
using Auth_Application.Features;
using Auth_Application.Models;
using CommonServices.Attributes;
using IdentityApplication;
using IdentityApplication.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AuthServer.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        public IMediator _mediator { get; }
        public IdentityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult> GatToken()
        {
            return Ok(await _mediator.Send(new GetAccessTokenQuery()));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AppAuthorize_Any]
        public async Task<ActionResult> Register(RegisterInput input)
        {
            var request = MapperObject.Mapper.Map<RegisterCommand>(input); 
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AppAuthorize_Any]
        public async Task<ActionResult> LogIn(LogInInput input)
        {
           var request = MapperObject.Mapper.Map<LoginQuery>(input); 
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AppAuthorize]
        public async Task<ActionResult> LogOut()
        {

            var request = new LogOutCommand() ;
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ResetPassword(string email)
        {
            var request = new ResetCommand() {Email=email};
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ConfirmResetPassword()
        {
            var request = new LogOutCommand() ;
            return Ok(await _mediator.Send(request));
        }


    }
}
