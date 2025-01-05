namespace Auth_Core.UseCase
{
    public  interface IApplicationUserManager
    {
        Task<bool> CheckEmailExistAsync(string email);
        Task<bool> UserIsExistAsync(string Id);
        Task<ApplicationUser<string>> GetUserByIdAsync(string Id);
        Task<string> GetUserNameByIdAsync(string Id);
        Task<ApplicationUser<string>> GetUserByEmailAsync(string email);
        Task<ApplicationUser<string>> GetUserByEmail(string email);
        Task<bool> IsBloked(string userId);
        Task<ApplicationUser<string>> GetUserByNationalId(long nationalId);
        Task<bool> CheckNationalIdBelongsForDifferentEmail(long nationalId, string email);
        Task<ApplicationUser<string>> GetUserByPhoneFormate(string formate1, string formate2);

        Task<bool> CheckNationalIdExistAsync(long nin);
    }
}
