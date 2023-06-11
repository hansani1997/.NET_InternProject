using BlueLotus360.Com.Application.Interfaces.Common;
using BlueLotus360.Com.Application.Requests.Identity;
using BlueLotus360.Com.Shared.Wrapper;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Application.Interfaces.Services.Account
{
    public interface IAccountService : IService
    {
        Task<IResult> UpdateProfileAsync(UpdateProfileRequest model, string userId);

        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId);

        Task<IResult<string>> GetProfilePictureAsync(string userId);

        Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId);
    }
}