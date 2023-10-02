using Microsoft.AspNetCore.Identity;

namespace BitWriters.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
