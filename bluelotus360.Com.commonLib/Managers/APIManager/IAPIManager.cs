using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using bluelotus360.Com.commonLib.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.APIManager
{
    public interface IAPIManager: IManager
    {
        Task<APIInformation> GetAPIInformation(APIRequestParameters parameters);
        Task<APIInformation> GetAPIEndPoints(APIRequestParameters parameters);
        Task<APIInformation> GenerateUberToken(APIRequestParameters request);
        Task<APIInformation> GetUberEndPoints(APIRequestParameters request);
        Task<APIInformation> GetApiDetailsByMerchantID(APIRequestParameters request);
    }
}
