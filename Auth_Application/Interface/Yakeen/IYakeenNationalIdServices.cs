
using Auth_Application.Models;

namespace Auth_Application.Interface
{
    public interface IYakeenNationalIdServices
    {
        Task<Tuple<UserDataModel, string, string>> GetUserDataFromYakeen(string nationalId, int birthYear, int birthMonth, string channel, string lang);
    }
       
}
