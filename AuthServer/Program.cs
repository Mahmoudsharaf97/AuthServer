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
var builder = WebApplication.CreateBuilder(args);


 //config appsettings
    var config = new AppSettingsConfiguration();
      builder.Configuration.Bind(config);
      builder.Services.AddSingleton(config);
 builder.Services.AddSingleton<GlobalInfo>();
ContextInjection.AddAuthContextInjection(builder.Services, config);
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

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
builder.Services.AddSingleton<IRefreshTokenService, RefreshTokenService>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//});
app.UseMiddleware<RequestMiddleWare>();
app.MapGeneralGroup(nameof(IdentityEndpoint), "General")
    .MapIdentityEndpoints();
app.Run();


