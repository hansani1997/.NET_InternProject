using BlueLotus360.Com.Application.Responses.Identity;
using System.Collections.Generic;

namespace BlueLotus360.Com.Application.Requests.Identity
{
    public class UpdateUserRolesRequest
    {
        public string UserId { get; set; }
        public IList<UserRoleModel> UserRoles { get; set; }
    }
}