using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.Entities.AccountProfile
{
    public class AccountProfileRequest
    {
        public long ElementKey { get; set; }

        public int FrmRow { get; set; }

        public int ToRow { get; set; }

        public string AccountCode { get; set; }

        public string AccountName { get; set; }

        public string OurCode { get; set; }

        public AccountProfileRequest()
        {
            FrmRow = 1;
            ToRow = 999999;
            AccountCode = "";
            AccountName = "";
            OurCode = null;
        }

    }

    public class AccountProfileResponse
    {
        public int AccountKey { get; set; }

        public string AccountCode { get; set; }

        public string AccountName { get; set; }

        public CodeBaseResponse Account { get; set; }

        public bool IsActive { get; set; }

        public AccountProfileResponse()
        {
            this.AccountKey = 0;
            this.AccountCode = String.Empty;
            this.AccountName = String.Empty;
            this.Account = new CodeBaseResponse();
            this.IsActive = true;
        }
    }

    //insert

    public class AccountProfileInsertRequest : BaseEntity
    {
        public string AccountCode { get; set; }

        public string AccountName { get; set; }

        public CodeBaseResponse AccountType { get; set; }

        public bool IsActive { get; set; }

        public AccountProfileInsertRequest()
        {
            AccountCode = String.Empty;
            AccountName = String.Empty;
            AccountType = new CodeBaseResponse();
            IsActive = true;
        }

        public void CopyFrom(AccountProfileInsertRequest source)
        {
            source.CopyProperties(this);
        }
    }

    public class AccountProfileInsertResponse
    {
        public int AccountKey { get; set; }

        public int CKy { get; set; }

        public string AccountCode { get; set; }

        public string AccountName { get; set; }

        public int AccTypKy { get; set; }

        public AccountProfileInsertResponse()
        {
            this.AccountKey = 0;
            this.CKy = 0;
            this.AccountCode = String.Empty;
            this.AccountName = String.Empty;
            this.AccTypKy = 0;
        }
    }
}
