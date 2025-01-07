using Auth_Application.Models.Base;
using Auth_Core;
using Auth_Core.Enums;
namespace Auth_Application.Models.LoginModels.LoginInput
{
	public class LoginConfirmationModel : BaseLoginModel
	{
		public override void ValidateModel()
		{
			base.ValidateModel();

			if (LoginMethod.LoginAccountConfirmation != LoginMethod)
				throw new AppException(ExceptionEnum.WrongLoginMethod);
			if (LoginType.Email == LoginType)
			{
				if (string.IsNullOrEmpty(NationalId))
					throw new AppException(ExceptionEnum.NationalIdEmpty);
				if (!BirthYear.HasValue || BirthYear <= 0)
					throw new AppException(ExceptionEnum.ErrorBirthYear);
				if (!BirthMonth.HasValue || BirthMonth > 12)
					throw new AppException(ExceptionEnum.ErrorBirthYear);
			}
		}
	}
}
