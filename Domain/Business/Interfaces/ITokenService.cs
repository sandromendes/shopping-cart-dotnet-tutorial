using Domain.Models;
using System.Security.Claims;

namespace Domain.Business.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        ClaimsPrincipal ValidateToken(string token);
    }
}
