using CS341_YMCA.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace CS341_YMCA.Helpers;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly SiteUserRepository siteUsers;

    private string email = "";
    private string passwordHash = "";

    public AuthStateProvider(SiteUserRepository siteUsers)
    {
        this.siteUsers = siteUsers;
    }

    public void LogIn(string email, string password)
    {
        this.email = email;
        passwordHash = password.CalculateSha512();

        var _State = GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(_State);
    }

    public void LogOut()
    {
        email = "";
        passwordHash = "";

        var state = GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(state);
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authResult = siteUsers.SiteUser_Authenticate(email, passwordHash);
        var isAuthenticated = authResult.Success;

        var identity = isAuthenticated ? new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, email)
            }, "LoggedIn") : new ClaimsIdentity();

        var result = new AuthenticationState(new ClaimsPrincipal(identity));
        
        return await Task.FromResult(result);
    }
}
