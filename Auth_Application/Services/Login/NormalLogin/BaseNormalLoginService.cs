using Auth_Application.Interface.Login;
using Auth_Application.Models.LoginModels;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Services.Login.NormalLogin
{
	public abstract class BaseNormalLoginService : ILoginStrategy
	{
		private readonly IUsersCachedManager _usersCachedManager;

		protected BaseNormalLoginService()
		{
			
		}
		protected BaseNormalLoginService(IUsersCachedManager usersCachedManager)
		{
			_usersCachedManager = usersCachedManager;
		}
		protected abstract void ValidateUser(ApplicationUser<string> user);
		public async void Login(LoginModel model)
		{
			if (model is null)
				throw new AppException(ExceptionEnum.ModelIsEmpty);
			model.ValidateModel();

			// validate captcha
			// check if user locked and count on login try 
			ApplicationUser<string> user = await _usersCachedManager.GetUserByEmailOrNationalIdAsync(model.LoginType, LoginType.NationalId == model.LoginType ? model.NationalId : model.UserName);
			ValidateUser(user); 


		}
	}
}
