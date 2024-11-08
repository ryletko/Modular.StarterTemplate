﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Example.WebApp.WebApi;

public class BaseController: ControllerBase
{
    protected string? GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
