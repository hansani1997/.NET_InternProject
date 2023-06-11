using BlueLotus360.Com.Application.Requests.Mail;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}