namespace Auth_Core.UseCase.Redis
{
    public interface IRedisCaching
    {
        Task<string> GetRefreshTokenAsync(string userId);
        Task<bool> SetRefreshTokenAsync(string userId, string tokenValue);
        Task<bool> SetSessionAsync(string userId, SessionStatus session);
        Task<SessionStatus> GetSessionAsync(string userId);
        Task<bool> DeleteSessionAsync(string userId);
        Task<bool> SetUserAsync(string userId, ApplicationUser<string> user);
        Task<ApplicationUser<string>> GetUserAsync(string email);
    }
}
