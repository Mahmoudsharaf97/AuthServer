using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Core.Enums
{
	public enum ErrorCodes
	{

		Success = 0,
		EmptyInputParamter,
		ServiceDown,
		InvalidCaptcha ,
		ServiceException ,
		OwnerNationalIdAndNationalIdAreEqual ,
		NotFound,
		CanNotCreate,
		CanNotSendSMS,
		ModelBinderError,
		ExceptionError,
		NotAuthorized,
		LoginIncorrectPhoneNumberNotVerifed,
		VerificationFaield,
		unAuthorized,
		InValidResponse,
		NotSuccess,
		NullResult

	}
}
