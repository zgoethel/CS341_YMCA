using CS341_YMCA.Controllers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace CS341_YMCA.Data
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly SiteUserController SiteUsers;

        private string Email = "";
        private string PasswordHash = "";

        public AuthStateProvider(SiteUserController SiteUsers)
        {
            this.SiteUsers = SiteUsers;
        }

        public void LogIn(string Email, string Password)
        {
            this.Email = Email;
            this.PasswordHash = Password.CalculateSha512();

            var _State = GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(_State);
        }

        public void LogOut()
        {
            this.Email = "";
            this.PasswordHash = "";

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
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
}
