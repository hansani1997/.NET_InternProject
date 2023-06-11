using BlueLotus360.Com.Application.Interfaces.Common;
using BlueLotus360.Com.Application.Requests.Identity;
using BlueLotus360.Com.Application.Responses.Identity;
using BlueLotus360.Com.Shared.Wrapper;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Application.Interfaces.Services.Identity
{
    public interface ITokenService : IService
    {
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
    }
}