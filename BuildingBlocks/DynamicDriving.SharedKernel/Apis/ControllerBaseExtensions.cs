﻿using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DynamicDriving.SharedKernel.Apis;

public static class ControllerBaseExtensions
{
    public static Guid GetCurrentUserIdBySub(this ControllerBase controllerBase)
    {
        ArgumentNullException.ThrowIfNull(controllerBase);

        var userId = controllerBase.User.FindFirstValue(JwtRegisteredClaimNames.Sub) ??
            throw new InvalidOperationException("Could not retrieve user id");

        return Guid.Parse(userId);
    }
}
