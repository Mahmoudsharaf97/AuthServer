using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Models
{
    public  class RefreshToken
    {

            public string Token { get; set; }
            public DateTime Expiration { get; set; }
            public string UserName { get; set; }
        
    }
}
