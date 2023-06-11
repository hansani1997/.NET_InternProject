using System.Collections.Generic;

namespace BlueLotus360.Com.Application.Responses.Identity
{
    public class GetAllUsersResponse
    {
        public IEnumerable<UserResponse> Users { get; set; }
    }
}