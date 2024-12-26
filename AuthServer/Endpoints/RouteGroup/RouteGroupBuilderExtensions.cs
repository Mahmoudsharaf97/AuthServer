using System.Diagnostics.CodeAnalysis;

namespace AuthServer.Endpoints.RouteGroup
{
	public static class RouteGroupBuilderExtensions
	{
		public static RouteGroupBuilder MapGeneralGroup(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string prefix,
		string groupName)
		{
			return builder.MapGroup($"api/{prefix.Replace("Endpoint","")}")
				.WithOpenApi()
				.WithTags(groupName);
				//.RequireAuthorization("admin");
		}
	}
}
