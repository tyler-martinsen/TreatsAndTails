using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;
using TreatsAndTails.Components;
using TreatsAndTails.Components.Services;
using TreatsAndTails.Components.Shared;
using TreatsAndTails.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddMudServices();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<InventoryService>();
builder.Services.AddTransient<TatContext>();
StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);
builder.Services.AddMudServices();
builder.Services.AddAuthentication("Cookies")
	.AddCookie(options =>
	{
		options.Cookie.HttpOnly = true;
		options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
	});
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<ProtectedSessionStorage>();

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

