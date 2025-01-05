using Auth_Application.Models.Base;
using Auth_Core;
using Auth_Core.Enums;
using SME_Core;

namespace Auth_Application.Models.LoginModels.LoginInput
{
	public class VerifyLoginYakeenMobile : BaseLoginModel
	{
		public string Phone { get; set; }
		//[JsonProperty("isYakeenChecked")]
		//public bool IsYakeenChecked { get; set; }


		public override void ValidateModel()
		{
			base.ValidateModel();

			if (LoginMethod.VerifyYakeenMobile != LoginMethod)
				throw new AppException(ExceptionEnum.WrongLoginMethod);
			if (string.IsNullOrEmpty(NationalId))
				throw new AppException(ExceptionEnum.NationalIdEmpty);
			if (string.IsNullOrEmpty(Phone))
				throw new AppException(ExceptionEnum.MobileEmpty);
			if (!Utilities.IsValidPhoneNo(Phone))
				throw new AppException(ExceptionEnum.ErrorPhone);
		}
	}

}
