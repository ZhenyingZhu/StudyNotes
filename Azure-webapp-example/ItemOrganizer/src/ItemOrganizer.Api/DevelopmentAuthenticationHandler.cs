using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ItemOrganizer.Api;

public sealed class DevelopmentAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IConfiguration configuration)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "Development";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var tenantId = configuration["DevelopmentAuthentication:TenantId"];
        var ownerObjectId =
            configuration["DevelopmentAuthentication:OwnerObjectId"];
        if (!Guid.TryParse(tenantId, out _)
            || !Guid.TryParse(ownerObjectId, out _))
        {
            return Task.FromResult(
                AuthenticateResult.Fail(
                    "Development authentication requires valid tenant and owner IDs."));
        }

        var claims = new[]
        {
            new Claim("tid", tenantId),
            new Claim("oid", ownerObjectId),
            new Claim(
                "scp",
                string.Join(
                    ' ',
                    AuthorizationPolicies.Read,
                    AuthorizationPolicies.Write,
                    "ItemOrganizer.Analyze"))
        };
        var identity = new ClaimsIdentity(claims, SchemeName);
        var ticket = new AuthenticationTicket(
            new ClaimsPrincipal(identity),
            SchemeName);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
