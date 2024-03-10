using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;
using TreatsAndTails.Components;
using TreatsAndTails.Components.Services;
using TreatsAndTails.Components.Shared;
using TreatsAndTails.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();
builder.Services.AddMudServices();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<TatContext>();
StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);
builder.Services.AddMudServices();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	// Configure JWT authentication options here
});
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

var defaultConnectionPassword = Environment.GetEnvironmentVariable("DefaultConnectionPassword");

if (!string.IsNullOrEmpty(defaultConnectionPassword))
{
    builder.Configuration["ConnectionStrings:DefaultConnection:Password"] = defaultConnectionPassword;
}

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();

