using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.Entities.Transaction
{
    public class CashInOutTransaction : BaseEntity
    {
        public string TransactionNumber { get; set; } = "";
        public string DocumentNumber { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Today;
        public CodeBaseResponse Location { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse WorkStation { get; set; } = new CodeBaseResponse();
        public AddressResponse Address { get; set; } = new AddressResponse();
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public long ElementKey { get; set; }


    }


    public class URLDefinitions
    {
        public string URL { get; set; }

    }


    public class GetFromTransactionRequest
    {
        public CodeBaseResponse TransactionType { get; set; }
        public CodeBaseResponse PreviousTransactionType { get; set; }
        public CodeBaseResponse Location { get; set; }
        public AccountResponse SupplierAccount { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public ProjectResponse Project { get; set; }
        public int TransactionNumber { get; set; } = 1;
        public string SerNo { get; set; }
        public long ElementKey { get; set; }
        public long FromElementKey { get; set; }



    }

    public class GetFromTransactionResponse
    {
        public long TransactionKey { get; set; } = 1;
        public string TransactionNumber { get; set; }
        public string DocumentNumber { get; set; }
        public string Prefix { get; set; }
        public DateTime TransactionDate { get; set; }
        public ProjectResponse Project { get; set; }
        public AddressResponse Address { get; set; }
        public long CodeKey { get; set; } = 1;
        public long ControlConKey { get; set; } = 1;
        public int LineNumber { get; set; }

    }

    public class FromTransactionOpenRequest
    {
        public long TransactionKey { get; set; } = 1;
        public CodeBaseResponse TransactionType { get; set; }
        public decimal ElementKey { get; set; }
    }

    

}
