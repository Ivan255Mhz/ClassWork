using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WebApplication4.Helpers;

public class MyHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public MyHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) 
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if(!Request.Headers.TryGetValue("Authorization", out var auHed))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        if(auHed !="SecretPass")
        {
            return Task.FromResult(AuthenticateResult.Fail("Неверный пропуск"));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name,"Вася"),
            new Claim(ClaimTypes.Role,"Admin"),

            new Claim ("Deportament", "IT")
        };
        var identity = new ClaimsIdentity(claims,Scheme.Name);
        var pri = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(pri, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
        
    }
}
