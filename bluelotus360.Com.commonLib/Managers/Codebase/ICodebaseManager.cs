using bluelotus360.Com.commonLib.Managers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.Codebase
{
    public interface ICodebaseManager:IManager
    {
        Task<IList<CodeBaseResponse>> GetCodesByConditionCode(CodeBaseResponse record);
        Task<CodeBaseResponse> GetCodesByConditionCodeAndOurCode(CodeBaseResponse record);
    }
}
