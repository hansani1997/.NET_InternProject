using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlueLotus.Com.Domain.Entity
{


    public class ItemSimple:BaseEntity
    {
        public string ItemName { get; set; }  // Added
        public int ItemKey { get; set; } // Added
        public string ItemCode { get; set; }  // Added

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
            } }


    }

    public class Item : ItemSimple
    {
        public Item()
        {
            ExpireDate = Convert.ToDateTime("1990/01/01");
        }

        public int LiNo { get; set; }
        public CodeBaseResponse ItemType { get; set; } // Added
        public string EAN { get; set; }  // Added

        public string ItemShortName { get; set; }  // Added
        public string Description { get; set; } // Added
        //public Unit ItemUnit { get; set; }  // Added
        //public Unit ServiceUnit { get; set; }  // Added 
        public string Remarks { get; set; }  // Added
        public decimal CostPrice { get; set; }  // Added
        public decimal SalesPrice { get; set; } // Added
        public decimal OptionalSalesPrice { get; set; } // Added
        public bool IsModifierItem { get; set; } //Added
        public bool IsCompositeItem { get; set; } //Added
        public bool IsPayemntType { get; set; } //Added
        public decimal MaximumDiscount { get; set; }//Added
        public decimal VatPercentage { get; set; }//Added
        public CodeBaseResponse ItemCategory1 { get; set; }
        public CodeBaseResponse ItemCategory2 { get; set; }
        public CodeBaseResponse ItemCategory3 { get; set; }
        public CodeBaseResponse ItemPriceCategory { get; set; }
        public CodeBaseResponse ItemProperty1 { get; set; }
        public CodeBaseResponse ItemProperty2 { get; set; }
        public CodeBaseResponse ItemProperty3 { get; set; }
        public CodeBaseResponse ItemProperty4 { get; set; }
        public Item ParentItem { get; set; }
        public CodeBaseResponse Brand { get; set; }
        public DateTime ExpireDate { get; set; } //Added
        public int ValueForProjectKey { get; set; } //Added

        public decimal SupplierWarranty { get; set; }
        public decimal CustomerWarranty { get; set; }
        public int ProjectId { get; set; } = 1;
        public string ItemComboTitle
        {
            get
            {
                return ItemCode + " - " + ItemName;
            }
        }
        public string PartNumber { get; set; }
        public CodeBaseResponse Model { get; set; }
        public bool IsSerialNumber { get; set; }
        public decimal ReOrderLevel { get; set; }
        public decimal ReOrderQuantity { get; set; }
    }



    public class ItemExtended : Item
    {
        public CodeBaseResponse ItemCategory4 { get; set; }
        public CodeBaseResponse ItemCategory5 { get; set; }
        public CodeBaseResponse ItemCategory6 { get; set; }
        public CodeBaseResponse ItemCategory7 { get; set; }
        public CodeBaseResponse ItemCategory8 { get; set; }
        public CodeBaseResponse ItemCategory9 { get; set; }
        public CodeBaseResponse ItemCategory10 { get; set; }
        public CodeBaseResponse ItemCategory11 { get; set; }
        public CodeBaseResponse ItemCategory12 { get; set; }

        public bool IsMain { get; set; }
        public byte Level { get; set; }

        public CodeBaseResponse AccessLevel { get; set; }
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

}
