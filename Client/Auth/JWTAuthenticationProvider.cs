using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using BlazorCrud.Client.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorCrud.Client.Auth;

public class JWTAuthenticationProvider: AuthenticationStateProvider, ILoginService
{
    private readonly IJSRuntime js;
    private readonly HttpClient http;
    public static readonly string TokenKey = "TOKENKEY";
    private AuthenticationState Anonymous => new(new ClaimsPrincipal(new ClaimsIdentity()));
    
    public JWTAuthenticationProvider(IJSRuntime js, HttpClient http)
    {
        this.js = js;
        this.http = http; 
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await js.GetFromLocalStorage(TokenKey);

        if (string.IsNullOrEmpty(token))
        {
            return Anonymous;
        }

        return BuildAtuhentaionState(token);
    }
    
    private AuthenticationState BuildAtuhentaionState(string token)
    {
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        
        var claims = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(claims);
        return new AuthenticationState(user);
    }

    public async Task Login(string token)
    {
        await js.SetInLocalStorage(TokenKey, token);
        var authState = BuildAtuhentaionState(token);
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }
    
    public async Task Logout()
    {
        http.DefaultRequestHeaders.Authorization = null;
        await js.RemoveItem(TokenKey);
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));        
    }
    
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

        if (roles != null)
        {
            if (roles.ToString().Trim().StartsWith("["))
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                foreach (var parsedRole in parsedRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
            }

            keyValuePairs.Remove(ClaimTypes.Role);
        }

        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }


}