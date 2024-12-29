using Auth_Application.Interface;
using Auth_Application.Services;
using Auth_Core;
using Auth_Core.Global;
using Auth_Core.MiddleWare;
using Auth_Infrastructure;
using AuthServer.Endpoints.IdentityEndpoint;
using AuthServer.Endpoints.RouteGroup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Auth_Infrastructure.DependencyInjection;
using Auth_Application.DependencyInjection;
using System.Reflection;
using MediatR;

var builder = WebApplication.CreateBuilder(args);


 //config appsettings
    var config = new AppSettingsConfiguration();
      builder.Configuration.Bind(config);
      builder.Services.AddSingleton(config);
builder.Services.AddScoped<SME_Core.Utilities>();
builder.Services.AddSingleton<GlobalInfo>();
 builder.Services.AddInfrastructureServicesInjection();
 builder.Services.AddApplicationServicesInjection();
ContextInjection.AddAuthContextInjection(builder.Services, config);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here"))
		};
	});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	//app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<RequestMiddleWare>();
app.MapGeneralGroup(nameof(IdentityEndpoint), "General")
    .MapIdentityEndpoints();
app.Run();


