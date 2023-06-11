using BL10.CleanArchitecture.Domain.DTO.Object;
using BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlueLotus.Com.Domain.Entity
{
    public class ItemSimple:BaseEntity
    {
        public string? ItemName { get; set; } = "";  // Added                   //ItemName
        public int ItemKey { get; set; } // Added                               //ItemKey
        public string? ItemCode { get; set; } = ""; // Added                    //ItemCode
        public string? ItemNameOnly { get; set; } = "";
        public string? ItemCodeOnly { get; set; } = "";
        public int FilterKey { get; set; }
        public ItemSimple()
        {
            ItemKey = 1;
        }
        public ItemSimple(int ItemKey)
        {
            this.ItemKey = ItemKey;
        }
        public bool IsParentItem { get; set; }
        public string ComboTitle { get {
                if (ItemKey == 1)
                {
                    return "-";
                }
            else {
                    return ItemName;
                }
            } 
        }
        public CodeBaseResponse ItemType { get; set; } = new CodeBaseResponse();
        public string? Base64ImageDocument { get; set; } = "";

    }

    public class Item : ItemSimple
    {
        public int LiNo { get; set; }
        public string? EAN { get; set; } = "";  // Added
        public string? ItemShortName { get; set; } = ""; // Added
        public string? Description { get; set; } = ""; // Added                 //Description
        public UnitResponse ItemUnit { get; set; } = new UnitResponse();        //ItemUnit
        //public UnitResponse ServiceUnit { get; set; } = new UnitResponse();
        public string? Remarks { get; set; } = "";  // Added
        public decimal CostPrice { get; set; }  // Added
        public decimal SalesPrice { get; set; } // Added
        public decimal OptionalSalesPrice { get; set; } // Added
        public bool IsModifierItem { get; set; } //Added
        public bool IsCompositeItem { get; set; } //Added
        public bool IsPayemntType { get; set; } //Added
        public decimal MaximumDiscount { get; set; }//Added
        public decimal VatPercentage { get; set; }//Added
        public CodeBaseResponse ItemCategory1 { get; set; } = new CodeBaseResponse();   //Categories
        public CodeBaseResponse ItemCategory2 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemCategory3 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemCategory4 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemCategory5 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemCategory6 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemCategory7 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemCategory8 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemPriceCategory { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemProperty1 { get; set; } = new CodeBaseResponse(); //color
        public CodeBaseResponse ItemProperty2 { get; set; } = new CodeBaseResponse(); //size
        public CodeBaseResponse ItemProperty3 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemProperty4 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Brand { get; set; } = new CodeBaseResponse();       //Brand
        public DateTime ExpireDate { get; set; } //Added
        public int ValueForProjectKey { get; set; } = 1; //Added

        public decimal SupplierWarranty { get; set; }                           //SupplierWarranty
        public decimal CustomerWarranty { get; set; }
        public int ProjectId { get; set; } = 1;
        public string ItemComboTitle
        {
            get
            {
                return ItemCode + " - " + ItemName;
            }
        }
        public string? PartNumber { get; set; } = "";                           //PartNumber
        public CodeBaseResponse Model { get; set; } = new CodeBaseResponse();   //Model
        public bool IsSerialNumber { get; set; }
        public decimal ReOrderLevel { get; set; }                               //RedorderLevel
        public decimal ReOrderQuantity { get; set; }
        public string? ItmCd { get; set; } = "";
        public string? ItmNm { get; set; } = "";
        public bool IsImpCosting { get; set; }
        public int HSCodeKey { get; set; } = 1;
        public bool isAlwTrnRateChange { get; set; }
        public decimal length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public bool isPos { get; set; }
        public bool isSysRec { get; set; }
        public int BUKey { get; set; }
        public CodeBaseResponse AccessLevel { get; set; } = new CodeBaseResponse();
        public int Count { get; set; }

        //Item Profile V3.0 ------------------

        public CodeBaseResponse ItemType { get; set; } = new CodeBaseResponse();
        public ItemResponse ParentItem { get; set; } = new ItemResponse();
        public AddressResponse DefaultSupplier { get; set; } = new AddressResponse();
        public string? OldItemCode { get; set; } = "";
        public bool IsProfActive { get; set; } = true;
        public bool IsProfApprove { get; set; } = true;
        public IList<Base64Document> Base64Documents { get; set; } = new List<Base64Document>();
        public bool IsInEditMode { get; set; }
        public IList<ItemUnit> MultiUnits { get; set; } = new List<ItemUnit>();

        public int[] ItemKeys { get; set; }
        

        public Item()
        {
            ExpireDate = Convert.ToDateTime("1990/01/01");
            ItemUnit = new UnitResponse();
            //ServiceUnit = new UnitResponse();
            ItemCategory1= new CodeBaseResponse();
            ItemCategory2= new CodeBaseResponse();
            ItemCategory3= new CodeBaseResponse();
            ItemCategory4= new CodeBaseResponse();
            ItemCategory5= new CodeBaseResponse();
            ItemCategory6= new CodeBaseResponse();
            ItemCategory7= new CodeBaseResponse();
            ItemCategory8= new CodeBaseResponse();
            ItemPriceCategory = new CodeBaseResponse();
            ItemProperty1  = new CodeBaseResponse();
            ItemProperty2 = new CodeBaseResponse();
            ItemProperty3 = new CodeBaseResponse();
            ItemProperty4 = new CodeBaseResponse();
            Brand = new CodeBaseResponse();
            Model = new CodeBaseResponse();
            AccessLevel = new CodeBaseResponse();

            //Item Profile v3.0 -----------

            this.ItemType = new CodeBaseResponse();
            this.ParentItem = new ItemResponse();
            this.DefaultSupplier = new AddressResponse();

        }

        public void CopyFrom(Item source)
        {
            source.CopyProperties(this);
        }

    }

    public class ItemUnit : BaseEntity
    {
        public int ItemUnitkey { get; set; }
        public bool IsBaseUnit { get; set; }
        public decimal BaseUnitConversionRate { get; set; }
        public decimal UnitConversionRate { get; set; }
        public decimal ConversionRate { get; set; }
        public bool IsPurchaseUnit { get; set; }
        public bool IsDefaultPurchaseUnit { get; set; }
        public bool IsFractionPurchaseUnit { get; set; }
        public bool IsInventoryUnit { get; set; }
        public bool IsDefaultInventoryUnit { get; set; }
        public bool IsFractionInventoryUnit { get; set; }
        public bool IsSalesUnit { get; set; }
        public bool IsDefaultSalesUnit { get; set; }
        public bool IsFractionSalesUnit { get; set; }
        public bool IsServiceUnit { get; set; }
        public bool IsService1Unit { get; set; }
        public bool IsBulkUnit { get; set; }
        public bool IsInnerUnit { get; set; }
        public bool IsLooseUnit { get; set; }
        public string? Description { get; set; } = "";
        public string? EqualMark { get; set; } = "=";
        public int ItemKey { get; set; } = 1;
        public UnitResponse TargetUnit { get; set; } = new UnitResponse();
        public UnitResponse BasedUnit { get; set; } = new UnitResponse();
        public UnitResponse PurchaseBulkUnit { get; set; } = new UnitResponse();
        public UnitResponse PurchaseInnerUnit { get; set; } = new UnitResponse();
        public UnitResponse PurchaseLooseUnit { get; set; } = new UnitResponse();
        public UnitResponse SalesBulkUnit { get; set; } = new UnitResponse();
        public UnitResponse SalesInnerUnit { get; set; } = new UnitResponse();
        public UnitResponse SalesLooseUnit { get; set; } = new UnitResponse();
        public UnitResponse InventoryBulkUnit { get; set; } = new UnitResponse();
        public UnitResponse InventoryInnerUnit { get; set; } = new UnitResponse();
        public UnitResponse InventoryLooseUnit { get; set; } = new UnitResponse();
        public bool IsInEditMode { get; set; }
        public bool IsUnitActive { get; set; }
        public ItemUnit()
        {

        }

        public void CopyFrom(ItemUnit source)
        {
            source.CopyProperties(this);
        }

    }
    //public class MultiUnitsTab : BaseEntity
    //{
    //    public int ItemKey { get; set; } = 1;
    //    public string? EqualMark { get; set; } = "=";
    //    public UnitResponse TargetUnit { get; set; } =new UnitResponse();  //MappedUnitName
    //    public decimal BaseUnitConversionRate { get; set; }
    //    public decimal UnitConversionRate { get; set; }
    //    public bool IsSalesUnit { get; set; }
    //    public decimal ConversionRate { get; set; }
    //    public bool IsPurchaseUnit { get; set; }
    //    public bool IsBaseUnit { get; set; }
    //    public bool IsInEditMode { get; set; }
    //    public bool IsJustAdded { get; set; }
    //    public int ItemUnitkey { get; set; } = 1;
    //    public int BaseUnitKey { get; set; } = 1;
    //    public string? BaseUnit { get; set; } = "";

    //    public void CopyFrom(MultiUnitsTab source)
    //    {
    //        source.CopyProperties(this);
    //    }
    //}

    public class ItemCombinations
    {
        public int RequestingObjectKey { get; set; } = 1;
        public int ParentItemKey { get; set; } = 1;
        public string? ItemCode { get; set; } = "";
        public IList<CodeBaseResponse> ItemProperty1 { get; set; } = new List<CodeBaseResponse>();
        public IList<CodeBaseResponse> ItemProperty2 { get; set; } = new List<CodeBaseResponse>();
        public IList<CodeBaseResponse> ItemProperty3 { get; set; } = new List<CodeBaseResponse>();
        public IList<CodeBaseResponse> ItemProperty4 { get; set; } = new List<CodeBaseResponse>();
        public decimal CostPrice { get; set; }
        public decimal SalesPrice { get; set; }
        public bool IsInEditMode { get; set; }

    }

    public class ItemComponent : BaseEntity
    {
        public ItemResponse FInishedItem { get; set; } = new ItemResponse();
        public ItemResponse ComponentItem { get; set; } = new ItemResponse();
        public string? Description { get; set; } = "";
        public decimal Quantity { get; set; }
        public decimal RequestedQuantity { get; set; }
        public decimal AnalysisQuantity { get; set; }
        public decimal WastagePercentage { get; set; }
        public decimal WastageQuantity { get; set; }
        public decimal UsagePercentage { get; set; }
        public decimal TransactionQuantity { get; set; }
        public decimal TransactionRate { get; set; }
        public decimal LineTotal
        {
            get
            {
                return (TransactionQuantity * TransactionRate) + ItemTaxType1;
            }
        }
        public decimal Rate { get; set; }
        public UnitResponse TransactionUnit { get; set; } = new UnitResponse();

        public UnitResponse BaseUnit { get; set; } = new UnitResponse();

        public decimal ConversionRate { get; set; } = 1;
        public decimal CompactFactor { get; set; }

        public CodeBaseResponse ResReqSchTyp { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemProperty { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Site { get; set; } = new CodeBaseResponse();
        public int ItemComponentKey { get; set; } = 1;
        public bool isAdrWise { get; set; }

        public bool IsRateIncludedTaxType1 { get; set; }
        public bool IsRateIncludedTaxType2 { get; set; }
        public bool IsRateIncludedTaxType3 { get; set; }
        public bool IsRateIncludedTaxType4 { get; set; }
        public bool IsRateIncludedTaxType5 { get; set; }

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

        public void CalculateTaxes()
        {
            decimal DiscountAmount = 0;
            decimal PreDiscountLineTotal = TransactionQuantity * TransactionRate;
            ItemTaxType1 = (PreDiscountLineTotal - DiscountAmount) * (ItemTaxType1Per / 100);
        }
        public bool HasError { get; set; }
        public string? Message { get; set; } = "";
        public bool IsInEditMode { get; set; }

        public void CopyFrom(ItemComponent source)
        {
            source.CopyProperties(this);
        }
    }


    public class ItemExtended : Item
    {
        public CodeBaseResponse ItemCategory9 { get; set; }
        public CodeBaseResponse ItemCategory10 { get; set; }
        public CodeBaseResponse ItemCategory11 { get; set; }
        public CodeBaseResponse ItemCategory12 { get; set; }

        public bool IsMain { get; set; }
        public byte Level { get; set; }
        public CodeBaseResponse ConfidentialLevel { get; set; }

        public decimal GrossWeight { get; set; }
        public decimal NetWeight { get; set; }
        public decimal Warrenty { get; set; }
        public decimal AverageWarrenty { get; set; }

        public CodeBaseResponse BussienssUnit { get; set; }
        //public Account IncomeAccount { get; set; }
        //public Account ExpenseAccount { get; set; }
        //public Account AssetAccount { get; set; }
        //public Account DepreiciationAccount { get; set; }
        //public Account CostAccount { get; set; }
        //public Account CostManufactureAccount { get; set; }
        public decimal DepriciationPercentage { get; set; }
        public string OwnPartNumber { get; set; }
        //public Unit WarrentyUnit { get; set; }
        //public Address Address { get; set; }

        //public Unit LooseUnit { get; set; }
        //public Unit BulkUnit { get; set; }
        //public Unit StandardUnit { get; set; }
        //public Unit InernalUnit { get; set; }

        public decimal AnalysisQuantity { get; set; }

        public decimal NumberOfCondenseStates { get; set; }

        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }

        public bool IsDiscountinued { get; set; }

        public bool AllowTransactionRateChange { get; set; }

        public bool AllowZeroPrice { get; set; }

        public decimal MinimumQuantity { get; set; }
        public decimal MaximumQuantity { get; set; }

        public CodeBaseResponse ItemAccountCategory { get; set; }
        public bool IsSubstitute { get; set; }

        public bool IsItem1 { get; set; }
        public bool IsItem2 { get; set; }
        public bool IsItem3 { get; set; }
        public bool IsItem4 { get; set; }
        public bool IsGeneric { get; set; }

     
        public bool AllowForTransaction { get; set; }



    }

    public class ItemCodeRetrivalDTO
    {
        public int ItemTypeKey { get; set; } = 1;
        public int BrandKey { get; set; } = 1;
        public int ItemCategory2Key { get; set; } = 1;
    }

    public class FilterItem
    {
        public string SearchQuery { get; set; }

        public int FilterItemCategory1Key { get; set; }
        public int FilterItemCategory2Key { get; set; }
        public int FilterItemCategory3Key { get; set; }
        public int FilterItemCategory4Key { get; set; }
        public int FilterItemTypeKey { get; set; }
    }
    public class ItemRetrivalDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SearchQuery { get; set; }

        public int ItemCategory1Key { get; set; }
        public int ItemCategory2Key { get; set; }
        public int ItemCategory3Key { get; set; }
        public int ItemCategory4Key { get; set; }

        public int ItemCategory5Key { get; set; }
        public int ItemCategory6Key { get; set; }
        public int ItemCategory7Key { get; set; }
        public int ItemCategory8Key { get; set; }

        public int ItemCategory9Key { get; set; }
        public int ItemCategory10Key { get; set; }
        public int ItemCategory11Key { get; set; }
        public int ItemCategory12Key { get; set; }

        public int ItemTypeKey { get; set; }
        public int UnitKey { get; set; }
        public int ServiceUnitKey { get; set; }
        //public MinMax CostPriceFilter { get; set; }
        //public MinMax SalesPriceFilter { get; set; }

        public int IsParentItem { get; set; }
        public int ObjectKey { get; set; }

        public int ParentItemKey { get; set; }

        public int IsActive { get; set; }

        public int Page { set { PageNumber = value; } get { return PageNumber; } }

        public string Sort { get; set; }

        public string SortColoumn
        {
            get
            {
                if (Sort != null)
                {
                    string[] split = Sort.Split('-');
                    if (split.Length > 1)
                    {
                        return split[0];
                    }
                }
                return null;

            }

        }
        public int SortDirection
        {
            get
            {
                if (Sort != null)
                {
                    string[] split = Sort.Split('-');
                    if (split.Length > 1)
                    {
                        return (split[1].Equals("asc") ? 0 : 1);
                    }
                }
                return 0;

            }

        }


        public bool IsFirstLoadDone { get; set; }

        public decimal DefaultQuantity { get; set; }

        public int FromItemKey { get; set; } = 1;
        public ItemRetrivalDto()
        {
            PageSize = 10;
            PageNumber = 1;
            SearchQuery = "";
            ItemCategory1Key = 1;
            ItemCategory2Key = 1;
            ItemCategory3Key = 1;
            ItemCategory4Key = 1;
            ItemCategory5Key = 1;
            ItemCategory6Key = 1;
            ItemCategory7Key = 1;
            ItemCategory8Key = 1;
            ItemCategory9Key = 1;
            ItemCategory10Key = 1;
            ItemCategory11Key = 1;
            ItemCategory12Key = 1;
            ItemTypeKey = 1;
            UnitKey = 1;
            ServiceUnitKey = 1;
            ParentItemKey = 1;
            //CostPriceFilter = new MinMax()
            //{
            //    MinValue = 0,
            //    MaxValue = 1000
            //};

            //SalesPriceFilter = new MinMax()
            //{
            //    MinValue = 0,
            //    MaxValue = 1000
            //};
            IsParentItem = -1;
            ObjectKey = 1;
            IsActive = -1;
        }



        public string Filter { get; set; }

        public string SearchQueryFromFilter
        {
            get
            {
                if (Filter != null)
                {
                    Regex regex = new Regex("(\'.*?\')", RegexOptions.Singleline);
                    MatchCollection matches = regex.Matches(Filter);
                    if (matches.Count > 0)
                    {
                        return matches[0].Value.Replace("'","");
                    }
                }
                return null;


            }

        }


        public int PreKey { get; set; } = 1;

        public int TransactionTypeKey { get; set; } = 1;

        public bool GroupByColor { get; set; }

    }
    public class ItemRateFilterDTO
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int LocationKey { get; set; } = 1;
        public int ProjectKey { get; set; } = 1;
        public int TransactionTypeKey { get; set; } = 1;
        public int PayementTermKey { get; set; } = 1;
        public int ItemTypeKey { get; set; } = 1;
        public int AddressKey { get; set; } = 1;
        public int AccountKey { get; set; } = 1;
        public int ItemKey { get; set; }

        public int ControlConKey { get; set; } = 1;
        public int BussinessUnitKey { get; set; } = 1;

        public int CompanyKey { get; set; } = 1;

        public DateTime TransactionDate { get; set; }

        public ItemRateFilterDTO()
        {
            FromDate = DateTime.Today.AddYears(-10);
            ToDate = DateTime.Now;
        }

    }

    public class ItemRelationDto
    {
        public int ItemRelationKey { get; set; } = 1;

        public int ItemKey { get; set; } = 1;

        public int ItemRelationTypeKey { get; set; } = 1;

        public int  RelationTypeKey { get; set; } = 1;

        public string UUID { get; set; }
    }

    public class ItemBudjet
    {
        public int ItmBgtKy { get; set; }
        public int ItemKey { get; set; }
        public int AccountKey { get; set; }
        public int ProjectKey { get; set; }
        public int BUKey { get; set; }
        public int AddressKey { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string ItemName { get; set; }
        public string AccountName { get; set; }
        public string AddressName { get; set; }
        public string ProjectName { get; set; }
        public string BUNm { get; set; }
        public int UnitKy { get; set; }
        public string UnitNm { get; set; }
        public decimal BudgetAmount { get; set; }
        public double Qty { get; set; }

        public ItemBudjet()
        {
            Qty = 0.00;
            PageNumber = 1;
            PageSize = 10;
            ItemKey = 1;
            AccountKey = 1;
            ProjectKey = 1;
            BUKey = 1;
            AddressKey = 1;
            FromDate = DateTime.Now.ToString("dd/MM/yyyy");
            ToDate = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    public class EditItemBudget
    {
        public int EditItmKey { get; set; }
        public int AccKy { get; set; }
        public int AdrKy { get; set; }
        public int PrjKy { get; set; }
        public int EditBUKey { get; set; }
        public string BudgetDate { get; set; }
        public decimal Amt { get; set; }
        public double EditQty { get; set; }
        public int EditItmBgtKy { get; set; }
    }

    public class CreateItemBudget
    {
        public int ItmKey { get; set; }
        public int AccKey { get; set; }
        public int AdrKey { get; set; }
        public int PrjKey { get; set; }
        public int BUKy { get; set; }
        public string FrmDt { get; set; }
        public string ToDt { get; set; }
        public decimal Amount { get; set; }
        public double AddQty { get; set; }
        public CreateItemBudget()
        {
            AccKey = 1;
            AdrKey = 1;
            ItmKey = 1;
            PrjKey = 1;
            BUKy = 1;
            Amount = 0;
            AddQty = 0;

        }

    }

    //item profile v3.0
    public class URLDefinition
    {
        public string URL { get; set; }

    }

    public class ItemOpenRequest
    {
        public long ItemKey { get; set; } 

        public long ElementKey { get; set; } 

    }


}
