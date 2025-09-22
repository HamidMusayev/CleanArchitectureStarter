using AutoMapper;
using Contracts.v1.Users;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponse?>
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;
    public GetUserByIdQueryHandler(IUserRepository repo, IMapper mapper) => (_repo, _mapper) = (repo, mapper);

    public async Task<UserResponse?> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(request.Id, ct);
        return user is null ? null : _mapper.Map<UserResponse>(user);
    }
}