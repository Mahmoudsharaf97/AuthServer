using Auth_Application.Models.Base;
using Auth_Core;
using Newtonsoft.Json;
using SME_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Models.LoginModels
{
	public class VerifyLoginYakeenMobile : BaseLoginModel
	{
		public string Phone { get; set; }
		//[JsonProperty("isYakeenChecked")]
		//public bool IsYakeenChecked { get; set; }


		public override void ValidateModel()
		{
			base.ValidateModel();

			if (string.IsNullOrEmpty(Phone))
				throw new AppException(ExceptionEnum.MobileEmpty);
			if (!Utilities.IsValidPhoneNo(Phone))
				throw new AppException(ExceptionEnum.ErrorPhone);
		}
	}

}
