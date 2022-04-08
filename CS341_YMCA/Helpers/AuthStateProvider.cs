using CS341_YMCA.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace CS341_YMCA.Helpers;

/// <summary>
/// Scoped injected object which tracks a specific session's authentication
/// state; establishes claims identities to hook into .NET authentication.
/// </summary>
public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly SiteUserRepository siteUsers;

    /// <summary>
    /// Stored identity name identifying which account is authenticated.
    /// </summary>
    private string email = "";

    /// <summary>
    /// Authentication token created from the plain-text password provided by
    /// the user during login.
    /// </summary>
    private string passwordHash = "";

    public AuthStateProvider(SiteUserRepository siteUsers)
    {
        this.siteUsers = siteUsers;
    }

    /// <summary>
    /// Stores internal authentication state for the specified user; assumes the
    /// provided details are valid. This is not a service for validating creds.
    /// </summary>
    /// <param name="email">Authenticated email (account username).</param>
    /// <param name="password">Plain-text password to hash and store.</param>
    public void LogIn(string email, string password)
    {
        // Store the credentials in the auth state
        this.email = email;
        passwordHash = password.CalculateSha512();

        // Alert the framework that authentication has changed
        var _State = GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(_State);
    }

    public void LogOut()
    {
        // Nullify the stored credentials
        email = "";
        passwordHash = "";

        // Alert the framework that authentication has changed
        var state = GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(state);
    }

    /// <summary>
    /// Attempts to authenticate with the stored credentials in state.
    /// Successful authentication results in a generated .NET identity.
    /// </summary>
    /// <returns></returns>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Attempt to authenticate against the database
        var authResult = siteUsers.SiteUser_Authenticate(email, passwordHash);
        var isAuthenticated = authResult.Success;
        // Set the identity according to whether the login was successful
        var identity = isAuthenticated
            // If successful, provide an identity name
            ? new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, email)
            }, "LoggedIn")
            // Otherwise provide an incomplete identity
            : new ClaimsIdentity();

        var result = new AuthenticationState(new ClaimsPrincipal(identity));
        
        return await Task.FromResult(result);
    }
}
