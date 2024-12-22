using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Features
{
    public class ResetCommand : IRequest<bool>
    {
        public string Email { get; set; }
    }
}
