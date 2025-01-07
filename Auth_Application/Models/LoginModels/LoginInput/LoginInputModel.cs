using Auth_Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Models.LoginModels.LoginInput
{
	public class LoginInputModel
	{
		[Required]
		public LoginMethod LoginMethod { get; set; }
		[Required]
		public LoginType LoginType { get; set; }
		public NormalLoginModel? NormalLoginModel { get; set; }
		public VerifyLoginYakeenMobile? VerifyLoginYakeenMobile { get; set; }
		public LoginConfirmationModel? LoginConfirmationModel { get; set; }
		public VerifyLoginOTPModel? VerifyLoginOTPModel { get; set; }
	}
}
