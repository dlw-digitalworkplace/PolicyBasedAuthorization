using DLWR.PolicyBasedAuthorization.NET6.Authorization.Handlers;
using DLWR.PolicyBasedAuthorization.NET6.Authorization.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);


////// INJECT CUSTOM AUTHORIZATION HANDLERS //////
builder.Services.AddScoped<IAuthorizationHandler, MyHandler>();
////// INJECT CUSTOM AUTHORIZATION HANDLERS //////

////// ADD AUTHENTICATION //////
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
////// ADD AUTHENTICATION //////

////// ADD AUTHORIZATION + CUSTOM POLICIES //////
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("My", p => p.AddRequirements(new MyRequirement()));
});
////// ADD AUTHORIZATION + CUSTOM POLICIES //////

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


////// USE AUTHENTICATION, AUTHORIZATION //////
app.UseAuthentication();
app.UseAuthorization();
////// USE AUTHENTICATION, AUTHORIZATION //////

////// MAP CONTROLLERS AFTER USE AUTHORIZATION //////
app.MapControllers();
////// MAP CONTROLLERS AFTER USE AUTHORIZATION //////

app.Run();
