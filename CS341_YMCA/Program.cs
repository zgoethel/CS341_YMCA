/**
 * Program configuration and startup, initializes services.
 * @author Zach Goethel
 */

using Microsoft.AspNetCore.Components.Authorization;

using CS341_YMCA.Helpers;
using CS341_YMCA.Services;

var Builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Builder.Services.AddHttpContextAccessor();
Builder.Services.AddRazorPages();
Builder.Services.AddServerSideBlazor();
Builder.Services.AddAuthorizationCore();

Builder.Services.AddTransient<EmailService>();
Builder.Services.AddTransient<DatabaseService>();
Builder.Services.AddTransient<SiteUserRepository>();
Builder.Services.AddTransient<ClassRepository>();
Builder.Services.AddTransient<ClassValidationService>();
Builder.Services.AddTransient<FileStorageService>();
Builder.Services.AddTransient<PrereqValidationService>();

Builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationStateService>();

var App = Builder.Build();

// Configure the HTTP request pipeline.
if (!App.Environment.IsDevelopment())
{
    App.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    App.UseHsts();
}

App.UseHttpsRedirection();
App.UseStaticFiles();
App.UseRouting();
App.MapBlazorHub();
App.MapFallbackToPage("/_Host");
App.MapControllers();
App.MapDefaultControllerRoute();

App.Run();
