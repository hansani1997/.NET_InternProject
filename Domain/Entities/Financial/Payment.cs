using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.CleanArchitecture.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.Financial
{
    public class Payment : BaseEntity
    {
        public DateTime PaymentDate { get; set; }= DateTime.Now;    
        public AccountResponse DebitAccount { get; set; }
        public AccountResponse CreditAccount { get; set; }
        public AddressResponse PaymentAddress { get; set; }
        public decimal PaymentAmount { get; set; }
        public string? Description { get; set; }
        public string? PaymentDocumentNumber { get; set; }  
        public bool IsInEditMode { get; set; }
        public Payment()
        {
            DebitAccount= new AccountResponse();
            CreditAccount= new AccountResponse();
            PaymentAddress= new AddressResponse();
        }

        public void Clear()
        {
            CreditAccount = new AccountResponse();
            PaymentAddress = new AddressResponse();
            PaymentAmount = 0;
            Description = "";
        }

        public void CopyFrom(Payment source)
        {
            source.CopyProperties(this);

        }
    }

    public class BLJournalLite : BLTransaction
    {
        public AccTrnSingleEntry SelectedSingleEntry { get; set; }
        public List<AccTrnSingleEntry> AccTrnSingleEntries { get; set; }

        public BLJournalLite()
        {
            SelectedSingleEntry = new AccTrnSingleEntry();
            AccTrnSingleEntries = new List<AccTrnSingleEntry>();
        }

        public void CopyFrom(BLJournalLite source)
        {
            source.CopyProperties(this);

        }
    }

    public class AccTrnSingleEntry : TransactionLineItem
    {
        public AccountResponse DebitAccount { get; set; }
        public AccountResponse CreditAccount { get; set; }
        public decimal PaymentAmount { get; set; }
        public int CreditAccountTrnKy { get; set; }
        public int DebitAccountTrnKy { get; set; }
        
        public AccTrnSingleEntry()
        {
            DebitAccount = new AccountResponse();
            CreditAccount = new AccountResponse();
            PaymentAmount = 0;
            EffectiveDate=DateTime.Now; 
        }
    }

    public class PettyCash
    {
        
    }
}
