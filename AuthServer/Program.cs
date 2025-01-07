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
using MassTransit;
using Auth_Core.Consumers;
using Serilog;
using Auth_Core.EventBus;

var builder = WebApplication.CreateBuilder(args);

#region Logging
builder.Host.UseSerilog((ctx, lc) => lc
.WriteTo.Console()
.ReadFrom.Configuration(ctx.Configuration));
#endregion

//config appsettings
var config = new AppSettingsConfiguration();
  builder.Configuration.Bind(config);
  builder.Services.AddSingleton(config);
builder.Services.AddScoped<SME_Core.Utilities>();
builder.Services.AddSingleton<GlobalInfo>();
 builder.Services.AddInfrastructureServicesInjection();
 builder.Services.AddApplicationServicesInjection();
//builder.Services.AddSingleton<IEventBus, EventBus>();

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
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.JwtSecretKey))
		};
	});

#region Mass Transit Configurations

builder.Services.AddMassTransit(configure => 
{
	configure.SetKebabCaseEndpointNameFormatter();

	configure.AddConsumer<IndentityLogConsumer>();

	configure.UsingRabbitMq((busContext, rabbitMqConfigurator) =>
	{
        rabbitMqConfigurator.Host(new Uri(config.MessageBrokerSetting.Host), host =>
		{
			host.Username(config.MessageBrokerSetting.UserName);
			host.Password(config.MessageBrokerSetting.Password);
		});

		rabbitMqConfigurator.ConfigureEndpoints(busContext);
	});
});

#endregion

var app = builder.Build();
//LogMiddleWare._eventBus = app.Services.GetRequiredService<IEventBus>();
//var _eventBus = app.Services.GetRequiredService<IEventBus>();
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
app.UseMiddleware<LogMiddleWare>();
app.MapGeneralGroup(nameof(IdentityEndpoint), "General")
    .MapIdentityEndpoints();
app.Run();


