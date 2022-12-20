using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorCrud.Client.Auth;

public class AuthStateProviderFalse: AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await Task.Delay(1000);
        // var anonymous = new ClaimsIdentity();
        var claims = new List<Claim>{
            new Claim(ClaimTypes.Name, "Deiby Montoya"),
            new Claim(ClaimTypes.Role, "Admin")};
        var user = new ClaimsIdentity(claims, "Fake authentication type");
        return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user)));

    }

}