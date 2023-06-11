using BlueLotus360.Com.Application.Responses.Identity;
using BlueLotus360.Com.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlueLotus360.Com.Application.Interfaces.Chat;
using BlueLotus360.Com.Application.Models.Chat;

namespace BlueLotus360.Com.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId);

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> message);

        Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string userId, string contactId);
    }
}