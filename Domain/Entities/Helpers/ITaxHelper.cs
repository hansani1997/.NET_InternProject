using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.Helpers
{
    public interface TaxableTransactionLine
    {
        public decimal ItemTaxType1Per { get; set; }
        public decimal ItemTaxType2Per { get; set; }
        public decimal ItemTaxType3Per { get; set; }
        public decimal ItemTaxType4Per { get; set; }
        public decimal ItemTaxType5Per { get; set; }
        public decimal ItemTaxType6Per { get; set; }


        public decimal ItemTaxType1 { get; set; }
        public decimal ItemTaxType2 { get; set; }
        public decimal ItemTaxType3 { get; set; }
        public decimal ItemTaxType4 { get; set; }
        public decimal ItemTaxType5 { get; set; }
        public decimal ItemTaxType6 { get; set; }


        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TransactionRate { get; set; }
        public decimal TransactionQuantity { get; set; }

        public bool IsRateInclusiveTaxType1 { get; set; }
        public bool CalculateTaxOnPreDiscountValue { get; set; }

        decimal GetLineTotalWithDiscount();





    }
}
