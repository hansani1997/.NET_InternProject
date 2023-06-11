using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.CleanArchitecture.Domain.Utility;
namespace BlueLotus360.CleanArchitecture.Domain.Entities.Transaction
{
    public class BLTransaction : BaseEntity
    {
        public long TransactionKey { get; set; } = 1;

        public string TransactionNumber { get; set; } = "New";
        public long ElementKey { get; set; } = 1;
        public CodeBaseResponse TransactionType { get; set; }=new CodeBaseResponse();
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }

        public decimal HeaderDiscountAmount { get; set; }

        public CodeBaseResponse TransactionCurrency { get; set; }=new CodeBaseResponse();
        public decimal TransactionExchangeRate { get; set; }
        public string DocumentNumber { get; set; } = "";
        public string YourReference { get; set; } = "";
        public DateTime YourReferenceDate { get; set; }=DateTime.Now;
        public DateTime TransactionDate { get; set; }=DateTime.Now;
        public CodeBaseResponse PaymentTerm { get; set; } = new CodeBaseResponse();
        public int IsPrinted { get; set; }
        public int IsRecurrence { get; set; }
        public DateTime ReminderDate { get; set; } = DateTime.Now;
        public int IsLocked { get; set; }
        public CodeBaseResponse AccessLevel { get; set; }=new CodeBaseResponse();
        public CodeBaseResponse ConfidentialLevel { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ApproveState { get; set; } = new CodeBaseResponse();
        public long ObjectKey { get; set; } = 1;
        public ProjectResponse TransactionProject { get; set; } = new ProjectResponse();
        public CodeBaseResponse BussinessUnit { get; set; } = new CodeBaseResponse();
        public long HeaderTransferLinkKey = 1;
        public long ContraAccountObjectKey { get; set; } = 1; 
        public AccountResponse ContraAccount { get; set; } = new AccountResponse();
        public CodeBaseResponse Location { get; set; } =new CodeBaseResponse();
        public string Description { get; set; } = "";
        public string Remarks { get; set; } = "";
        public ItemResponse CustomItem { get; set; } = new ItemResponse();
        public CodeBaseResponse Shift { get; set; }=new CodeBaseResponse(); 
        public decimal CommisionPercentage { get; set; }

        public int IsQuantityPosted { get; set; }
        public int IsValuePosted { get; set; }
        public int IsItemTrnValue { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
        public decimal Amount3 { get; set; }
        public decimal Amount4 { get; set; }
        public decimal Amount5 { get; set; }
        public decimal Amount6 { get; set; }
        public string OtherNumber { get; set; } = "";
        public CodeBaseResponse Code1 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Code2 { get; set; } = new CodeBaseResponse();
        public AddressResponse Rep { get; set; } = new AddressResponse();
        public decimal RepCommissionPercentage { get; set; }
        public long OrderDetailKey { get; set; } = 1;
        public long AccountObjectKey { get; set; } = 1;
        public AccountResponse Account { get; set; } = new AccountResponse();
        public AddressResponse Address { get; set; } = new AddressResponse();
        public long OrderNumberKey { get; set; } = 1;
        public AddressResponse Address1 { get; set; } = new AddressResponse();
        public AddressResponse Address2 { get; set; } = new AddressResponse();
        public long FromOrderKey { get; set; } = 1;
        public long FromTransactionKey { get; set; } = 1;
        public long RecurenceDeliveryKey { get; set; } = 1;
        public string TransactionImageFilePath { get; set; } = "";
        public int IsMultiCredit { get; set; }
        public decimal ItemTaxType1 { get; set; }
        public decimal ItemTaxType2 { get; set; }
        public decimal ItemTaxType3 { get; set; }
        public decimal ItemTaxType4 { get; set; }
        public decimal ItemTaxType5 { get; set; }
        public bool IsFromImport { get; set; }
        public bool IsHold { get; set; }
        public bool IsBlock { get; set; } 
        public CodeBaseResponse ApproveReason { get; set; }= new CodeBaseResponse();
        public ItemSerialNumber SerialNumber { get; set; } = new ItemSerialNumber();

        public DateTime DeliveryDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now;
        public string Prefix { get; set; } = "";
        public string PreviewURL { get; set; } = "";
        public int EntUsrKy { get; set; }
        public IList<TransactionLineItem> InvoiceLineItems { get; set; } = new List<TransactionLineItem>();
        public decimal TotalMarkupValue { get; set; }
        public decimal MarkupPercentage { get; set; }
        public bool IsVarcar1On { get; set; }
        public decimal Quantity1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal AdvancePayment { get; set; }
        public decimal PendingPayment { get; set; }


        public decimal ValueAddedServiceTotal { get; set; }
        public decimal TotalCompensation { get; set; }

        public decimal SingleItemDisAmt { get; set; } = 5;
        public decimal SingleItemQty { get; set; }


        public decimal NetAmount { get; set; }
        public decimal SubTotal { get; set; }
        //

        public string Prefix2 { get; set; } = "";
   

        public TransactionLineItem SelectedLineItem { get; set; }=new TransactionLineItem();
        public TransactionLineItem EditingLineItem { get; set; } = new TransactionLineItem();


        public decimal TotalQuantity { get; set; }
        public decimal TotalQuantityOfPieces { get; set; }



        public BLTransaction()
        {
            Location = new CodeBaseResponse();
            PaymentTerm = new CodeBaseResponse();
            Address = new AddressResponse();
            Account = new AccountResponse();
            ContraAccount = new AccountResponse();
            Rep = new AddressResponse();
            InvoiceLineItems = new List<TransactionLineItem>();
            YourReference = Guid.NewGuid().ToString().Substring(0, 8);
            YourReferenceDate = DateTime.Now;
            Code1 = new CodeBaseResponse();
            SerialNumber = new ItemSerialNumber();
            InitilizeNewLineItem();
        }

        public TransactionLineItem InitilizeNewLineItem()
        {
            SelectedLineItem = new TransactionLineItem();
            SelectedLineItem.Address = this.Address;
            SelectedLineItem.TransactionLocation = this.Location;
            SelectedLineItem.TransactionProject = this.TransactionProject;
            SelectedLineItem.Analysis1 = new();
            SelectedLineItem.Analysis2 = new();
            SelectedLineItem.Analysis3 = new();
            SelectedLineItem.Analysis5 = new();
            SelectedLineItem.BaringCompany = new();
            SelectedLineItem.BaringPrinciple = new();
            return SelectedLineItem;
        }


        public void CopyFrom(BLTransaction source)
        {
            source.CopyProperties(this);

        }

        public decimal GetOrderTotalWithDiscounts()
        {
            return InvoiceLineItems.Where(x => x.IsActive == 1 && !x.TransactionItem.IsCompensationItem()).Sum(x => x.GetNetLineTotal());
        }

        public decimal GetOrderTotalWithoutDiscounts()
        {
            return InvoiceLineItems.Where(x => x.IsActive == 1).Sum(x => x.GetLineTotalWithoutDiscount());
        }

        public decimal GetTransactionRateTotal()
        {

            this.Amount = InvoiceLineItems.Where(x => x.IsActive == 1 && !x.TransactionItem.IsCompensationItem()).Sum(x => x.GetLineTotalWithDiscount());
            this.Amount = Math.Max(Amount - CalculateCompensationTotal(), 0);
            return Amount;
        }

        public decimal GetOrderDiscountTotal()
        {
            return InvoiceLineItems.Where(x => x.IsActive == 1).Sum(d => d.GetLineDiscount());
        }

        public decimal GetQuantityTotal()
        {
            return InvoiceLineItems.Where(x => x.IsActive == 1 && x.TransactionItem.IsTangibleItem()&& !x.TransactionItem.IsWeightItem()).Sum(q => q.TransactionQuantity);
        }

        public decimal GetArticleTotal()
        {
            return InvoiceLineItems.Where(x => x.IsActive == 1 && x.TransactionItem.IsTangibleItem() && !x.TransactionItem.IsWeightItem()).Sum(q => q.TransactionQuantity * q.Quantity2);
        }


        public void AddGridItems(TransactionLineItem item)
        {
            InvoiceLineItems.Add(item);
            SelectedLineItem = new();
        }


        public decimal CalculateCBalances()
        {
            Amount6 = Amount1 + Amount3 - this.GetOrderTotalWithDiscounts();
            return 0;

        }

        public void CalculateTotals()
        {
            HeaderDiscountAmount = SingleItemDisAmt * Amount4;
            SubTotal = this.GetOrderTotalWithDiscounts();
            CalculateCompensationTotal();
            CalculateServiceTotal();
            NetAmount = SubTotal - (TotalCompensation + HeaderDiscountAmount);
            PendingPayment = NetAmount - Amount5;
            TotalQuantity = GetQuantityTotal();
            TotalMarkupValue = InvoiceLineItems.Sum(x => (x.IsActive==1)? x.GetTotalMarkupAmount():0);
            // = InvoiceLineItems.Sum(x => x.GetTotalMarkupAmount());
            TotalQuantityOfPieces = GetArticleTotal();
            Amount = NetAmount;

        }


        public decimal CalculateServiceTotal()
        {
            ValueAddedServiceTotal = 0;
            foreach (TransactionLineItem item in InvoiceLineItems)
            {
                if (item.TransactionItem.IsServiceItem() && item.IsActive == 1)
                {
                    ValueAddedServiceTotal += item.GetNetLineTotal();
                }
            }
            return ValueAddedServiceTotal;
        }

        public decimal CalculateCompensationTotal()
        {
            TotalCompensation = 0;
            foreach (TransactionLineItem item in InvoiceLineItems)
            {
                if (item.TransactionItem.IsCompensationItem() && item.IsActive == 1)
                {
                    TotalCompensation += item.GetLineTotalWithDiscount();
                }
            }
            return TotalCompensation;
        }


        public bool IsOntheSpotPayementTransaction()
        {

            if (this.PaymentTerm != null && this.PaymentTerm.CodeKey > 10)
            {
                return this.PaymentTerm.CodeName.Contains("Cash", StringComparison.InvariantCultureIgnoreCase)
                        || this.PaymentTerm.CodeName.Contains("Card", StringComparison.InvariantCultureIgnoreCase);

            }
            return false;
        }

    }

    public class FindTransactionStatus
    {
        public DateTime FromDate { get; set; } = DateTime.Now;
        public DateTime ToDate { get; set; } = DateTime.Now;
        public int TranTypeKey { get; set; } = 1;
        public int PrefixKey { get; set; } = 1;
        public string TranNo { get; set; } = "";
        public string DocNo { get; set; } = "";
        public int ApproveStatusKey { get; set; } = 1;
        public int AccountKey { get; set; } = 1;
        public int ProjectKy { get; set; } = 1;
        public string PmtDocNo { get; set; } = "";
        public long ObjectKey { get; set; } = 1;
    }

    public class TransactionPermission
    {
        public int IsAllowInsert { get; set; }
        public int IsAllowUpdate { get; set; }
        public int IsAllowDelete { get; set; }
        public int IsAllowSourceDocumentPrint { get; set; }
        public int IsAlwAcs { get; set; }
        public int IsAllowAprrove { get; set; }
        public string? Message { get; set; } = "";
        public CodeBaseResponse NextApproveStatuis { get; set; } = new CodeBaseResponse();

        public TransactionPermission()
        {
            NextApproveStatuis = new CodeBaseResponse();
        }

    }

    /*
     public decimal GetOrderTotalWithDiscounts()
        {
            //return InvoiceLineItems.Where(x => x.IsActive == 1 && !x.TransactionItem.IsCompensationItem()).Sum(x => x.GetLineTotalWithDiscount());
            return InvoiceLineItems.Where(x => x.IsActive == 1 && !x.TransactionItem.IsCompensationItem()).Sum(x => x.GetNetLineTotal());
        }

        public decimal GetOrderTotalWithoutDiscounts()
        {
            return InvoiceLineItems.Where(x => x.IsActive == 1).Sum(x => x.GetLineTotalWithoutDiscount());
        }

        public decimal GetTransactionRateTotal()
        {

            this.Amount = InvoiceLineItems.Where(x => x.IsActive == 1 && !x.TransactionItem.IsCompensationItem()).Sum(x => x.GetLineTotalWithDiscount());
            this.Amount = Math.Max(Amount - CalculateCompensationTotal(), 0);
            return Amount;
        }

        public decimal GetOrderDiscountTotal()
        {
            return InvoiceLineItems.Where(x => x.IsActive == 1).Sum(d => d.GetLineDiscount());
        }

        public decimal GetQuantityTotal()
        {
            return InvoiceLineItems.Where(x => x.IsActive == 1 && x.TransactionItem.IsTangibleItem()).Sum(q => q.TransactionQuantity);
        }

        public decimal GetArticleTotal()
        {
            return InvoiceLineItems.Where(x => x.IsActive == 1 && x.TransactionItem.IsTangibleItem()).Sum(q => q.TransactionQuantity*q.Quantity2);
        }


        public void AddGridItems(TransactionLineItem item)
        {
            InvoiceLineItems.Add(item);
            SelectedLineItem = new();
        }


        public decimal CalculateCBalances()
        {
            Amount6 = Amount1 + Amount3 - this.GetOrderTotalWithDiscounts();
            return 0;

        }

        public void CalculateTotals()
        {
            HeaderDiscountAmount = SingleItemDisAmt * Amount4;
            SubTotal = this.GetOrderTotalWithDiscounts();
            CalculateCompensationTotal();
            CalculateServiceTotal();
            NetAmount = SubTotal - (TotalCompensation + HeaderDiscountAmount);
            PendingPayment = NetAmount - Amount5;
            TotalQuantity = GetQuantityTotal();
            TotalQuantityOfPieces=GetArticleTotal();
            Amount = NetAmount;

        }


        public decimal CalculateServiceTotal()
        {
            ValueAddedServiceTotal = 0;
            foreach (TransactionLineItem item in InvoiceLineItems)
            {
                if (item.TransactionItem.IsServiceItem() && item.IsActive == 1)
                {
                    //ValueAddedServiceTotal += item.GetLineTotalWithDiscount();
                    ValueAddedServiceTotal+=item.GetNetLineTotal();
                }
            }
            return ValueAddedServiceTotal;
        }

        public decimal CalculateCompensationTotal()
        {
            TotalCompensation = 0;
            foreach (TransactionLineItem item in InvoiceLineItems)
            {
                if (item.TransactionItem.IsCompensationItem() && item.IsActive==1)
                {
                    TotalCompensation += item.GetLineTotalWithDiscount();
                }
            }
            return TotalCompensation;
        }


        public bool IsOntheSpotPayementTransaction()
        {

            if (this.PaymentTerm != null && this.PaymentTerm.CodeKey > 10)
            {
                return this.PaymentTerm.CodeName.Contains("Cash", StringComparison.InvariantCultureIgnoreCase)
                        || this.PaymentTerm.CodeName.Contains("Card", StringComparison.InvariantCultureIgnoreCase);

            }
            return false;
        }
     */

}
