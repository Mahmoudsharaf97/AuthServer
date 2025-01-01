﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Core.Enums
{
	public enum LoginMethod : byte
	{
		Login = 1,
		VerifyYakeenMobile,
		VerifyLoginOTP,
		LoginAccountConfirmation,
	}
}
