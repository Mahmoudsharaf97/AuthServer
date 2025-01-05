using Auth_Application.Models;
using Auth_Application.Models.LoginModels.LoginInput;
using Auth_Application.Models.LoginModels.LoginOutput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Interface.Login
{
    public interface ILoginStrategy
    {
		Task<GenericOutput<T>> Execute<T>(LoginInputModel loginInput) where T : class;

	}
}
