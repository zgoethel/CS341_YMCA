using CS341_YMCA.Data;
using CS341_YMCA.Controllers;
using Microsoft.AspNetCore.Components.Authorization;

var Builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Builder.Services.AddHttpContextAccessor();
Builder.Services.AddRazorPages();
Builder.Services.AddServerSideBlazor();
Builder.Services.AddAuthorizationCore();

Builder.Services.AddTransient<Database>();
Builder.Services.AddTransient<EmailSender>();
Builder.Services.AddTransient<SiteUserController>();
Builder.Services.AddTransient<ClassController>();

Builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

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