using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace WebApplication4.Helpers;

/// <summary>
/// Учебный AuthenticationHandler: пускает по заголовку Authorization: SecretPass.
/// </summary>
public class MyHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public MyHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        if (authorizationHeader != "SecretPass")
        {
            return Task.FromResult(AuthenticateResult.Fail("Неверный пропуск"));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Вася"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim("Department", "IT")
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
