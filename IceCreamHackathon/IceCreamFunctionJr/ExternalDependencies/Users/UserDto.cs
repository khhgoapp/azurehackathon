using System;

namespace IceCreamFunctionJr.ExternalDependencies.Users
{
    public record UserDto(Guid UserId, string UserName, string FullName);
}