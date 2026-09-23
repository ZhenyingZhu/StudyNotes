using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;

namespace ItemOrganizer.Api;

public static class AuthorizationPolicies
{
    public const string Read = "ItemOrganizer.Read";
    public const string Write = "ItemOrganizer.Write";
}

public sealed record CurrentUser(Guid TenantId, Guid OwnerObjectId);

public sealed class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
{
    public CurrentUser GetRequired()
    {
        var principal = httpContextAccessor.HttpContext?.User
            ?? throw new InvalidOperationException("No authenticated user is available.");

        return new(
            ParseClaim(principal, "tid"),
            ParseClaim(principal, "oid"));
    }

    private static Guid ParseClaim(ClaimsPrincipal principal, string claimType)
    {
        var value = principal.FindFirstValue(claimType);
        return Guid.TryParse(value, out var result)
            ? result
            : throw new InvalidOperationException(
                $"The authenticated user is missing a valid {claimType} claim.");
    }
}

public sealed record ScopeAccessRequirement(string Scope) : IAuthorizationRequirement;

public sealed class ScopeAccessHandler(IConfiguration configuration)
    : AuthorizationHandler<ScopeAccessRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ScopeAccessRequirement requirement)
    {
        var allowedTenantValue = configuration["Authentication:AllowedTenantId"];
        if (!Guid.TryParse(allowedTenantValue, out var allowedTenantId))
        {
            return Task.CompletedTask;
        }

        var tenantValue = context.User.FindFirstValue("tid");
        var ownerValue = context.User.FindFirstValue("oid");
        var scopeValue = context.User.FindFirstValue("scp");
        if (Guid.TryParse(tenantValue, out var tenantId)
            && Guid.TryParse(ownerValue, out _)
            && tenantId == allowedTenantId
            && scopeValue?.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Contains(requirement.Scope, StringComparer.Ordinal) == true)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public sealed class ProblemDetailsAuthorizationResultHandler
    : IAuthorizationMiddlewareResultHandler
{
    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        Microsoft.AspNetCore.Authorization.AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Succeeded)
        {
            await next(context);
            return;
        }

        var statusCode = authorizeResult.Challenged
            ? StatusCodes.Status401Unauthorized
            : StatusCodes.Status403Forbidden;
        var title = statusCode == StatusCodes.Status401Unauthorized
            ? "Authentication is required."
            : "The caller is not authorized to access this resource.";

        await Results.Problem(
            statusCode: statusCode,
            title: title,
            type: $"https://httpstatuses.com/{statusCode}",
            extensions: new Dictionary<string, object?>
            {
                ["correlationId"] = context.TraceIdentifier
            }).ExecuteAsync(context);
    }
}
