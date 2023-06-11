using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.CleanArchitecture.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.Entities.InventoryManagement.ItemTransfer
{
    public class ItemTransfer : BaseEntity
    {
        public int TrnTypKy { get; set; } = 1;
        public int FromTransactionKey { get; set; } = 1;
        public int ToTransactionKey { get; set; } = 1;
        public string TransactionNumber { get; set; } = "";
        public long ElementKey { get; set; } = 1;
        public string YourReference { get; set; } = "";
        public DateTime TransactionDate { get; set; }
        public CodeBaseResponse FromLocation { get; set; }
        public CodeBaseResponse ToLocation { get; set; }
        public decimal Quantity1 { get; set; } = 0;
        public string PrefixTrnNo { get; set; } = "";
        public string OurCode { get; set; } = "";
        public int HdrTrfLnkKy { get; set; } = 1;
        public string Prefix { get; set; } = "";
        public int PmtTrmKy { get; set; } = 1;
        public int isAct { get; set; } = 1;
        public int isApr { get; set; } = 1;
        public string FromTmStmp { get; set; } = "";
        public string ToTmStmp { get; set; } = "";
        public int ConFinLvlKy { get; set; } = 1;
        public int AcsLvlKy { get; set; } = 1;
        public int FromBuKy { get; set; } = 1;
        public int ToBuKy { get; set; } = 1;
        public bool CanUpdateToLocation { get; set; }
        public ItemTransferLineItem SelectedLineItem { get; set; }
        public List<string> TrnKyList { get; set; }
        public List<string> SerialNoList { get; set; }
        public List<ItemTransferLineItem> ScanItemTransferLineItem { get; set; }
        public IList<ItemTransferLineItem> ScanItemTransferLineItemNew { get; set; }
        public IList<ItemTransferLineItem> ScanItemTransferLineItemUpdated { get; set; }

        public LNDInvoice SelectedInvoice { get; set; }
        public List<LNDInvoice> Invoices { get; set; }
        public ItmtrnsferValidationResponse LocationWiseSerialNoValidations { get; set; }
        public List<string> ValidationMessages { get; set; }
        public string Description { get; set; } = "";
        public string Remark { get; set; } = "";
        public AddressResponse Address { get; set; }
        public ItemTransfer()
        {
            FromLocation = new CodeBaseResponse();
            ToLocation = new CodeBaseResponse();
            TransactionDate = DateTime.Now;
            FromTransactionKey = 1;
            ToTransactionKey = 1;
            ElementKey = 1;
            TrnKyList = new List<string>();
            SerialNoList = new List<string>();
            ScanItemTransferLineItem = new List<ItemTransferLineItem>();
            ScanItemTransferLineItemNew = new List<ItemTransferLineItem>();
            ScanItemTransferLineItemUpdated = new List<ItemTransferLineItem>();
            SelectedInvoice = new LNDInvoice();
            Invoices = new List<LNDInvoice>();
            LocationWiseSerialNoValidations = new ItmtrnsferValidationResponse();
            ValidationMessages = new List<string>();
            Address = new AddressResponse();
            SelectedLineItem = new ItemTransferLineItem();
        }

        public void CopyFrom(ItemTransfer source)
        {
            source.CopyProperties(this);

        }
        public void SetInvoice()
        {
            SerialNoList = new List<string>();

            foreach (var itm in this.ScanItemTransferLineItem.ToLookup(x => (x.InvoiceNo != null) ? x.InvoiceNo.Trim() : ""))
            {
                if (itm.FirstOrDefault().isActive == 0)
                {
                    continue;
                }
                else
                {
                    SelectedInvoice.InvoiceNo = itm.FirstOrDefault().InvoiceNo;
                    SelectedInvoice.TransactionNumber = itm.FirstOrDefault().TrnNo;
                    SelectedInvoice.BranchCode = itm.FirstOrDefault().BranchCode;
                    SelectedInvoice.ServiceName = itm.FirstOrDefault().ServiceName;
                    SelectedInvoice.DeliveryDate = !string.IsNullOrEmpty(itm.FirstOrDefault().DeliveryDate) ? DateTime.Parse(itm.FirstOrDefault().DeliveryDate) : DateTime.Now;
                    SelectedInvoice.DeliveryType = itm.FirstOrDefault().DeliveryType;
                    SelectedInvoice.IsActive = Convert.ToByte(itm.FirstOrDefault().isActive);
                    SelectedInvoice.SerialNumber.SerialNumber = itm.FirstOrDefault().TrnSerNo;
                    SelectedInvoice.DlryTypColour = itm.FirstOrDefault().DlryTypColour;
                    SelectedInvoice.SerialNumber.SerialNumberKey = itm.FirstOrDefault().HdrSerNoKy;
                    SelectedInvoice.Location.CodeKey = itm.FirstOrDefault().LocationKey;
                    SelectedInvoice.IsToSerApr = itm.Where(x => x.ToIsApr == 0).Count() == 0;
                    SelectedInvoice.LineItems = itm.ToList();
                    if (!string.IsNullOrEmpty(itm.FirstOrDefault().serialNo) && itm.FirstOrDefault().isActive == 1 && this.SerialNoList != null)
                    {
                        this.SerialNoList = SelectedInvoice.LineItems.Select(x => (x.serialNo != null) ? x.serialNo : "").ToList();
                    }
                    this.Invoices.Add(SelectedInvoice);
                    SelectedInvoice = new LNDInvoice();


                }



            }


        }
    }

    public class ItemTransferLineItem
    {
        public int ObjectKey { get; set; } = 1;
        public string TrnNo { get; set; } = "";
        public string OurCode { get; set; } = "";
        public int LineNumber { get; set; } = 1;
        public int TransactionTypeKey { get; set; } = 1;
        public string TransactionType { get; set; } = "";
        public int TransactionKey { get; set; } = 1;
        public long ItemKey { get; set; } = 1;
        public string ItemName { get; set; } = "";
        public string ItemCode { get; set; } = "";
        public int OriItmTrnKy { get; set; } = 1;
        public int FromItemTrnKey { get; set; } = 1;
        public int ToItemTrnKey { get; set; } = 1;
        public int LocationKey { get; set; } = 1;
        public decimal Qty { get; set; } = 0;
        public decimal TransactionQuantity { get; set; } = 0;
        public int TrnUnitKy { get; set; } = 1;
        public string? TrnUnit { get; set; } = "";
        public decimal TransactionRate { get; set; } = 0;
        public int PreItmTrnKy { get; set; } = 1;
        public int ItmTrnTrfLnkKy { get; set; } = 1;
        public int ReturnVal { get; set; } = 1;
        public int NoOfArticles { get; set; } = 1;
        public string serialNo { get; set; } = "";
        public IList<ItemSerialNumber> SerialNumbers { get; set; } = new List<ItemSerialNumber>();
        public string TrnSerNo { get; set; } = "";
        public int SerNoKy { get; set; } = 1;
        public string BranchCode { get; set; } = "";
        public string ServiceName { get; set; } = "";
        public string DeliveryType { get; set; } = "";
        public string DeliveryDate { get; set; } = "";
        public decimal TrnPri { get; set; }
        public int FromBuKy { get; set; } = 1;
        public int ToBuKy { get; set; } = 1;
        public decimal DisAmt { get; set; }
        public decimal TrnDisAmt { get; set; }
        public decimal DisPer { get; set; }
        public string TmStmp { get; set; } = "";
        public int isActive { get; set; } = 1;
        public int FromPrjKy { get; set; } = 1;
        public int ToPrjKy { get; set; } = 1;
        public int AdrKy { get; set; } = 1;
        public int ItmPrpKy { get; set; } = 1;
        public int isInventory { get; set; } = 1;
        public int isCosting { get; set; } = 1;
        public int IsSetOff { get; set; } = 1;
        public int isApr { get; set; }
        public int ItmTrnPrp1Ky { get; set; } = 1;
        public int RefItmTrnKy { get; set; } = 1;
        public string Des { get; set; } = "";
        public string Rem { get; set; } = "";
        public int CondStateKy { get; set; } = 1;
        public decimal HdrDisAmt { get; set; }
        public int FrmNo { get; set; } = 1;
        public int ToNo { get; set; } = 1;
        public int NxtActNo { get; set; } = 1;
        public DateTime NxtActDt { get; set; }
        public int NxtActTypKy { get; set; } = 1;
        public int ItmPackKy { get; set; } = 1;
        public decimal ComisPer { get; set; }
        public int isQty { get; set; } = 1;
        public int isPri { get; set; } = 1;
        public int isVal { get; set; } = 1;
        public int isDisplay { get; set; } = 1;
        public int isNoPrnPri { get; set; } = 1;
        public int isQlity { get; set; } = 1;
        public int AcsLvlKy { get; set; } = 1;
        public int CdKy1 { get; set; } = 1;
        public int CdKy2 { get; set; } = 1;
        public string DlryTypColour { get; set; } = "";
        public string Prefix { get; set; } = "";
        public int PrefixKy { get; set; } = 1;
        public string InvoiceNo { get; set; } = "";
        public int HdrSerNoKy { get; set; } = 1;
        public bool IsJustScanned { get; set; }
        public decimal Peices { get; set; } = 0;
        public ItmtrnsferValidationResponse LocationWiseitemSerialNoValidation { get; set; }
        public CodeBaseResponse ApproveState { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse TransferredLocation { get; set; } = new CodeBaseResponse();
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public CodeBaseResponse AnalysisType1 { get; set; } = new CodeBaseResponse();
        public decimal PreTransactionQuantity { get; set; }
        public int ToIsApr { get; set; }
        public int ToSerIsApr { get; set; }
        public decimal ItemTaxType1Per { get; set; }
        public decimal ItemTaxType2Per { get; set; }
        public decimal ItemTaxType3Per { get; set; }
        public decimal ItemTaxType4Per { get; set; }
        public decimal ItemTaxType5Per { get; set; }

        public decimal ItemTaxType1 { get; set; }
        public decimal ItemTaxType2 { get; set; }
        public decimal ItemTaxType3 { get; set; }
        public decimal ItemTaxType4 { get; set; }
        public decimal ItemTaxType5 { get; set; }
        public ItemTransferLineItem()
        {
            LocationWiseitemSerialNoValidation = new ItmtrnsferValidationResponse();
            ApproveState = new CodeBaseResponse();
            TransferredLocation = new CodeBaseResponse();
            AnalysisType1 = new CodeBaseResponse();
        }
    }

    public class FindItemTransferRequest
    {
        public string TrnNo { get; set; } = "";
        public DateTime? FrmDt { get; set; } = DateTime.Now;
        public DateTime? ToDt { get; set; } = DateTime.Now;
        public int TrnTypKy { get; set; } = 1;
        public int InvoiceNo { get; set; } = 1;
        public long AprStsKy { get; set; } = 1;
        public long ItmKy { get; set; } = 1;
        public long AdrKy { get; set; } = 1;
        public CodeBaseResponse FrmLoc { get; set; }
        public CodeBaseResponse ToLoc { get; set; }
        public int FrmPrjKy { get; set; } = 1;
        public int ToPrjKy { get; set; } = 1;
        public int SupKy { get; set; } = 1;
        public int BUKy { get; set; } = 1;
        public int isRecur { get; set; } = 0;
        public int isPrinted { get; set; } = 0;
        public int ObjKy { get; set; } = 1;
        public int PreFixKy { get; set; } = 1;

        public FindItemTransferRequest()
        {
            //FrmDt=null;
            //ToDt=null;
            FrmLoc = new CodeBaseResponse();
            ToLoc = new CodeBaseResponse();
        }
    }

    public class FindItemTransferResponse
    {
        public string TrnNo { get; set; } = "";
        public int TrnKy { get; set; } = 1;
        public DateTime TrnDt { get; set; }
        public string Prefix { get; set; } = "";
        public string DocNo { get; set; } = "";
        public string YurRef { get; set; } = "";
        public int AdrId { get; set; } = 1;
        public string AdrNm { get; set; } = "";
        public int PrjId { get; set; } = 1;
        public string PrjNm { get; set; } = "";
        public int DepTrnNo { get; set; } = 1;
        public string ServiceName { get; set; } = "";
    }

    public class TransferOpenRequest
    {
        public long ObjKy { get; set; }
        public int TrnKy { get; set; } = 1;
        public int isStockAdd { get; set; } = 0;
    }

    public class LNDInvoice : BLTransaction
    {
        public string InvoiceNo { get; set; } = "";
        public string BranchCode { get; set; } = "";
        public string ServiceName { get; set; } = "";
        public string DeliveryType { get; set; } = "";//code2        
        public bool ShowDetails { get; set; } = false;
        public string DlryTypColour { get; set; } = "";
        public int PrefixKy { get; set; } = 1;
        public bool IsJustScanned { get; set; }
        public bool IsToSerApr { get; set; }
        public IList<ItemTransferLineItem> LineItems { get; set; }
        public ItmtrnsferValidationResponse LocationWiseInvoiceSerialNoValidation { get; set; }
        public LNDInvoice()
        {
            LineItems = new List<ItemTransferLineItem>();
            LocationWiseInvoiceSerialNoValidation = new ItmtrnsferValidationResponse();
        }


    }

    public class ItmtrnsferValidationResponse
    {
        public bool HasError { get; set; }
        public string Message { get; set; } = "";
    }


}
