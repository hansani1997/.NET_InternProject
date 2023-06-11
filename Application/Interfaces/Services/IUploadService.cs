using BlueLotus360.Com.Application.Requests;

namespace BlueLotus360.Com.Application.Interfaces.Services
{
    public interface IUploadService
    {
        string UploadAsync(UploadRequest request);
    }
}