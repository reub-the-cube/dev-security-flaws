using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Security101.Tests;

public class TestUserAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger, UrlEncoder encoder) : BaseAuthHandler(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var result = CreateSuccessfulAuthenticationResult([]);
        return Task.FromResult(result);
    }
}

public class TestAdminAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger, UrlEncoder encoder) : BaseAuthHandler(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Role, "Admin")
        };

        var result = CreateSuccessfulAuthenticationResult(claims);

        return Task.FromResult(result);
    }
}

public abstract class BaseAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger, UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected static AuthenticateResult CreateSuccessfulAuthenticationResult(IEnumerable<Claim>? claims)
    {
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "AdminScheme");

        return AuthenticateResult.Success(ticket);
    }
}