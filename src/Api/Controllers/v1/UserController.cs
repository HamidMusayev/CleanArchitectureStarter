using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Queries.GetUserById;
using Contracts.v1.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiController]
[Route("api/v1/users")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateUserCommand(request.Email, request.GivenName, request.FamilyName),
            ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : Problem(title: result.Error!.Code, detail: result.Error.Description, statusCode: 400);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetById(Guid id, CancellationToken ct)
    {
        var user = await mediator.Send(new GetUserByIdQuery(id), ct);
        return user is null ? NotFound() : Ok(user);
    }
}