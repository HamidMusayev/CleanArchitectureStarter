using Application.Abstractions.Persistence;
using Application.Common.Results;
using AutoMapper;
using Contracts.v1.Users;
using Domain.Repositories;
using Domain.Users;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserResponse>>
{
    private readonly IUserRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUserRepository repo, IUnitOfWork uow, IMapper mapper)
        => (_repo, _uow, _mapper) = (repo, uow, mapper);

    public async Task<Result<UserResponse>> Handle(CreateUserCommand cmd, CancellationToken ct)
    {
        var user = User.Create(cmd.Email, cmd.GivenName, cmd.FamilyName);
        _repo.Add(user);
        await _uow.SaveChangesAsync(ct);
        var dto = _mapper.Map<UserResponse>(user);
        return Result<UserResponse>.Success(dto);
    }
}