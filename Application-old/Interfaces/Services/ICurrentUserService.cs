using BlueLotus360.Com.Application.Interfaces.Common;

namespace BlueLotus360.Com.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}