using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Core.Enums
{
    public enum LoginType : byte
    {
        Email = 1,
        NationalId = 2
    }   
    public enum RegisterType : byte
    {
        VerifyYakeenMobile = 1,
        VerifyYakeenNationalId = 2
    }
}
