using Auth_Application.Interface;
using Auth_Application.Models;
using Auth_Core.UseCase.Redis;

namespace Auth_Application.Services
{
    public  class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRedisCaching _redisCaching;

        public RefreshTokenService(IRedisCaching redisCaching)
        {
            _redisCaching = redisCaching;
        }

		public Task SaveRefreshToken(string username, string refreshToken)
		{
			throw new NotImplementedException();
		}

		public Task<bool> ValidateRefreshToken(string UserId, string refreshToken)
		{
			throw new NotImplementedException();
		}
	}
}
