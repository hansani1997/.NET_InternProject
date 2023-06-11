using BL10.CleanArchitecture.Domain.DTO;
using BlueLotus360.CleanArchitecture.Domain.Entities.AccountProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.AccountProfile
{
    public interface IAccountProfileManager : IManager
    {
        bool IsExceptionthrown();
        Task<IList<AccountProfileResponse>> GetAccountProfileList(AccountProfileRequest request);

        Task<AccountProfileInsertResponse> InsertAccountProfile(AccountProfileInsertRequest request);

        Task<AccountProfileResponse> UpdatedAccountProfile(AccountProfileResponse request);


        //Account Profile Vresion 2

        #region Account Profile Version V2
        Task<IList<AccountProfileResponse>> GetAccountProfileMainGridDetails(BaseServerFilterInfo request);
        Task<AccountProfileResponse> UpdateAccountProfileDetails(AccountProfileResponse request);
        Task<AccountProfileResponse> InsertAccountProfileDetails(AccountProfileResponse request);
        Task<AccountProfileResponse> SelectSignleAccountRecord(AccountProfileResponse request);
        #endregion

    }
}
