using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.Entities.Transaction
{
    public class ItemSerialNumber : BaseEntity
    {
        public long SerialNumberKey { get; set; } = 1;
        public long TransactionKey { get; set; } 
        public long ItemTransactionKey { get; set; }
        public int LineNumber { get; set; }
        public string SerialNumber { get; set; } = "";
        public string CustomerSerialNumber { get; set; } = "";
        public string SupplierWarrantyReading { get; set; } = "";
        public string CustomerWarrantyReading { get; set; } = "";
        public long ItemKey { get; set; } = 1;
        public DateTime? SupplierExpiryDate { get; set; } = DateTime.Now;
        public DateTime? CustomerExpiryDate { get; set; } = DateTime.Now;
        public string EngineNumber { get; set; } = "";
        public string VehicleNumber { get; set; } = "";
        public string BatchNumber { get; set; } = "";
        public long ElementKey { get; set; } = 1;
        public long PersistingElementKey { get; set; } = 1;
        public bool IsDefault { get; set; }
        public bool IsCompanyValidation { get; set; }

    }

    public class RecviedAmountResponse
    {
        public decimal TotalPayedAmount { get; set; }
    }

    public class InvoiceDetailsByHdrSerNo
    {

        public long TransactionKey { get; set; } = 1;
        public DateTime DeliveryDate { get; set; } = DateTime.Now;
        public string TransactionNumber { get; set; }
        public string GINTransactionNumber { get; set; }

        public CodeBaseResponse DeliveryType { get; set; }
        public CodeBaseResponse BU { get; set; }
        public string InvoiceSerialNumber { get; set; }

        public IList<SerInvoiceLInetItem> LineItems { get; set; }

        public bool IsAlreadyCheckedOut { get; set; }

        public string CheckkedOutTransactionNumber { get; set; }

        public DateTime CheckOutDate { get; set; } = DateTime.Now;

        public InvoiceDetailsByHdrSerNo()
        {
            LineItems = new List<SerInvoiceLInetItem>();
        }

    }

    public class SerInvoiceLInetItem
    {
        public long ItemTranactionKey { get; set; } = 1;
        public ItemResponse TransactionItem { get; set; }
        public ProjectResponse Project { get; set; }
        public decimal Quantity2 { get; set; }
        public decimal Quantity { get; set; }
        public decimal TransactionQuantity { get; set; }
        public UnitResponse TransactionUnit { get; set; }


    }

}
