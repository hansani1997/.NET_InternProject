using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.Com.Application.Requests.Identity;
using BlueLotus360.Com.Application.Responses.Identity;
using BlueLotus360.Com.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.Identity.Authentication
{
	public interface IAuthenticationManager :IManager
	{
		Task<TokenResponse> Login(TokenRequest model);

		Task<CompletedUserAuth> GetUserInfoOffline();

        Task<IResult> Logout();

		Task<string> RefreshToken();

		Task<string> TryRefreshToken();

		Task<string> TryForceRefreshToken();

		Task<ClaimsPrincipal> CurrentUser();

		Task<CompletedUserAuth> GetUserInformation();
	}
}
