namespace Auth_Core.UseCase.Redis
{
    public interface IRedisCaching
    {
        Task<string> GetRefreshTokenAsync(string email);
        Task<bool> SetRefreshTokenAsync(string userId, string tokenValue);
        Task<bool> SetSessionAsync(string userId, SessionStatus session);
        Task<SessionStatus> GetSessionAsync(string email);
        Task<bool> DeleteSessionAsync(string userId);
        bool SetUser(string email, string ninKey, ApplicationUser<string> user);
        //bool SetUserAsync(string email,ApplicationUser<string> user);
		Task<ApplicationUser<string>> GetUserAsync(string email);
    }
}
