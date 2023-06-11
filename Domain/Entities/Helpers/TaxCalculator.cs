using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.Helpers
{
    public class TaxCalculator
    {


        public void CalculateTaxes(TaxableTransactionLine line)
        {
            CalculateTaxtType1(line);
        }

        private static void CalculateTaxtType1(TaxableTransactionLine line)
        {
            if (!line.IsRateInclusiveTaxType1)
            {
                decimal lineTotal = line.GetLineTotalWithDiscount();
                line.ItemTaxType1 = (lineTotal * line.ItemTaxType1Per) / 100;
            }           
        }

    }
}
