using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.DTO.Object
{
    public class ObjectFormRequest
    {
        public long MenuKey { get; set; }

    }

    public class DenominationRequest
    {
        public int ObjectKey { get; set; } = 1;
        public int WorkstationKey { get; set; } = 1;
        public int ShiftKey { get; set; } = 1;
        public DateTime EffectiveDate { get; set; } = DateTime.Today;
    }

    public class DenominationEntry
    {
        public CodeBaseResponse Denomination { get; set; }
        public int ControlConKy { get; set; }
        public int ShiftKey { get; set; }
        public int WorkstationDenominationKey { get; set; } = 1;
        public int WorkStationKey { get; set; } = 1;
        public DateTime EffectiveDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal Multiplier { get; set; }

        public decimal GetRowTotal()
        {
            return Quantity * Multiplier;
        }
    }
}
