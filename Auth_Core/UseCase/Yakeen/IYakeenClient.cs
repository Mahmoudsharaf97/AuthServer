using Auth_Core.Models.Yakeen;
using Auth_Core.Models;

namespace Auth_Core.UseCase
{
    public interface IYakeenClient
    {
        Task<YakeenMobileVerificationOutput> YakeenMobileVerificationAsync(long phone, long nationalId, string Language);
        Task<CustomerIdYakeenInfoDto> GetCustomerIdInfo(CustomerYakeenRequestDto request, ServiceRequestLog predefinedLogInfo);
    }
        
}
