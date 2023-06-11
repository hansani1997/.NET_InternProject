using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.com.razorComponents.Data;
using BlueLotus360.Com.Application.Responses.Identity;

namespace bluelotus360.com.razorComponents.Services
{
	public interface IUserService
	{
		public Task<TokenResponse> LoginAsync(User user);
		public Task<User> RegisterUserAsync(User user);
		public Task<User> GetUserByAccessTokenAsync(string accessToken);
		public Task<User> RefreshTokenAsync(RefreshRequest refreshRequest);
	}
}
