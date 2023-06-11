using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.Dashboard
{
    public class Inventory { }
    public class StockValuation
    {
        public int Brandkey { get; set; } = 1;
        public string BrandName { get; set; } = "";
        public string BrandCode { get; set; } = "";
        public decimal Value { get; set; }
        public decimal Quantity { get; set; }
        public IList<StockCategory> StockCategories { get; set; }

        public StockValuation()
        {
            StockCategories = new List<StockCategory>();
        }
    }

    public class StockCategory
    {
        public string CategoryName { get; set; } = "";
        public string CategoryCode { get; set; } = "";
        public int CategoryKey { get; set; } = 1;
        public decimal CategoryValue { get; set; }
        public decimal QuantityOfCategory { get; set; }
        public IList<InventoryItemDetails> ItemDetails { get; set; }

        public StockCategory()
        {
            ItemDetails = new List<InventoryItemDetails>();
        }

        public decimal GetValueTotal()
        {
            return ItemDetails.Sum(x => x.Value);
        }

    }
    public class InventoryItemDetails
    {
        public int LineNumber { get; set; }
        public string ItemName { get; set; } = "";
        public string ItemCode { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Value { get; set; }
    }
}
