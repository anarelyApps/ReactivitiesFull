using System;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Domain;
using Persistence;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Security;

public class UserAccessor(IHttpContextAccessor httpContextAccessor,AppDbContext context) : IUserAccessor
{
    public async Task<User> GetUserAsync()
    {
        return await context.Users.FindAsync(GetUserId())
        ?? throw new Exception("No user is logged in");
    }

    public string GetUserId()
    {
        return httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
        ??throw new Exception("No user found");
    }

    public async Task<User> GetUserWithPhotosAsync()
    {
        var userId = GetUserId();

        return await context.Users
        .Include(x=> x.Photos)
        .FirstOrDefaultAsync(x=>x.Id == userId)
        ?? throw new UnauthorizedAccessException("No user is logged in");
    }
}
