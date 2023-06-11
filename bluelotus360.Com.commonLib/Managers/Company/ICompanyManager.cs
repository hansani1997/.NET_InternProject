using bluelotus360.Com.MauiSupports.Models;
using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.Company
{
    public interface ICompanyManager:IManager
    {
        Task<IList<CompanyResponse>> GetUserCompanies();
        Task<ReportCompanyDetailsResponse> GetCompanyDetailsResponse();
        Task UpdateSelectedCompany(CompanyResponse response);

        //Task<ReportCompanyDetailsResponse> GetCompanyDetailsResponse();
    }
}
