using Auth_Application.Models.Base;
using Auth_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth_Core.Enums;
namespace Auth_Application.Models.LoginModels
{
	public class LoginConfirmationModel : BaseLoginModel
	{
		public override void ValidateModel()
		{
			base.ValidateModel();

			if(LoginMethod.LoginAccountConfirmation != this.LoginMethod)
				throw new AppException(ExceptionEnum.WrongLoginMethod);
			if(LoginType.Email == this.LoginType)
			{
				if (string.IsNullOrEmpty(this.NationalId))
					throw new AppException(ExceptionEnum.NationalIdEmpty);
				if (!this.BirthYear.HasValue || this.BirthYear <= 0)
					throw new AppException(ExceptionEnum.ErrorBirthYear);
				if (!this.BirthMonth.HasValue || this.BirthMonth > 12)
					throw new AppException(ExceptionEnum.ErrorBirthYear);
			}
		}
	}
}
