using CS341_YMCA.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace CS341_YMCA.Helpers;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly SiteUserRepository SiteUsers;

    private string Email = "";
    private string PasswordHash = "";

    public AuthStateProvider(SiteUserRepository SiteUsers)
    {
        this.SiteUsers = SiteUsers;
    }

    public void LogIn(string Email, string Password)
    {
        this.Email = Email;
        PasswordHash = Password.CalculateSha512();

        var _State = GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(_State);
    }

    public void LogOut()
    {
        Email = "";
        PasswordHash = "";

        var _State = GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(_State);
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var AuthResult = SiteUsers.SiteUser_Authenticate(Email, PasswordHash);
        var Authenticated = AuthResult.Success;

        var Identity = Authenticated ? new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, Email)
            }, "LoggedIn") : new ClaimsIdentity();

        var Result = new AuthenticationState(new ClaimsPrincipal(Identity));
        var ResultTask = Task.FromResult(Result);
        
        return await ResultTask;
    }
}
