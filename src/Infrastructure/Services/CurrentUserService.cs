using System.Security.Claims;
using Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public sealed class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    public string? UserId => accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    public bool IsAuthenticated => accessor.HttpContext?.User.Identity?.IsAuthenticated is true;
}