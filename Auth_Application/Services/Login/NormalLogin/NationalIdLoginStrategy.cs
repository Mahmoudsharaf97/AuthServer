using Auth_Application.Models.LoginModels.LoginInput;
using Auth_Application.Validations;
using Auth_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Services.Login.NormalLogin
{
	public class NationalIdLoginStrategy : BaseNormalLoginService
	{
		protected override async Task ValidateUser(ApplicationUser<string> user, NormalLoginModel model)
		{
			user.IsFoundUserByNationalId();
			if (model.NationalId[0]=='1')
			{
				string[] arrDateOfBirthH = user.DateOfBirthH.Split('-');
				string userBirthYear = arrDateOfBirthH[2],userBirthMonth = arrDateOfBirthH[1];           
				if(!model.BirthYear.ToString().Equals(userBirthYear))
					throw new AppException(ExceptionEnum.ErrorUserBirthYear);
				if (!model.BirthMonth.Value.ToString("D2").Equals(userBirthMonth))
					throw new AppException(ExceptionEnum.ErrorBirthMonth);
			}
			else if (model.NationalId[0]=='2')
			{
				int userBirthYear = user.DateOfBirthG.Year, userBirthMonth = user.DateOfBirthG.Month;
				if(userBirthYear != model.BirthYear)
					throw new AppException(ExceptionEnum.ErrorUserBirthYear);
				if(userBirthMonth != model.BirthMonth)
					throw new AppException(ExceptionEnum.ErrorUserBirthYear);
			}
		}
	}
}
