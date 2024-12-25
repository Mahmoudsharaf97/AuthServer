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

    }
}
