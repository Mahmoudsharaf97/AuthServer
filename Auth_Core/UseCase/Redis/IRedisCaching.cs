namespace Auth_Core.UseCase.Redis
{
    public interface IRedisCaching
    {
		Task<T> GetAsync<T>(string key);
		Task<bool> DeleteAsync(string key);
		Task<string> GetRefreshTokenAsync(string email);
        Task<bool> SetRefreshTokenAsync(string userId, string tokenValue);
        Task<bool> SetSessionAsync(string userId, SessionStatus session);
        Task<SessionStatus> GetSessionAsync(string email);
        Task<bool> DeleteSessionAsync(string userId);
        bool SetUser(string email, string ninKey, ApplicationUser<string> user);
        //bool SetUserAsync(string email,ApplicationUser<string> user);
		Task<ApplicationUser<string>> GetUserAsync(string email);
		Task<bool> SetAsync<T>(string key, T value);
	}
        Task<bool> SetYakeenRegisterUser(ApplicationUser<string> user);
        Task<ApplicationUser<string>> GetYakeenRegistUser(string userEmail, string userNin);
    }
}
