
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.CleanArchitecture.Domain.Utility;

namespace BL10.CleanArchitecture.Domain.Entities.Dashboard
{
    public class AuditTrail : BaseEntity
    {
        public long ElementKey { get; set; } = 1;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public AddressResponse User { get; set; }
        public int Id { get; set; }
        public int EnterdCount { get; set; }
        public int UpdatedCount { get; set; }

        public AuditTrail()
        {
            FromDate = DateTime.Today;
            ToDate = DateTime.Today;
            User = new AddressResponse();
            Id = 0;
            EnterdCount = 0;
            UpdatedCount = 0;
        }

        public void CopyFrom(AuditTrail source)
        {
            source.CopyProperties(this);

        }
    }
    public class AuditTrailEnterdUpdatedResponse
    {
        public int TransactionKey { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TransactionNumber { get; set; }
        public decimal Amount { get; set; }
        public string? CreditAccountCode { get; set; }
        public string? DebitAccountCode { get; set; }
        public string? Description { get; set; }

        public AuditTrailEnterdUpdatedResponse()
        {
            TransactionKey = 0;
            TransactionDate = new DateTime();
            TransactionNumber = 0;
            Amount = 0;
            CreditAccountCode = string.Empty;
            DebitAccountCode = string.Empty;
            Description = string.Empty;
        }
    }
}
