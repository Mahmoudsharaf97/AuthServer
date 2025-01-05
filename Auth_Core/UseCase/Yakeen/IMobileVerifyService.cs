using Auth_Core.Models.Yakeen;
using Auth_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Core.UseCase.Yakeen
{
    public  interface IMobileVerifyService
    {
        Task<YakeenMobileVerificationOutput> YakeenMobileVerificationAsync(long phone, long nationalId, string Language);

    }
}
