using Motorcycle.Shared.Dtos;

namespace Motorcycle.Shared.Requests;

public record UserGetAllResponse(List<UserDto> Users);