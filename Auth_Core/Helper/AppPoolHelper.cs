using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Core.Helper
{
	public class AppPoolHelper
	{
		private static string _appPoolName;

		public static string APP_POOL_NAME
		{
			get
			{
				if (string.IsNullOrEmpty(_appPoolName))
				{
					_appPoolName = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);
				}
				return _appPoolName;
			}
		}
	}
}
