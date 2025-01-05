using Auth_Application.Models.Base;
using Auth_Core;
using Auth_Core.Enums;

namespace Auth_Application.Models.LoginModels
{
	public class NormalLoginModel : BaseLoginModel
	{


		public override void ValidateModel()
		{
			base.ValidateModel();
			// validate captcha
			if (LoginMethod.Login != this.LoginMethod)
				throw new AppException(ExceptionEnum.WrongLoginMethod);
		}
	}
}
