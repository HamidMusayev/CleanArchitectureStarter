using Api.Extensions;
using Application.Features.Auth.Commands.Login;
using Contracts.v1.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiController]
[Route("api/v1/auth")]
[AllowAnonymous]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken ct)
        => (await mediator.Send(new LoginCommand(request.Email, request.Password), ct)).ToActionResult();
}
