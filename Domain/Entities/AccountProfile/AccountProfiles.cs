using BlueLotus.Com.Domain.Entity;
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
        public long ElementKey { get; set; } = 1;

        public int FrmRow { get; set; }

        public int ToRow { get; set; }

        public string? AccountCode { get; set; } = "";

        public string? AccountName { get; set; } = "";

        public string? OurCode { get; set; } = "";
        public string? Notes { get; set; } = "";
        public string? OurAccCd { get; set; } = "";

        public CodeBaseResponse Account { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Currency { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ParentAccount { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse BusinessUnit { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse AccessLevel { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse AccountCategory1 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse AccountCategory2 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse AccountCategory3 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse AccountCategory4 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse AccountCategory5 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse AccountCategory6 { get; set; } = new CodeBaseResponse();

        public bool IsActive { get; set; }
        public bool IsApprove { get; set; }
        public bool IsParentAccount { get; set; }
        public bool IsAllowTrn { get; set; }
        public bool IsControlAccount { get; set; }
        public bool IsBudget { get; set; }
        public bool IsCredit { get; set; }
        public bool IsDefault { get; set; }
        public bool IsCustSupplier { get; set; }
        public bool SysRec { get; set; }
        //public bool IsLmpCosting { get; set }

        public AccountProfileRequest()
        {
            FrmRow = 1;
            ToRow = 999999;
            AccountCode = "";
            AccountName = "";
            OurCode = null;
        }

    }

    public class AccountProfileResponse : BaseEntity
    {
        public int AccountKey { get; set; } = 1;
        public string? AccCd { get; set; } = "";
        public string? AccountCode { get; set; } = "";
        public string? AccNm { get; set; } = "";
        public string? AccountName { get; set; } = "";
        public string? Alias { get; set; } = "";
        public int BankKey { get; set; } = 1;
        public int BranchKey { get; set; } = 1;
        public string? AccountNumber { get; set; } = "";
        public string? EmailPersonal { get; set; } = "";
        public string? EmailBusiness { get; set; } = "";
        public string? MobilePersonal { get; set; } = "";
        public string? MobileBussiness { get; set; } = "";
        public string? Telephone { get; set; } = "";
        public decimal Notes { get; set; }
        public string? OurAccountCode { get; set; } = "";
        public string? SalesProcessGroup { get; set; } = "";
        public string? TransactionName { get; set; } = "";
        public string? TransactionHeaderCode { get; set; } = "";
        public string? TransactionDetailCode { get; set; } = "";
        public string? AddressID { get; set; } = "";
        public string? AddressName { get; set; } = "";
        public int Count { get; set; }
        public decimal SO { get; set; }
        public float Maint { get; set; }
        public string? TaxNo { get; set; } = "";
        public string? TaxNo2 { get; set; } = "";


        public CodeBaseResponse AccountType { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Currency { get; set; } = new CodeBaseResponse();
        public AccountResponse ParentAccount { get; set; } = new AccountResponse();
        public CodeBaseResponse BusinessUnit { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse AccessLevel { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse AccountCategory1 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse AccountCategory2 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse AccountCategory3 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse AccountCategory4 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse AccountCategory5 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse AccountCategory6 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemCategory2 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse MultiAddressType { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ConfinLevel { get; set; } = new CodeBaseResponse();

        public bool isProfileActive { get; set; }
        public bool isApprove { get; set; }
        public bool IsParentAccount { get; set; }
        public bool IsBudget { get; set; }
        public bool IsCredit { get; set; }
        public bool IsAllowedFroTransaction { get; set; }
        public bool IsControlAccount { get; set; }
        public bool IsCustomerSupplier { get; set; }
        public bool Select { get; set; }
        public bool IsDefault { get; set; }
        public bool IsSystemRecord { get; set; }
        public bool IsImpCosting { get; set; }
        public bool IsRootAccount { get; set; }
        public bool IsInEditMode { get; set; }
        public bool IsIntegrateToBank { get; set; }

        public AccountProfileResponse()
        {
            this.ConfinLevel = new CodeBaseResponse();
            this.MultiAddressType = new CodeBaseResponse();
            this.AccountType = new CodeBaseResponse();
            this.Currency = new CodeBaseResponse();
            this.ParentAccount = new AccountResponse();
            this.BusinessUnit = new CodeBaseResponse();
            this.AccessLevel = new CodeBaseResponse();
            this.AccountCategory1 = new CodeBaseResponse();
            this.AccountCategory2 = new CodeBaseResponse();
            this.AccountCategory3 = new CodeBaseResponse();
            this.AccountCategory4 = new CodeBaseResponse();
            this.AccountCategory5 = new CodeBaseResponse();
            this.AccountCategory6 = new CodeBaseResponse();
        }

        public void CopyFrom(AccountProfileResponse source)
        {
            source.CopyProperties(this);
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
        public int AccountKey { get; set; } = 1;

        public int CKy { get; set; }

        public string AccountCode { get; set; }

        public string AccountName { get; set; }

        public int AccTypKy { get; set; }

        public AccountProfileInsertResponse()
        {
           this.AccountCode = String.Empty;
           this.AccountName = String.Empty;
        }
    }
}
