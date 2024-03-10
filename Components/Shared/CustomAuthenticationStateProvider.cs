using Microsoft.AspNetCore.Components.Authorization;
using TreatsAndTails.Models;

namespace TreatsAndTails.Components.Shared
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
	{
        private AuthenticationState? _authenticationState;

        public void SetAuthenticationState(AuthenticationState authenticationState)
        {
            _authenticationState = authenticationState;
            NotifyAuthenticationStateChanged(Task.FromResult(_authenticationState));
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(_authenticationState ?? new AuthenticationState(new()));
        }
    }

}
