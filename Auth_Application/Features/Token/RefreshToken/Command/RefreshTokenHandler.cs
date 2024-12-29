using Auth_Application.Features.Token.AccessToken.Queries;
using Auth_Application.Interface;
using Auth_Application.Models;
using Auth_Application.Validations;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase.Redis;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Features.Token.RefreshToken.Command
{
	internal class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, IdentityOutput>
	{
		private readonly ITokenServices _tokenServices;
		private readonly IUsersCachedManager _usersCachedManager;
		private readonly IRedisCaching _redisCaching;
		public RefreshTokenHandler(ITokenServices tokenServices, IUsersCachedManager usersCachedManager, IRedisCaching redisCaching)
		{
			_tokenServices = tokenServices;
			_usersCachedManager = usersCachedManager;
			_redisCaching = redisCaching;
		}
		public async Task<IdentityOutput> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
		{
			//validate inputs
			ApplicationUser<string> user =  await _usersCachedManager.GetUserAsync(request.LoginType, LoginType.NationalId == request.LoginType ? request.NationalId : request.Email);
			user.IsValidUser();

			var cachedToken = await _redisCaching.GetRefreshTokenAsync(user.Email);
			if(string.IsNullOrEmpty(cachedToken))
				throw new AppException(ExceptionEnum.CachedRefreshTokenWithEmailNotFound);
			if(!cachedToken.Equals(request.RefreshToken))
				throw new AppException(ExceptionEnum.CachedRefreshTokenWithEmailNotFound);

			SessionStatus session = await _redisCaching.GetSessionAsync(user.Email);
			if (session is null)
				throw new AppException(ExceptionEnum.GenricError);

			var token = await _tokenServices.GetAccessToken(user, session.SessionId);
			if (token is null)
				throw new AppException(ExceptionEnum.GenricError);

			return token;
		}
	}
}
