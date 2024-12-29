using Auth_Application.Features.Token.AccessToken.Queries;
using Auth_Application.Models;
using Auth_Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Features.Token.RefreshToken.Command
{
	public class RefreshTokenCommand : IRequest<IdentityOutput>
	{
		public LoginType LoginType { get; set; }
		public string Email { get; set; }
		public string NationalId { get; set; }
		public string RefreshToken { get; set; }
	}
}
