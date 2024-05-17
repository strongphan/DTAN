using PetAdoption.Api.Data;
using System.Security.Claims;

namespace PetAdoption.Api.Services
{
    public interface ITokenService
    {
        string GenerateJWT(IEnumerable<Claim>? additionalClaims = null);
        string GenerateJWT(User user, IEnumerable<Claim>? additionalClaims = null);
    }
}