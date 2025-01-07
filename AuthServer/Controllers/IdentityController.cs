//using Auth_Application.Attributes;
//using Auth_Application.Features;
//using Auth_Application.Features.Token.AccessToken.Queries;
//using Auth_Application.Models;
//using CommonServices.Attributes;
//using IdentityApplication;
//using MediatR;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//namespace AuthServer.Controllers
//{
//    [Route("[action]")]
//    [ApiController]
//    public class IdentityController : ControllerBase
//    {
//        public IMediator _mediator { get; }
//        public IdentityController(IMediator mediator)
//        {
//            _mediator = mediator;
//        }

//        [HttpGet]
//		[Route("[action]")]
//		[ProducesResponseType(StatusCodes.Status200OK)]
//        [AllowAnonymous]
//        public async Task<ActionResult> GatToken()
//        {
//            return Ok(await _mediator.Send(new GetAccessTokenQuery()));
//        }

//        [HttpPost]
//		[Route("[action]")]
//		[ProducesResponseType(StatusCodes.Status200OK)]
//        [AppAuthorize_Any]
//        public async Task<ActionResult> Register([FromBody] RegisterInput input)
//        {
//			RegisterCommand request = MapperObject.Mapper.Map<RegisterCommand>(input); 
//            return Ok(await _mediator.Send(request));
//        }

//        [HttpPost]
//		[Route("[action]")]
//		[ProducesResponseType(StatusCodes.Status200OK)]
//        [AppAuthorize_Any]
//        public async Task<ActionResult> LogIn([FromBody] LogInInput input)
//        {
//         //  var request = MapperObject.Mapper.Map<BeginEndLoginQuery>(input); 
//            return Ok(await _mediator.Send(input));
//        }

//        [HttpPost]
//		[Route("[action]")]
//		[ProducesResponseType(StatusCodes.Status200OK)]
//        [AppAuthorize]
//        public async Task<ActionResult> LogOut()
//        {

//            var request = new LogOutCommand() ;
//            return Ok(await _mediator.Send(request));
//        }

//        [HttpPost]
//		[Route("[action]")]
//		[ProducesResponseType(StatusCodes.Status200OK)]
//        public async Task<ActionResult> ResetPassword([FromQuery] string email)
//        {
//            var request = new ResetCommand() {Email=email};
//            return Ok(await _mediator.Send(request));
//        }

//        [HttpPost]
//		[Route("[action]")]
//		[ProducesResponseType(StatusCodes.Status200OK)]
//        public async Task<ActionResult> ConfirmResetPassword()
//        {
//            var request = new LogOutCommand() ;
//            return Ok(await _mediator.Send(request));
//        }


//    }
//}
