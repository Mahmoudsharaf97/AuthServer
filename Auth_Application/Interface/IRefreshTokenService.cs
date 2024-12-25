namespace Auth_Application.Interface
{
    public  interface IRefreshTokenService
    {

      Task SaveRefreshToken(string username, string refreshToken);
      Task<bool> ValidateRefreshToken(string UserId, string refreshToken);
  
    }
}
