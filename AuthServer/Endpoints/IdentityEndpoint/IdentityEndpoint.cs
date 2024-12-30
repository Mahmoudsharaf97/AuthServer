using AuthServer.Endpoints.IdentityEndpoint.EndpointActions;

namespace AuthServer.Endpoints.IdentityEndpoint
{
	public static class IdentityEndpoint
	{
		public static RouteGroupBuilder MapIdentityEndpoints(this RouteGroupBuilder group)
		{
			group.MapPost($"/v1/{nameof(ConfirmResetPassword)}", ConfirmResetPassword.Action)
				.WithName("ConfirmResetPassword")
				.WithSummary("ConfirmResetPassword info");

			group.MapGet($"/v1/{nameof(GatToken)}", GatToken.Action)
				.WithName("GatToken")
				.WithSummary("GatToken info");

			group.MapPost($"/v1/{nameof(RefreshToken)}", RefreshToken.Action)
				.WithName("RefreshToken")
				.WithSummary("RefreshToken info");

			group.MapPost($"/v1/{nameof(LogIn)}", LogIn.Action)
				.WithName("LogIn")
				.WithSummary("LogIn info");
			
			group.MapPost($"/v1/{nameof(LogOut)}", LogOut.Action)
				.WithName("LogOut")
				.WithSummary("LogOut info");
			
			group.MapPost($"/v1/{nameof(Register)}", Register.Action)
				.WithName("Register")
				.WithSummary("Register info");
			
			group.MapPost($"/v1/{nameof(ResetPassword)}", ResetPassword.Action)
				.WithName("ResetPassword")
				.WithSummary("ResetPassword info");		 
			
			group.MapGet($"/v1/{nameof(Captcha)}", Captcha.Action)
				.WithName("Captcha")
				.WithSummary("Captcha info");	

			group.MapPost($"/v1/{nameof(ValidateCaptcha)}", ValidateCaptcha.Action)
				.WithName("ValidateCaptcha")
				.WithSummary("ValidateCaptcha info");


			return group;
		}
	}
}
